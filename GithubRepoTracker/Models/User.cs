using System.ComponentModel.DataAnnotations;

namespace GithubRepoTracker.Models
{
    public class User
    {
        public string userName;

        [DataType(DataType.Password)]
        public string password; 
    }
}
