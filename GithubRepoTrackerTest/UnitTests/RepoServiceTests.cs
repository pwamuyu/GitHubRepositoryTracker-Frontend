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
    public class RepoServiceTests
    {
        [Fact]
        public async Task GetAllRepos_ReturnsListOfRepos()
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
            
            var paginatedRepo = new PaginatedRepo
            {
                data = new[]
                {
                    new Repo { repositoryName = "Repository1" },
                    new Repo { repositoryName = "Repository2" },
                    new Repo { repositoryName = "Repository3" },
                    new Repo { repositoryName = "Repository4" },
                    new Repo { repositoryName = "Repository5" },
                    new Repo { repositoryName = "Repository6" },
                    new Repo { repositoryName = "Repository7" },
                    new Repo { repositoryName = "Repository8" },
                    new Repo { repositoryName = "Repository9" },
                    new Repo { repositoryName = "Repository10" },
                    new Repo { repositoryName = "Repository11" },
                    new Repo { repositoryName = "Repository12" },
                    new Repo { repositoryName = "Repository13" },
                    new Repo { repositoryName = "Repository14" },
                    new Repo { repositoryName = "Repository15" },
                    new Repo { repositoryName = "Repository16" },
                    new Repo { repositoryName = "Repository17" },
                    new Repo { repositoryName = "Repository18" },
                    new Repo { repositoryName = "Repository19" },
                    new Repo { repositoryName = "Repository20" }
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

                        var paginatedResponse = new PaginatedRepo
                        {
                            data = paginatedRepo.data
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
