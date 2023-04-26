namespace GithubRepoTracker.Interfaces
{
    public interface ApiAuthInterface
    {
        Task<string> GetAccessTokenAsync ();
    }
}
