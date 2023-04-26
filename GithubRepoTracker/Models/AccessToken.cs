using Newtonsoft.Json;

namespace GithubRepoTracker.Models
{
    public class AccessToken
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("expiration")]
        public DateTime ExpirationTime { get; set; }
    }
}
