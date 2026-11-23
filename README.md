## ⛔Never push sensitive information such as client id's, secrets or keys into repositories including in the README file⛔

# Provider Feedback Web
<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">

[![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status/das-providerfeedback-web?repoName=SkillsFundingAgency%2Fdas-providerfeedback-web&branchName=master)](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_build/latest?definitionId=3675&repoName=SkillsFundingAgency%2Fdas-providerfeedback-web&branchName=main)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=SkillsFundingAgency_das-providerfeedback-web&metric=alert_status)](https://sonarcloud.io/project/overview?id=SkillsFundingAgency_das-providerfeedback-web)
[![Jira Project](https://img.shields.io/badge/Jira-Project-blue)](https://skillsfundingagency.atlassian.net/browse/EC-481)
[![Confluence Project](https://img.shields.io/badge/Confluence-Project-blue)](https://skillsfundingagency.atlassian.net/wiki/spaces/NDL/pages/4492754969/Provider+feedback)
[![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?longCache=true&style=flat-square)](https://en.wikipedia.org/wiki/MIT_License)

This repository represents the Provider Feedback Web code base. Provider Feedback is a service that allows providers to view feedback from employers and apprentices. The UI code base is the `das-providerfeedback-web` repository, the inner APIs are the `das-apprentice-feedback-api` and `das-employer-feedback-api` repositories, and the outer API code base is in the `das-apim-endpoints` repository within the `ProviderFeedback` project.

## Developer Setup
### Requirements

In order to run this solution locally you will need the following:
* [.net 10.0](https://www.microsoft.com/net/download/)
* (VS Code Only) [C# Extension](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp)
* [Azurite](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite) (previously known as Azure Storage Emulator)

### Environment Setup

* **appsettings.json** - add the following to your local appsettings.json file.
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "ConfigurationStorageConnectionString": "UseDevelopmentStorage=true;",
  "ConfigNames": "SFA.DAS.ProviderFeedback.Web,SFA.DAS.Provider.DfeSignIn",
  "EnvironmentName": "LOCAL",
  "ResourceEnvironmentName": "LOCAL",
  "Version": "1.0",
  "APPLICATIONINSIGHTS_CONNECTION_STRING": "",
  "AllowedHosts": "*",
  "cdn": {
    "url": "https://das-at-frnt-end.azureedge.net"
  },
  "ProviderFeedbackWeb": {
    "ShowReviewNotice": true,
    "ReviewNoticeDate": "March/April 2025"
  }
}
```
* **Config** - You can find the latest config files in the [das-employer-config](https://github.com/SkillsFundingAgency/das-employer-config) repository:
    * `das-providerfeedback-web/SFA.DAS.ProviderFeedback.Web.json` - Row Key: `SFA.DAS.ProviderFeedback.Web_1.0`
    * `das-shared-config/SFA.DAS.Provider.DfeSignIn.json` - Row Key: `SFA.DAS.Provider.DfeSignIn_1.0`

    Partition Key for both: `LOCAL`

    Either add each row to the `Configuration` table in Azurite (e.g. via Azure Storage Explorer) with the JSON as the `Data` value, or put the config directly in your local `appsettings.json`. Either will work.

* **Outer API config** - In the `SFA.DAS.ProviderFeedback.Web` config row, set `ProviderFeedbackOuterApi.ApiBaseUrl` (and `SubscriptionKey` if pointing at a deployed environment) to your local or AT outer API.
* **Redis** - If `ProviderFeedback.RedisConnectionString` is set in your local config, Redis must be running (e.g. `docker run -d -p 6379:6379 redis`), otherwise startup fails. Set it to `""` to skip Redis and use default data protection locally.
* **DfE Sign-in** - There is no stub login. You need a DfE Sign-in test account with access to the provider service in the test environment.

### Running

* Start Azurite, e.g. run `azurite` from a terminal, or let Visual Studio start it automatically
* Solutions to run in conjunction - To get this solution running successfully you will also need the following solutions running:
    * The Outer API [das-apim-endpoints](https://github.com/SkillsFundingAgency/das-apim-endpoints/tree/main/src/ProviderFeedback) should be available either running locally or accessible in an Azure tenancy.
    * The Inner API [das-apprentice-feedback-api](https://github.com/SkillsFundingAgency/das-apprentice-feedback-api) should be available either running locally or accessible in an Azure tenancy.
    * The Inner API [das-employer-feedback-api](https://github.com/SkillsFundingAgency/das-employer-feedback-api) [SFA.DAS.EmployerFeedback.Api] should be available either running locally or accessible in an Azure tenancy.
    * The Roatp API [das-roatp-api](https://github.com/SkillsFundingAgency/das-roatp-api) should be available either running locally or accessible in an Azure tenancy. This api is used to determine the provider's status.
* Run the `SFA.DAS.ProviderFeedback.Web` project (https profile: https://localhost:7067) 

### Tests

Run all tests from the `src` folder with `dotnet test`.

#### Unit Tests

There are several unit test projects in the solution built using C#, .net 10.0, FluentAssertions, Moq, NUnit, and AutoFixture.
* `SFA.DAS.ProviderFeedback.Application.UnitTests`
* `SFA.DAS.ProviderFeedbackWeb.UnitTests`