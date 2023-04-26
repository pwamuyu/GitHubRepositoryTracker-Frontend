using GithubRepoTracker.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace GithubRepoTracker.ViewModels
{
    public class RepoListViewModel
    {

        public IEnumerable<Repo> Repos { get; set; }
        public IEnumerable<Topic> Topics { get; set; }
        public IEnumerable<Language> Languages { get; set; }
        public string SelectedLanguage { get; set; }
        public string SelectedTopic { get; set; }
        public string SortOrder { get; set; }
        public string SortParam { get; set; }

        public string SortOrderStar { get; set; }
        public string SortOrderFork { get; set; }
        public string SortOrderDate { get; set; }
        public int CurrentPage { get; set; }
        public Pager Pager { get; set; }
    }
}
