using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderFeedback.Domain.ApiResponse;
using SFA.DAS.ProviderFeedback.Domain.Interfaces;
using SFA.DAS.ProviderFeedback.Domain.ProviderAccounts.Api.Requests;
using SFA.DAS.ProviderFeedback.Domain.ProviderAccounts.Api.Responses;
using SFA.DAS.ProviderFeedback.Web.Infrastructure.Authorization;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;

namespace SFA.DAS.ProviderFeedback.Web.UnitTests.Infrastructure.Authorization;

public class WhenHandlingTrainingProviderAllRolesRequirement
{
    [Test, MoqAutoData]
    public async Task Then_Fails_If_No_Provider_Ukprn_Claim(
        int ukprn,
        TrainingProviderAllRolesRequirement providerRequirement,
        TrainingProviderAllRolesAuthorizationHandler authorizationHandler)
    {
        //Arrange
        var claim = new Claim("NotProviderClaim", ukprn.ToString());
        var claimsPrinciple = new ClaimsPrincipal(new[] { new ClaimsIdentity(new[] { claim }) });
        var context = new AuthorizationHandlerContext(new[] { providerRequirement }, claimsPrinciple, null);

        //Act
        await authorizationHandler.HandleAsync(context);

        //Assert
        Assert.IsFalse(context.HasSucceeded);
        Assert.IsTrue(context.HasFailed);
    }

    [Test, MoqAutoData]
    public async Task Then_Fails_If_Non_Numeric_Provider_Ukprn_Claim(
        string ukprn,
        TrainingProviderAllRolesRequirement providerRequirement,
        TrainingProviderAllRolesAuthorizationHandler authorizationHandler)
    {
        //Arrange
        var claim = new Claim(ProviderClaims.ProviderUkprn, ukprn);
        var claimsPrinciple = new ClaimsPrincipal(new[] { new ClaimsIdentity(new[] { claim }) });
        var context = new AuthorizationHandlerContext(new[] { providerRequirement }, claimsPrinciple, null);

        //Act
        await authorizationHandler.HandleAsync(context);

        //Assert
        Assert.IsFalse(context.HasSucceeded);
        Assert.IsTrue(context.HasFailed);
    }

    [Test, MoqAutoData]
    public async Task Then_Fails_If_Provider_Ukprn_Claim_Response_Is_False(
        int ukprn,
        TrainingProviderAllRolesRequirement providerRequirement,
        [Frozen] Mock<IApiClient> apiClient,
        TrainingProviderAllRolesAuthorizationHandler authorizationHandler)
    {
        //Arrange
        var httpContextBase = new Mock<HttpContext>();
        var httpResponse = new Mock<HttpResponse>();
        httpContextBase.Setup(c => c.Response).Returns(httpResponse.Object);
        var filterContext = new AuthorizationFilterContext(new ActionContext(httpContextBase.Object, new RouteData(), new ActionDescriptor()), new List<IFilterMetadata>());
        var claim = new Claim(ProviderClaims.ProviderUkprn, ukprn.ToString());
        var claimsPrinciple = new ClaimsPrincipal(new[] { new ClaimsIdentity(new[] { claim }) });
        var context = new AuthorizationHandlerContext(new[] { providerRequirement }, claimsPrinciple, filterContext);

        var response = new ApiResponse<GetProviderAccountResponse>(new GetProviderAccountResponse { CanAccessService = false }, System.Net.HttpStatusCode.Found, string.Empty);// Body = new GetProviderAccountResponse { CanAccessService = false } };
        apiClient.Setup(x =>
            x.Get<GetProviderAccountResponse>(
                It.Is<GetProviderAccountRequest>(c => c.GetUrl.Contains(ukprn.ToString())))).ReturnsAsync(response);

        //Act
        await authorizationHandler.HandleAsync(context);

        //Assert
        context.HasSucceeded.Should().BeTrue();
        httpResponse.Verify(x => x.Redirect(It.Is<string>(c => c.Contains("/error/403/invalid-status"))));
    }
    [Test, MoqAutoData]
    public async Task Then_Succeeds_If_Provider_Ukprn_Claim_Response_Is_True(
        int ukprn,
        TrainingProviderAllRolesRequirement providerRequirement,
        [Frozen] Mock<IApiClient> apiClient,
        TrainingProviderAllRolesAuthorizationHandler authorizationHandler)
    {
        //Arrange
        var claim = new Claim(ProviderClaims.ProviderUkprn, ukprn.ToString());
        var claimsPrinciple = new ClaimsPrincipal(new[] { new ClaimsIdentity(new[] { claim }) });
        var context = new AuthorizationHandlerContext(new[] { providerRequirement }, claimsPrinciple, null);
        var response = new ApiResponse<GetProviderAccountResponse>(new GetProviderAccountResponse { CanAccessService = true }, System.Net.HttpStatusCode.Found, string.Empty);// Body = new GetProviderAccountResponse { CanAccessService = false } };

        apiClient.Setup(x =>
            x.Get<GetProviderAccountResponse>(
                It.Is<GetProviderAccountRequest>(c => c.GetUrl.Contains(ukprn.ToString())))).ReturnsAsync(response);

        //Act
        await authorizationHandler.HandleAsync(context);

        //Assert
        Assert.IsTrue(context.HasSucceeded);
        Assert.IsFalse(context.HasFailed);
    }
}