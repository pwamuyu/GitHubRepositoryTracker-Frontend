namespace GithubRepoTracker.Models
{
    public class PaginatedRepo
    {
        public Repo[] data { get; set; }

        public int totalPages { get; set; }
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
    }
}
