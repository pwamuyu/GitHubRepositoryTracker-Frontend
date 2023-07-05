using System.Net;
using GithubRepoTracker.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace GithubRepoTrackerTest.UnitTests
{
    public class ApiAuthServiceTests
    {

        [Fact]
        public async Task GetAccessTokenAsync_SuccessfulResponse_ReturnsAccessToken()
        {
            var inMemorySettings = new System.Collections.Generic.Dictionary<string, string> {
                {"AuthCredentians:UserName", "username"},
                {"AuthCredentians:apiPassword", "password"},
                {"ApiBaseUrl", "https://api.github.com"},
                {"ApiEndpoints:GetAccessToken", "/login"}

            };
         
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"token\": \"token\"}")
            };
            
            
            var mockLogger = new Mock<ILogger<ApiAuthService>>();
            
            var mockHttpClient = new Mock<HttpClient>();
            mockHttpClient
                .Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(httpResponse);
            
            var service = new ApiAuthService(
                mockLogger.Object,
                mockHttpClient.Object,
                configuration);
            // Arrange
            var expectedToken = "token";

            // Act
            var result = await service.GetAccessTokenAsync();

            // Assert
            Assert.Equal(expectedToken, result);
        }

    }
}