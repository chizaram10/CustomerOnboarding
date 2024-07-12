using Common.Core;
using Common.Core.Interfaces.Services;
using Common.Core.Settings;
using Common.Infrastructure.Implementations.Services;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CustomerOnboarding.Test
{
    public class HttpRequestServiceTests
    {
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly IOptions<TestAPIClient> _options;
        private readonly HttpRequestService _httpRequestService;

        public HttpRequestServiceTests()
        {
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();

            var testAPIClient = new TestAPIClient
            {
                Name = "TestClient",
                Url = "https://api.example.com/banks"
            };

            _options = Options.Create(testAPIClient);

            _httpRequestService = new HttpRequestService(_mockHttpClientFactory.Object, _options);
        }

        [Fact]
        public async Task GetBanks_ShouldReturnBanksList_OnSuccess()
        {
            // Arrange
            var responseContent = new
            {
                result = new[]
                {
                    new { BankName = "Bank A", BankCode = "001" },
                    new { BankName = "Bank B", BankCode = "002" }
                }
            };

            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(responseContent))
            };

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _httpRequestService.GetBanks();

            // Assert
            Assert.True(result.Success);
            Assert.Equal(2, result.Data.Count());
        }

        [Fact]
        public async Task GetBanks_ShouldReturnError_OnHttpFailure()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                ReasonPhrase = "Bad Request"
            };

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _httpRequestService.GetBanks();

            // Assert
            Assert.False(result.Success);
            Assert.Equal("400 | Bad Request", result.Message);
        }

        [Fact]
        public async Task GetBanks_ShouldReturnError_OnException()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Throws(new HttpRequestException("Network error"));

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _httpRequestService.GetBanks();

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Network error", result.Message);
        }

        [Fact]
        public async Task GetBanks_ShouldReturnError_OnInvalidJson()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("Invalid JSON")
            };

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _httpRequestService.GetBanks();

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid response object returned.", result.Message);
        }
    }
}
