using GithubRepoTracker.Interfaces;
using GithubRepoTracker.Models;
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

        public RepoService(HttpClient client, IConfiguration configuration, ApiAuthInterface apiAuthInterface)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _configuration = configuration;
            _apiAuthInterface = apiAuthInterface;

            BaseUrl = _configuration.GetValue<string>("ApiBaseUrl");
            //Token = _configuration.GetValue<string>("Token");
            

        }

        //getting all the repositories
        public async Task<IEnumerable<Repo>> GetAllRepos()
        {
            var Token = await _apiAuthInterface.GetAccessTokenAsync();
            var repos = new List<Repo>();

          
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



            }
            return repos;
        }


        //getting repos per language

        public IEnumerable<Repo> ReposPerLanguage(string language)
        {
            IEnumerable<Repo> repos =
            from repo in GetAllRepos().Result
            where repo.language.languageName == language
            select repo;

            return repos;
        }

        //getting repos per topic

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
