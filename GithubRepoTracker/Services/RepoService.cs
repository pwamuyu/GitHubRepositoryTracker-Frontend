using GithubRepoTracker.Interfaces;
using GithubRepoTracker.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Web;

namespace GithubRepoTracker.Services
{
    public class RepoService : RepoInterface
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private readonly ApiAuthInterface _apiAuthInterface;
        private readonly string BaseUrl;
        private IMemoryCache _memoryCache;

        public RepoService(HttpClient client, IConfiguration configuration, ApiAuthInterface apiAuthInterface, IMemoryCache memoryCache)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _configuration = configuration;
            _apiAuthInterface = apiAuthInterface;
            _memoryCache = memoryCache;

            BaseUrl = _configuration.GetValue<string>("ApiBaseUrl");
            

        }

        //getting all the repositories

        /// <summary>
        /// This method gets repos, it first checks if there is data in the cache before making a call to the API
        /// </summary>
        /// <returns>Returns a list of repos</returns>
        public async Task<List<Repo>> GetAllRepos()
        {

            var cacheKey = "allRepos";
            if (_memoryCache.TryGetValue(cacheKey, out var cachedResponse))
            {
                return cachedResponse as List<Repo>;
            }

            List<Repo> repos = new List<Repo>();


            var Token = await _apiAuthInterface.GetAccessTokenAsync();



            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var pageNumber = 1;
            var pageSize = 20;

            while (true)
            {


                var builder = new UriBuilder(BaseUrl + _configuration.GetValue<string>("ApiEndpoints:GetAllReposEndpoint"));

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

                    var paginatedRepos = JsonConvert.DeserializeObject<PaginatedRepo>(content);

                    if (!paginatedRepos.data.Any())
                    {
                        // no more repos available
                        break;
                    }

                    repos.AddRange(paginatedRepos.data);

                    pageNumber++;
                }
                catch (Exception ex)
                {
                    // handle exception
                    Console.WriteLine(ex.ToString());
                    break;
                }

                _memoryCache.Set(cacheKey, repos);




            }

            return repos;
        }


        //getting repos per language

        /// <summary>
        /// This method  get repos from GetAllRepos method and extracts repos under the language
        /// </summary>
        /// <param name="language"></param>
        /// <returns>Filtered repos as list</returns>

        public IEnumerable<Repo> ReposPerLanguage(string language)
        {
            IEnumerable<Repo> repos =
            from repo in GetAllRepos().Result
            where repo.language.languageName == language
            select repo;

            return repos;
        }

        //getting repos per topic

        /// <summary>
        /// This method  get repos from GetAllRepos methos and extracts repos under the topic
        /// </summary>
        /// <param name="topic"></param>
        /// <returns> Returns a list of repos under the topic passed</returns>
        public IEnumerable<Repo> ReposPerTopic(string topic)
        {
            IEnumerable<Repo> repos = GetAllRepos().Result;
            List<Repo> reposPerTopic = new List<Repo>();
            
            foreach (var repo in repos) 
            { 
                foreach(var repoTopic in repo.repositoryTopics)
                {
                    if (repoTopic.topicName.ToLower() == topic.ToLower() )
                    {
                        reposPerTopic.Add(repo);
                        break;

                    }
                }

            }

            return reposPerTopic;
        }
    }
}
