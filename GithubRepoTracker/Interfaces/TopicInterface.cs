using GithubRepoTracker.Models;

namespace GithubRepoTracker.Interfaces
{
    public interface TopicInterface
    {
        Task<IEnumerable<Topic>> GetAllTopics();
    }
}
