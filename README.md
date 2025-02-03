## ⛔Never push sensitive information such as client id's, secrets or keys into repositories including in the README file⛔

# Provider Feedback Web
<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">

[![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status/das-providerfeedback-web?repoName=SkillsFundingAgency%2Fdas-providerfeedback-web&branchName=master)](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_build/latest?definitionId=2539&repoName=SkillsFundingAgency%2Fdas-providerfeedback-web&branchName=master)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=SkillsFundingAgency_das-providerfeedback-web&metric=alert_status)](https://sonarcloud.io/project/overview?id=SkillsFundingAgency_das-providerfeedback-web)
[![Jira Project](https://img.shields.io/badge/Jira-Project-blue)](https://skillsfundingagency.atlassian.net/browse/QF-72)
[![Confluence Project](https://img.shields.io/badge/Confluence-Project-blue)](https://skillsfundingagency.atlassian.net/wiki/spaces/NDL/pages/3776446580/Provider+Feedback+-+QF)
[![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?longCache=true&style=flat-square)](https://en.wikipedia.org/wiki/MIT_License)

This repository represents the Provider Feedback Web code base. Provider Feedback is a service that allows providers to view their feedback from employers and apprentices.Either way, the UI code base is the `das-providerfeedback-web` repository, the innner api's are the `das-apprentice-feedback-api` repository and `das-provide-feedback-employer` repository , and the outer API code base is in the `das-apim-endpoints` repository within the `ProviderFeedback` project. It should be noted that this service is integrated into the apprentice account which means to run it locally you need other services running simultaneously. 

## Developer Setup
### Requirements

In order to run this solution locally you will need the following:
* [.net 8.0](https://www.microsoft.com/net/download/)
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
  "ConfigNames": "SFA.DAS.ProviderFeedback.Web",
  "EnvironmentName": "LOCAL",
  "ResourceEnvironmentName": "LOCAL",
  "Version": "1.0",
  "APPINSIGHTS_INSTRUMENTATIONKEY": "",
  "AllowedHosts": "*",
  "cdn": {
    "url": "https://das-at-frnt-end.azureedge.net"
  },
  "ProviderFeedbackWeb": {
    "ShowReviewNotice": true,
    "ReviewNoticeDate": "March/April 2024"
  }
```
* **Config** - You can find the latest config file in [das-employer-config](https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-providerfeedback-web/SFA.DAS.ProviderFeedback.Web.json) repository.

* **Azure Table Storage Explorer** - There is a choice on whether to add the row key and data to the Azure Table Storage Explorer or have that config in the appsettings.json file. Either will work. 

    Azure Table Storage config

    Row Key: SFA.DAS.ProviderFeedback.Web_1.0

    Partition Key: LOCAL

### Running

* Start Azurite e.g. using a command `C:\Program Files (x86)\Microsoft SDKs\Azure\Storage Emulator>AzureStorageEmulator.exe start`
* Solutions to run in conjunction - To get this solution running successfully you will also need the following solutions running:
    * The Outer API [das-apim-endpoints](https://github.com/SkillsFundingAgency/das-apim-endpoints/tree/master/src/ProviderFeedback) should be available either running locally or accessible in an Azure tenancy.
    * The Inner API [das-apprentice-feedback-api](https://github.com/SkillsFundingAgency/das-apprentice-feedback-api) should be available either running locally or accessible in an Azure tenancy.
    * The Inner API [das-provide-feedback-employer [ESFA.DAS.EmployerProvideFeedback.Api]](https://github.com/SkillsFundingAgency/das-provide-feedback-employer) should be available either running locally or accessible in an Azure tenancy.
    * `das-apprentice-login-service` to get through the authentication for the frontend. 
    * `das-apprentice-accounts-api` for the login connected to the authentication on the frontend. 
* Run the solution 

### Tests

This codebase includes unit ,which are organized into separate projects with appropriate names.

#### Unit Tests

There are several unit test projects in the solution built using C#, .net 8.0, FluentAssertions, Moq, NUnit, and AutoFixture.
* `SFA.DAS.ProviderFeedback.Application.UnitTests
* `SFA.DAS.ProviderFeedbackWeb.UnitTests`