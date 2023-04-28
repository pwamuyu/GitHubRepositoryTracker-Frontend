using GithubRepoTracker.Models;

namespace GithubRepoTracker.Interfaces
{
    public interface LanguageInterface
    {
        Task<List<Language>> GetAllLanguages();
    }
}
