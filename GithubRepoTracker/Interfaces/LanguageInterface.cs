using GithubRepoTracker.Models;

namespace GithubRepoTracker.Interfaces
{
    public interface LanguageInterface
    {
        Task<IEnumerable<Language>> GetAllLanguages();
    }
}
