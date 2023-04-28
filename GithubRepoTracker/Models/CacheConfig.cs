using Microsoft.Extensions.Caching.Memory;

namespace GithubRepoTracker.Models
{
    public class CacheConfig
    {
        public readonly TimeSpan _CacheDuration;
        private readonly int CacheDuration = 30;
        public CacheConfig()
        {
            _CacheDuration = TimeSpan.FromMinutes(CacheDuration);
        }

    }

}
