using System.Net;
using System.Web;
using GithubRepoTracker.Models;
using GithubRepoTracker.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;

namespace GithubRepoTrackerTest.UnitTests
{
    public class LanguageServiceTests
    {
        [Fact]
        public async Task GetAllLanguages_ReturnsListOfLanguages()
        {
            var mockLogger = new Mock<ILogger<LanguageService>>();

            var inMemorySettings = new System.Collections.Generic.Dictionary<string, string> {
                {"AuthCredentians:UserName", "username"},
                {"AuthCredentians:apiPassword", "password"},
                {"ApiBaseUrl", "https://api.github.com"},
                {"ApiEndpoints:GetAccessToken", "/login"},
                {"ApiEndpoints:GetAllLanguagesEndpoint", "/languages"}

            };
            
            var paginatedLanguages = new PaginatedLanguage
            {
                data = new[]
                {
                    new Language { languageName = "Java" },
                    new Language { languageName = "C#" },
                    new Language { languageName = "Python" },
                    new Language { languageName = "Scala" },
                    new Language { languageName = "Golang" },
                    new Language { languageName = "DOTNET" },
                    new Language { languageName = "C" },
                    new Language { languageName = "C++" },
                    new Language { languageName = "Lisp" },
                    new Language { languageName = "JavaScript" },
                    new Language { languageName = "Ruby" },
                    new Language { languageName = "PHP" },
                    new Language { languageName = "Swift" },
                    new Language { languageName = "Rust" },
                    new Language { languageName = "TypeScript" },
                    new Language { languageName = "Kotlin" },
                    new Language { languageName = "Perl" },
                    new Language { languageName = "Haskell" },
                    new Language { languageName = "Objective-C" },
                    new Language { languageName = "Assembly" }
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

                        var paginatedResponse = new PaginatedLanguage
                        {
                            data = paginatedLanguages.data
                        };

                        var responseContent = JsonConvert.SerializeObject(paginatedResponse);

                        return new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new StringContent(responseContent)
                        };
                    }
                    // Return an empty page for any other page number
                    var emptyResponseContent = JsonConvert.SerializeObject(new PaginatedLanguage());

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
            
            var service = new LanguageService(
                mockLogger.Object,
                client,
                configuration,
                api);

            // Act
            var languages = await service.GetAllLanguages();

            // Assert
            Assert.Equal(20, languages.Count);
        }
    }
}

