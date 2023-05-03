using GithubRepoTracker.Models;
using System.ComponentModel.DataAnnotations;

namespace GithubRepoTracker.Data
{
    public class MockData
    {

        public static List<Topic> GetTestTopicItems()
        {
            

            var topics = new List<Topic>
                {
                new Topic { topicId = 1,topicName = "C#"},
                new Topic { topicId = 2,topicName = "javaScript"},
                new Topic { topicId = 3,topicName = "python"},
                new Topic { topicId = 4,topicName = "careers" },
                new Topic { topicId = 5,topicName = "certification"}

            };
            return topics;
        }

        public static List<Repo> GetTestRepoItems()
        {

        var repositories = new List<Repo>
                {
                new Repo
                {
                    repositoryName = "repo1",
                    description = "This is the 1st repo",
                    url = "https://github.com/example/repository1",
                    language = new Language{ languageName ="c#"},
                    repositoryTopics = new Topic[]
                    {
                         new Topic { topicId = 1,topicName = "C#"},
                         new Topic { topicId = 2,topicName = "javaScript"}
                    },
                    stargazersCount = 10,
                    forksCount = 2,
                    updatedAt = new DateTime(2022, 10, 2)

                },
                new Repo
                {
                    repositoryName = "repo2",
                    description = "This is the 2nd repo",
                    url = "https://github.com/example/repository2",
                    language = new Language{ languageName ="python"},
                    repositoryTopics = new Topic[]
                    {
                        new Topic { topicId = 3,topicName = "python"},
                        new Topic { topicId = 4,topicName = "careers" },
                        new Topic { topicId = 5,topicName = "certification"}
                    },
                    stargazersCount = 200,
                    forksCount = 9,
                    updatedAt = new DateTime(2022, 5, 2)

                },
                new Repo
                {
                    repositoryName = "repo2",
                    description = "This is the 2nd repo",
                    url = "https://github.com/example/repository2",
                    language = new Language{ languageName ="javaScript"},
                    repositoryTopics = new Topic[]
                    {
                        new Topic { topicId = 4,topicName = "careers" },
                        new Topic { topicId = 5,topicName = "certification"}
                    },
                    stargazersCount = 200,
                    forksCount = 9,
                    updatedAt = new DateTime(2022, 5, 2)

                }
            };
            return repositories;
        }

        public static List<Language> GetTestLanguageItems()
        {


            var languages = new List<Language>
                {
                new Language{ languageName ="javaScript"},
                new Language{ languageName ="python"},
                new Language{ languageName ="c#"},

            };
            return languages;
        }


    }
}
