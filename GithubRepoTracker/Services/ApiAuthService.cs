using GithubRepoTracker.Interfaces;
using GithubRepoTracker.Models;
using Newtonsoft.Json;
using System.Text;

namespace GithubRepoTracker.Services
{
    public class ApiAuthService : ApiAuthInterface
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private readonly string BaseUrl;
        public ApiAuthService(HttpClient client, IConfiguration configuration)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _configuration = configuration;

            BaseUrl = _configuration.GetValue<string>("ApiBaseUrl");
        }

        public async Task<string> GetAccessTokenAsync()
        {
            string token = "";

            var user = new User()
            {
                userName = _configuration.GetValue<string>("AuthCredentians:UserName"),
            
                password = _configuration.GetValue<string>("AuthCredentians:apiPassword")
            };


            var builder = new UriBuilder(BaseUrl + _configuration.GetValue<string>("ApiEndpoints:GetAccessToken"));
            var url = builder.ToString();

            var userJson = JsonConvert.SerializeObject(user);
            var data = new StringContent(userJson, Encoding.UTF8, "application/json");


            var res = await _client.PostAsync(url, data);

            if (res.IsSuccessStatusCode)
            {


                var result = await res.Content.ReadAsStringAsync();
                var deserializedRes = JsonConvert.DeserializeObject<AccessToken>(result);
                token = deserializedRes.Token;


            }
            return token;
        }
    }
}
