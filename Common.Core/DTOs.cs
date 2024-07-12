using System.ComponentModel.DataAnnotations;

namespace Common.Core
{
    public record OnboardCustomerRequestDTO([Required, Phone]string PhoneNumber, [Required, EmailAddress]string Email, [Required]string Password, [Required]int StateId, [Required]int LGAId);
    public record OTPVerificationRequestDTO([Required, EmailAddress]string Email, [Required, RegularExpression(@"^\d{6}$")]string Otp);
    public record BankResponseDTO(string BankName, string BankCode);
    public record ServiceResponse(bool Success, string Message);
    public record ServiceResponse<T>(bool Success, string Message, T Data);
    public class CustomerDTO
    {
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string LGA { get; set; } =  string.Empty;
        public bool IsVerified { get; set; }
    }
}
