using System.ComponentModel.DataAnnotations;

namespace GithubRepoTracker.Models
{
    public class Repo
    {
        public string repositoryName { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public Language language { get; set; }
        public Topic[] repositoryTopics { get; set; }

        [DataType(DataType.DateTime)] 
        public DateTime updatedAt { get; set; }
        public int forksCount { get; set; }
        public int stargazersCount { get; set; }

    }
}
