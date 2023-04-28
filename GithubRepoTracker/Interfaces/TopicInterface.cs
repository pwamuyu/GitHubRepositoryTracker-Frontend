using GithubRepoTracker.Models;

namespace GithubRepoTracker.Interfaces
{
    public interface TopicInterface
    {
        Task<List<Topic>> GetAllTopics();
    }
}
