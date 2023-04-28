using GithubRepoTracker.Models;

namespace GithubRepoTracker.Interfaces
{
    public interface RepoInterface
    {


        Task<List<Repo>> GetAllRepos();
        IEnumerable<Repo> ReposPerTopic(string topic);
        IEnumerable<Repo> ReposPerLanguage(string language);
       
    }
}
