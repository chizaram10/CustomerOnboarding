using Common.Core;
using Common.Core.Interfaces.Services;
using Common.Core.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Common.Infrastructure.Implementations.Services
{
    public class HttpRequestService : IHttpRequestService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly TestAPIClient _testAPIClient;

        public HttpRequestService(IHttpClientFactory clientFactory, IOptions<TestAPIClient> options1)
        {
            _clientFactory = clientFactory;
            _testAPIClient = options1.Value;;
        }
        public async Task<ServiceResponse<IEnumerable<BankResponseDTO>>> GetBanks()
        {
            HttpClient httpClient = _clientFactory.CreateClient(_testAPIClient.Name);
            var uriBuilder = new UriBuilder($"{_testAPIClient.Url}");
            string requestUri = uriBuilder.ToString();

            try
            {
                using (HttpResponseMessage response = await httpClient.GetAsync(requestUri))
                {
                    string stringResult = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        return new ServiceResponse<IEnumerable<BankResponseDTO>>(false, $"{(int)response.StatusCode} | {response.ReasonPhrase}", null!);
                    }

                    if (stringResult == "Invalid JSON")
                    {
                        return new ServiceResponse<IEnumerable<BankResponseDTO>>(false, "Invalid response object returned.", null!);
                    }

                    var responseObject = JObject.Parse(stringResult);

                    var resultArray = responseObject["result"]?.ToString();
                    var formResponse = JsonConvert.DeserializeObject<List<BankResponseDTO>>(resultArray!);

                    if (formResponse != null)
                    {
                        return new ServiceResponse<IEnumerable<BankResponseDTO>>(true, "Suucessfully retrieved list of banks,", formResponse);
                    }
                    else
                    {
                        return new ServiceResponse<IEnumerable<BankResponseDTO>>(false, "Unable to parse response from Get Banks API.", null!);
                    }
                }
            }
            catch (Exception ex)
            {
                return new ServiceResponse<IEnumerable<BankResponseDTO>>(false, ex.Message, null!);
            }
        }
    }
}
