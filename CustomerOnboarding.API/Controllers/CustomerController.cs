using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerOnboarding.API.Controllers
{
    using Common.Core;
    using Common.Core.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost("onboard-customer")]
        public async Task<IActionResult> OnboardCustomer(OnboardCustomerRequestDTO request)
        {
            var response = await _customerService.OnboardCustomerAsync(request);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(OTPVerificationRequestDTO request)
        {
            var response = await _customerService.VerifyOtpAsync(request);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpGet("get-customers")]
        public async Task<IActionResult> GetAllCustomers()
        {
            var response = await _customerService.GetAllCustomersAsync();

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
