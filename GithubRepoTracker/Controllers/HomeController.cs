using GithubRepoTracker.Interfaces;
using GithubRepoTracker.Models;
using GithubRepoTracker.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GithubRepoTracker.Controllers
{
    public class HomeController : Controller
    {

        
        private readonly TopicInterface _topicInterface;
        private readonly RepoInterface _repoInterface;
        private LanguageInterface _languageInterface;
        

        public HomeController( RepoInterface repoInterface, TopicInterface topicInterface,LanguageInterface languageInterface)
        {
            _repoInterface = repoInterface;
            _topicInterface = topicInterface;
            _languageInterface = languageInterface;
             

        }

        // Get: repos action 
       
        public IActionResult Index(RepoListViewModel repolistViewModel,string? topic,string language,int? page)
        {
            List<Repo> repos = new List<Repo>();
            if (repolistViewModel.SelectedTopic != null) topic = repolistViewModel.SelectedTopic;
            if (repolistViewModel.SelectedLanguage != null) language = repolistViewModel.SelectedLanguage;
            if (topic != null)
            {
                repos = _repoInterface.ReposPerTopic(topic).ToList();

                repolistViewModel.SelectedTopic = topic;
            }
            else if(language != null)
            {
                repos = _repoInterface.ReposPerLanguage(language).ToList();

                repolistViewModel.SelectedLanguage = language;

                
            }
            else
            {
                repos = _repoInterface.GetAllRepos().Result.ToList();
            }
            
            if (repolistViewModel.SortOrderFork == "fork_desc" && repolistViewModel.SortParam == "forks")
            {
                repolistViewModel.Repos = repos.OrderByDescending(x => x.forksCount);
                repolistViewModel.SortOrder = "fork_desc";
                repolistViewModel.SortOrderDate = "date_desc";
                repolistViewModel.SortOrderFork = "fork_asc";
                repolistViewModel.SortOrderStar = "star_desc";
                repolistViewModel.SortParam = "forks";
            }
            else if (repolistViewModel.SortOrderFork == "fork_asc" && repolistViewModel.SortParam == "forks")
            {
                repolistViewModel.Repos = repos.OrderBy(x => x.forksCount);
                repolistViewModel.SortOrder = "fork_asc";
                repolistViewModel.SortOrderDate = "date_desc";
                repolistViewModel.SortOrderFork = "fork_desc";
                repolistViewModel.SortOrderStar = "star_desc";
                repolistViewModel.SortParam = "forks";

            }
            else if (repolistViewModel.SortOrderStar == "star_desc" && repolistViewModel.SortParam == "stars")
            {
                repolistViewModel.Repos = repos.OrderByDescending(x => x.stargazersCount);
                repolistViewModel.SortOrder = "star_desc";
                repolistViewModel.SortOrderDate = "date_desc";
                repolistViewModel.SortOrderFork = "fork_desc";
                repolistViewModel.SortOrderStar = "star_asc";
                repolistViewModel.SortParam = "stars";
            }
            else if (repolistViewModel.SortOrderStar == "star_asc"&& repolistViewModel.SortParam == "stars")
            {
                repolistViewModel.Repos = repos.OrderBy(x => x.stargazersCount);
                repolistViewModel.SortOrder = "star_asc";
                repolistViewModel.SortOrderDate = "date_desc";
                repolistViewModel.SortOrderFork = "fork_desc";
                repolistViewModel.SortOrderStar = "star_desc";
                repolistViewModel.SortParam = "stars";
            }
            else if (repolistViewModel.SortOrderDate == "date_asc" && repolistViewModel.SortParam == "date")
            {
                repolistViewModel.Repos = repos.OrderBy(x => x.updatedAt);
                repolistViewModel.SortOrder = "date_asc";
                repolistViewModel.SortOrderDate = "date_desc";
                repolistViewModel.SortOrderFork = "fork_desc";
                repolistViewModel.SortOrderStar = "star_desc";
                repolistViewModel.SortParam = "date";
            }
            else
            {
                repolistViewModel.Repos = repos.OrderByDescending(x => x.updatedAt);
                repolistViewModel.SortOrder = "date_desc";
                repolistViewModel.SortOrderDate = "date_asc";
                repolistViewModel.SortOrderFork = "fork_desc";
                repolistViewModel.SortOrderStar = "star_desc";
                repolistViewModel.SortParam = "date";
            }

            var pager = new Pager(repolistViewModel.Repos.Count(), page);
            repolistViewModel.Repos = repolistViewModel.Repos.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);
            repolistViewModel.Pager = pager;

            repolistViewModel.Topics =_topicInterface.GetAllTopics().Result;
            repolistViewModel.Languages = _languageInterface.GetAllLanguages().Result;


            return View(repolistViewModel);
        }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}