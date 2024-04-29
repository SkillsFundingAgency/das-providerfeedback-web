using System.Security.Claims;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderFeedback.Web.Infrastructure.Authorization;
using SFA.DAS.Testing.AutoFixture;
using FluentAssertions;

namespace SFA.DAS.ProviderFeedback.Web.UnitTests.Infrastructure.Authorization
{
    public class WhenHandlingProviderAuthorizationRequirement
    {
        [Test, MoqAutoData]
        public async Task Then_Fails_If_No_Provider_Ukprn_Claim(
            int ukprn,
            ProviderUkPrnRequirement providerRequirement,
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            ProviderAuthorizationHandler authorizationHandler)
        {
            //Arrange
            var claim = new Claim("NotProviderClaim", ukprn.ToString());
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});
            var context = new AuthorizationHandlerContext(new [] {providerRequirement}, claimsPrinciple, null);

            //Act
            await authorizationHandler.HandleAsync(context);

            //Assert
            Assert.IsFalse(context.HasSucceeded);
            Assert.IsTrue(context.HasFailed);
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_No_Ukrpn_In_Route_But_Has_Claim_Then_Succeeds(
            int ukprn,
            ProviderUkPrnRequirement providerRequirement,
            [Frozen]Mock<IHttpContextAccessor> httpContextAccessor,
            ProviderAuthorizationHandler authorizationHandler)
        {
            //Arrange
            var responseMock = new FeatureCollection();
            var httpContext = new DefaultHttpContext(responseMock);
            httpContextAccessor.Setup(_ => _.HttpContext).Returns(httpContext);
            var claim = new Claim(ProviderClaims.ProviderUkprn, ukprn.ToString());
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});
            var context = new AuthorizationHandlerContext(new []{providerRequirement},claimsPrinciple, null);
            
            //Act
            await authorizationHandler.HandleAsync(context);

            //Assert
            Assert.IsTrue(context.HasSucceeded);
            Assert.IsFalse(context.HasFailed);
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Route_Ukprn_Does_Not_Match_Value_In_Claim_Then_Fails(
            int ukprn,
            int routeUkprn,
            ProviderUkPrnRequirement providerRequirement,
            [Frozen]Mock<IHttpContextAccessor> httpContextAccessor,
            ProviderAuthorizationHandler authorizationHandler)
        {
            //Arrange
            var responseMock = new FeatureCollection();
            var httpContext = new DefaultHttpContext(responseMock);
            httpContext.Request.RouteValues.Add("ukprn",routeUkprn);
            httpContextAccessor.Setup(_ => _.HttpContext).Returns(httpContext);
            var claim = new Claim(ProviderClaims.ProviderUkprn, ukprn.ToString());
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});
            var context = new AuthorizationHandlerContext(new []{providerRequirement},claimsPrinciple, null);
            
            //Act
            await authorizationHandler.HandleAsync(context);

            //Assert
            Assert.IsFalse(context.HasSucceeded);
            Assert.IsTrue(context.HasFailed);
        }
        
        
        [Test, MoqAutoData]
        public async Task Then_If_The_Route_Ukprn_Does_Match_Value_In_Claim_Then_Succeeds(
            int ukprn,
            ProviderUkPrnRequirement providerRequirement,
            [Frozen]Mock<IHttpContextAccessor> httpContextAccessor,
            ProviderAuthorizationHandler authorizationHandler)
        {
            //Arrange
            var responseMock = new FeatureCollection();
            var httpContext = new DefaultHttpContext(responseMock);
            httpContext.Request.RouteValues.Add("ukprn",ukprn);
            httpContextAccessor.Setup(_ => _.HttpContext).Returns(httpContext);
            var claim = new Claim(ProviderClaims.ProviderUkprn, ukprn.ToString());
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});
            var context = new AuthorizationHandlerContext(new []{providerRequirement},claimsPrinciple, null);
            
            //Act
            await authorizationHandler.HandleAsync(context);

            //Assert
            Assert.IsTrue(context.HasSucceeded);
            Assert.IsFalse(context.HasFailed);
        }
    }
}