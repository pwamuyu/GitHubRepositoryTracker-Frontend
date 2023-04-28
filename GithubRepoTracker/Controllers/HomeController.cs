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
       
        public async Task<IActionResult> Index(RepoListViewModel repolistViewModel,string? topic,string language,int? page)
        {
            List<Repo> repos = new List<Repo>();

            //If the repos have been filtered by topic happens in case of changing page or sorting
            if (repolistViewModel.SelectedTopic != null) topic = repolistViewModel.SelectedTopic;

            //If the repos have been filtered by language happens in case of changing page or sorting
            if (repolistViewModel.SelectedLanguage != null) language = repolistViewModel.SelectedLanguage;

            
            if (topic != null)
            {
                //Filter the repos by topic
                repos = _repoInterface.ReposPerTopic(topic).ToList();

                //and set the SelectedTopic to be the topic
                repolistViewModel.SelectedTopic = topic;
            }
            else if(language != null)
            {
                //Filter the repos by topic
                repos = _repoInterface.ReposPerLanguage(language).ToList();

                //and set the SelectedTopic to be the topic
                repolistViewModel.SelectedLanguage = language;

                
            }
            else
            {
                //Return all the repos if non of the filtering paramenters is there
                repos = await _repoInterface.GetAllRepos();
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

            repolistViewModel.Topics = await _topicInterface.GetAllTopics();
            repolistViewModel.Languages = await _languageInterface.GetAllLanguages();


            return View(repolistViewModel);
        }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}