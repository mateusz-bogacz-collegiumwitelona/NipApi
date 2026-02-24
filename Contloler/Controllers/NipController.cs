using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace Contloler.Controllers
{
    [Route("api/search")]
    [ApiController]
    public class NipController : ControllerBase
    {
        public readonly INipServices _nipServices;

        public NipController(INipServices nipServices)
        {
            _nipServices = nipServices;
        }

        [HttpGet("nip/{nip}")]
        public async Task<IActionResult> GetInfoByNip(string nip)
        {
            var result = await _nipServices.GetInfoByNip(nip);

            return result.IsSuccess
               ? StatusCode(result.StatusCode, result.Data)
               : StatusCode(result.StatusCode, new
               {
                   success = false,
                   message = result.Message,
                   errors = result.Errors
               });
        }
    }
}
