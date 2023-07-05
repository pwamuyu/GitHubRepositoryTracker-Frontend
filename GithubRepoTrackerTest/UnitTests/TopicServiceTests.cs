using System.Net;
using System.Web;
using GithubRepoTracker.Interfaces;
using GithubRepoTracker.Models;
using GithubRepoTracker.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;

namespace GithubRepoTrackerTest.UnitTests
{
    public class TopicServiceTests
    {
        [Fact]
        public async Task GetAllTopics_ReturnsListOfTopics()
        {
                      var mockLogger = new Mock<ILogger<RepoService>>();

            var inMemorySettings = new System.Collections.Generic.Dictionary<string, string> {
                {"AuthCredentians:UserName", "username"},
                {"AuthCredentians:apiPassword", "password"},
                {"ApiBaseUrl", "https://api.github.com"},
                {"ApiEndpoints:GetAccessToken", "/login"},
                {"ApiEndpoints:GetAllLanguagesEndpoint", "/languages"}

            };
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            
            var paginatedTopics = new PaginatedTopics
            {
                data = new[]
                {
                    new Topic { topicId = 1, topicName = "Topic1" },
                    new Topic { topicId = 2, topicName = "Topic2" },
                    new Topic { topicId = 3, topicName = "Topic3" },
                    new Topic { topicId = 4, topicName = "Topic4" },
                    new Topic { topicId = 5, topicName = "Topic5" },
                    new Topic { topicId = 6, topicName = "Topic6" },
                    new Topic { topicId = 7, topicName = "Topic7" },
                    new Topic { topicId = 8, topicName = "Topic8" },
                    new Topic { topicId = 9, topicName = "Topic9" },
                    new Topic { topicId = 10, topicName = "Topic10" },
                    new Topic { topicId = 11, topicName = "Topic11" },
                    new Topic { topicId = 12, topicName = "Topic12" },
                    new Topic { topicId = 13, topicName = "Topic13" },
                    new Topic { topicId = 14, topicName = "Topic14" },
                    new Topic { topicId = 15, topicName = "Topic15" },
                    new Topic { topicId = 16, topicName = "Topic16" },
                    new Topic { topicId = 17, topicName = "Topic17" },
                    new Topic { topicId = 18, topicName = "Topic18" },
                    new Topic { topicId = 19, topicName = "Topic19" },
                    new Topic { topicId = 20, topicName = "Topic20" }
                }
            };

            var tokenResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"token\": \"token\"}")
            };
         
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var tokenHttpMock = new Mock<HttpClient>();
            tokenHttpMock
                .Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tokenResponse);

            var mock = new Mock<HttpMessageHandler>();
            mock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage request, CancellationToken cancellationToken)  =>
                {
                    var query = HttpUtility.ParseQueryString(request.RequestUri.Query);
                    var pageNumber = int.Parse(query["pageNumber"]);
                    var pageSize = int.Parse(query["pageSize"]);

                    // Check if requested page number is 1
                    if (pageNumber == 1)
                    {

                        var paginatedResponse = new PaginatedTopics
                        {
                            data = paginatedTopics.data
                        };

                        var responseContent = JsonConvert.SerializeObject(paginatedResponse);

                        return new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new StringContent(responseContent)
                        };
                    }
                    // Return an empty page for any other page number
                    var emptyResponseContent = JsonConvert.SerializeObject(new PaginatedRepo());

                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(emptyResponseContent)
                    };
                });

            var client = new HttpClient(mock.Object);

            var mockApiLogger = new Mock<ILogger<ApiAuthService>>();

            
            var api = new ApiAuthService(
                mockApiLogger.Object,
                tokenHttpMock.Object,
                configuration);
            
            var service = new RepoService(
                mockLogger.Object,
                client,
                configuration,
                api,
                memoryCache);

            // Act
            var languages = await service.GetAllRepos();

            // Assert
            Assert.Equal(20, languages.Count);
        }
    }
}
