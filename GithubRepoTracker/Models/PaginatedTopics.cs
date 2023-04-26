namespace GithubRepoTracker.Models
{
    public class PaginatedTopics
    {
        public Topic[] data { get; set; }

        public int totalPages { get; set; }
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
    }
}
