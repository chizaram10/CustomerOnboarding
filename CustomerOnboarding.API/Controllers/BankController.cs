using Common.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerOnboarding.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankController : ControllerBase
    {
        private readonly IHttpRequestService _httpRequestService;

        public BankController(IHttpRequestService httpRequestService)
        {
            _httpRequestService = httpRequestService;
        }

        [HttpGet("getbanks")]
        public async Task<IActionResult> GetBanks()
        {
            var response = await _httpRequestService.GetBanks();

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }

}
