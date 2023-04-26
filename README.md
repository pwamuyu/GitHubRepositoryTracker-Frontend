# Introduction

This project is a web application retrieves data from an API.

Users can view repositories sorted by updatedAt date, number of stars, or number
of forks. Additionally, users can filter repositories by topic or language and
sort them in ascending or descending order.

It is developed using ASP.NET Core MVC.

# Installation process

1. Clone the repository using git clone [repository-url](https://MicrosoftLeapClassroom@dev.azure.com/MicrosoftLeapClassroom/GitRepositoryTracker/_git/GitHubRepositoryTracker-FrontEnd)
2. Navigate to the project folder and open the solution file in Visual Studio.

# Software dependencies

.NET 6.0 or higher
Visual Studio 2022

# Configuration settings

appsetting.config
```
"ApiBaseUrl": "your base url",
  "ApiEndpoints": {
    "GetAllReposEndpoint": "The endpoint that returns all the repositories",
    "GetAllTopicsEndpoint": "The endpoint that returns all the topics",
    "GetAllLanguagesEndpoint": "The endpoint that returns all the languages",
    "GetAccessToken": "The endpoint that returns the access token"
  },
  "AllowedHosts": "*",
  "Token": "Your access token",
  "AuthCredentians": {
    "UserName": "The app username",
    "apiPassword": "The password used to get the access token"
  }
  ```


# Build

To build the code:

* Open GithubRepoTracker.sln with Visual Studio
* Update the config file.
* From the Build menu, select "Build GithubRepoTracker"

# Source Code

## [Models](GithubRepoTracker/Models)

It has all the objects and its properties.

## [Interfaces](GithubRepoTracker/Interfaces)

The methods are defined here.

## [Services](GithubRepoTracker/Services)

Contains the implementations of the methods defined in the interfaces.

## [ViewModels](GithubRepoTracker/ViewModels)

Has all the entities required by the views.

