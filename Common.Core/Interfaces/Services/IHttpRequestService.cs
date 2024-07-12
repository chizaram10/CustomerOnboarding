namespace Common.Core.Interfaces.Services
{
    public interface IHttpRequestService
    {
        Task<ServiceResponse<IEnumerable<BankResponseDTO>>> GetBanks();
    }
}
