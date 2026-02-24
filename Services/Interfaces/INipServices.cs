using DTO.Response;
using Services.Helpers;

namespace Services.Interfaces
{
    public interface INipServices
    {
        Task<ResultHandler<GetNipDataResponse>> GetInfoByNip(string nip);
    }
}
