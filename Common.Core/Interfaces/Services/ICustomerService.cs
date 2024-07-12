using Common.Core.Entities;

namespace Common.Core.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<ServiceResponse> OnboardCustomerAsync(OnboardCustomerRequestDTO request);
        Task<ServiceResponse> VerifyOtpAsync(OTPVerificationRequestDTO request);
        Task<ServiceResponse<IEnumerable<CustomerDTO>>> GetAllCustomersAsync();
    }
}
