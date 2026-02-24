using DTO.Response;
using Microsoft.AspNetCore.Http;
using Services.Helpers;
using Services.Interfaces;
using System.Text.Json;

namespace Services.Services
{
    public class NipServices : INipServices
    {
        private readonly HttpClient _httpClient;

        public NipServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResultHandler<GetNipDataResponse>> GetInfoByNip(string nip)
        {
            try
            {
                nip = nip.Replace("-", "").Trim();

                if (string.IsNullOrEmpty(nip) || nip.Length != 10)
                    return ResultHandler<GetNipDataResponse>
                        .Failure(
                        "Invalid NIP format. NIP should be 10 digits long.",
                        StatusCodes.Status400BadRequest);

                var date = DateTime.UtcNow.ToString("yyyy-MM-dd");
                string url = $"https://wl-api.mf.gov.pl/api/search/nip/{nip}?date={date}";

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return ResultHandler<GetNipDataResponse>
                        .Failure(
                        $"Failed to retrieve data for NIP {nip}. API responded with status code {response.StatusCode}.",
                        StatusCodes.Status502BadGateway);
                }

                var content = await response.Content.ReadAsStringAsync();

                var data = JsonSerializer.Deserialize<GetNipDataResponse>(content);

                if (data?.result?.subject == null)
                    return ResultHandler<GetNipDataResponse>
                            .Failure(
                            $"Failed to parse response for NIP {nip}.",
                            StatusCodes.Status500InternalServerError);

                var subject = data.result.subject;

                var result = new GetNipDataResponse
                {
                    code = data.code,
                    message = data.message,
                    result = new ResultDataResponse
                    {
                        subject = new SubjectResponse
                        {
                           name = subject.name,
                           nip = subject.nip,
                           regon = subject.regon,
                           statusVat = subject.statusVat
                        }
                    }
                };

                if (result == null || result.result?.subject == null)
                    return ResultHandler<GetNipDataResponse>
                            .Failure(
                            $"Failed to map response data for NIP {nip}.",
                            StatusCodes.Status500InternalServerError);

                return ResultHandler<GetNipDataResponse>
                    .Success(
                    $"Successfully retrieved data for NIP {nip}.",
                    StatusCodes.Status200OK,
                    result);

            }
            catch (HttpRequestException httpEx)
            {
                return ResultHandler<GetNipDataResponse>
                    .Failure(
                    $"HTTP request error while retrieving data for NIP {nip}: {httpEx.Message}",
                    StatusCodes.Status502BadGateway);
            }
            catch (Exception ex)
            {
                return ResultHandler<GetNipDataResponse>
                    .Failure(
                    $"An error occurred while retrieving data for NIP {nip}: {ex.Message}",
                    StatusCodes.Status500InternalServerError);
            }
        }
    }
}
