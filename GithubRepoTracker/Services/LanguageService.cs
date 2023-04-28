using GithubRepoTracker.Interfaces;
using GithubRepoTracker.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Web;

namespace GithubRepoTracker.Services
{
    public class LanguageService : LanguageInterface
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private readonly ApiAuthInterface _apiAuthInterface;
        private readonly string BaseUrl;

        public LanguageService(HttpClient client, IConfiguration configuration, ApiAuthInterface apiAuthInterface)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _configuration = configuration;
           _apiAuthInterface = apiAuthInterface;

            BaseUrl = _configuration.GetValue<string>("ApiBaseUrl");

        }
        /// <summary>
        /// Makes a get request to the api to get the languages
        /// </summary>
        /// <returns>A list of languages</returns>

        public async Task<List<Language>> GetAllLanguages()
        {
            var Token = await _apiAuthInterface.GetAccessTokenAsync();
            var language = new List<Language>();


            _client.DefaultRequestHeaders.Clear();
            //Define request data format  
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var pageNumber = 1;
            var pageSize = 20;

            while (true)
            {
                var builder = new UriBuilder(BaseUrl + _configuration.GetValue<string>("ApiEndpoints:GetAllLanguagesEndpoint"));

                var query = HttpUtility.ParseQueryString(builder.Query);
                query["pageNumber"] = pageNumber.ToString();
                query["pageSize"] = pageSize.ToString();
                builder.Query = query.ToString();
                var uri = builder.ToString();

                try
                {
                    var response = await _client.GetAsync(uri);

                    if (!response.IsSuccessStatusCode)
                    {
                        // handle error response
                        break;
                    }

                    var content = await response.Content.ReadAsStringAsync();

                    var paginatedLanguages = JsonConvert.DeserializeObject<PaginatedLanguage>(content);

                    if (!paginatedLanguages.data.Any())
                    {
                        // no more topics available
                        break;
                    }

                    language.AddRange(paginatedLanguages.data);

                    pageNumber++;
                }
                catch (Exception ex)
                {
                    // handle exception
                    Console.WriteLine(ex);
                    break;
                }


               
            }
            return language;
        }
    }
}
