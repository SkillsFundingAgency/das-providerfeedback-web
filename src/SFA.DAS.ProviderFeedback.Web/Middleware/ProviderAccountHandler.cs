using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SFA.DAS.ProviderFeedback.Extensions;
using SFA.DAS.ProviderFeedback.Web.Configuration;
using SFA.DAS.ProviderFeedback.Web.Configuration.Routing;

namespace SFA.DAS.ProviderFeedback.Web.Middleware
{
    public class ProviderAccountHandler : AuthorizationHandler<ProviderAccountRequirement>
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        //private readonly IProviderVacancyClient _client;
        //private readonly IBlockedOrganisationQuery _blockedOrganisationsRepo;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly Predicate<Claim> _ukprnClaimFinderPredicate = c => c.Type.Equals(ProviderRecruitClaims.IdamsUserUkprnClaimsTypeIdentifier) 
                                                                            || c.Type.Equals(ProviderRecruitClaims.DfEUkprnClaimsTypeIdentifier);
        private readonly IDictionary<string, object> _dict = new Dictionary<string, object>();
       //private readonly ITrainingProviderSummaryProvider _trainingProviderSummaryProvider;

        public ProviderAccountHandler(IWebHostEnvironment hostingEnvironment, ITempDataProvider tempDataProvider)
        {
            _hostingEnvironment = hostingEnvironment;
            //_client = client;
            //_blockedOrganisationsRepo = blockedOrganisationsRepo;
            _tempDataProvider = tempDataProvider;
            //_trainingProviderSummaryProvider = trainingProviderSummaryProvider;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ProviderAccountRequirement requirement)
        {
            var hasIdentityServerAuthorization = HasServiceAuthorization(context) && HasUkprnAuthorization(context);

            if (context.User.HasClaim(_ukprnClaimFinderPredicate))
            {
                var ukprnFromClaim = context.User.FindFirst(_ukprnClaimFinderPredicate).Value;

                var isOnRoatp = await HasRoatpAuthorizationAsync(context, ukprnFromClaim);

                if (hasIdentityServerAuthorization && isOnRoatp)
                {
                    if (HasDoneOncePerAuthorizedSessionActions(context) == false)
                    {
                        var isProviderBlocked = await HasBeenBlockedOnRecruit(ukprnFromClaim);

                        if (isProviderBlocked == false)
                        {
                            SetOncePerAuthorizedSessionActionsCompleted(context);
                        }
                        else
                        {
                            context.Fail();
                        }
                    }

                    if (context.HasFailed)
                    {
                        var mvcContext = (AuthorizationFilterContext)context.Resource;
                        _tempDataProvider.SaveTempData(mvcContext.HttpContext, _dict);
                    }
                    else
                    {
                        context.Succeed(requirement);
                    }
                }
            }
        }

        private async Task<bool> HasBeenBlockedOnRecruit(string ukprnFromClaim)
        {
            //var bo = await _blockedOrganisationsRepo.GetByOrganisationIdAsync(ukprnFromClaim);
            //var isBlocked = (bo == null || bo.BlockedStatus == BlockedStatus.Unblocked) == false;

            //_dict.Add(TempDataKeys.IsBlockedProvider, isBlocked);

            //return isBlocked;
            return false;
        }

        private bool HasServiceAuthorization(AuthorizationHandlerContext context)
        {
            Predicate<Claim> serviceClaimFinderPredicate = c => c.Type.Equals(ProviderRecruitClaims.IdamsUserServiceTypeClaimTypeIdentifier) 
                                                                || c.Type.Equals(ProviderRecruitClaims.DfEUserServiceTypeClaimTypeIdentifier);

            if (context.User.HasClaim(serviceClaimFinderPredicate))
            {
                var serviceClaims = context.User.FindAll(serviceClaimFinderPredicate);

                return serviceClaims.Any(claim => claim.Value.IsServiceClaim());
            }

            return false;
        }

        private bool HasUkprnAuthorization(AuthorizationHandlerContext context)
        {
            if (context.Resource is AuthorizationFilterContext mvcContext && mvcContext.RouteData.Values.ContainsKey(RouteValues.Ukprn))
            {
                if (context.User.HasClaim(_ukprnClaimFinderPredicate))
                {
                    var ukprnFromClaim = context.User.FindFirst(_ukprnClaimFinderPredicate).Value;
                    var ukprnFromUrl = mvcContext.RouteData.Values[RouteValues.Ukprn].ToString();

                    if (!string.IsNullOrEmpty(ukprnFromUrl) && ukprnFromUrl.Equals(ukprnFromClaim))
                    {
                        mvcContext.HttpContext.Items.TryAdd(ContextItemKeys.ProviderIdentifier, ukprnFromClaim);
                        _dict.Add(TempDataKeys.ProviderIdentifier, ukprnFromClaim);

                        return true;
                    }
                }
            }
            else
            {
                return true;
            }

            return false;
        }

        private async Task<bool> HasRoatpAuthorizationAsync(AuthorizationHandlerContext context, string ukprnFromClaim)
        {
            //if (HasDoneOncePerAuthorizedSessionActions(context))
            //    return true;

            //try
            //{
            //    if (long.TryParse(ukprnFromClaim, out var ukprn) == false)
            //        return false;

            //    if (ukprn == EsfaTestTrainingProvider.Ukprn)
            //        return true;

            //    var provider = await _trainingProviderSummaryProvider.GetAsync(ukprn);

            //    _dict.Add(TempDataKeys.ProviderName, provider.ProviderName);

            //    return provider != null;
            //}
            //catch (Exception)
            //{
            //    return false;
            //}
            return true;
        }

        //private async Task SetupProvider(AuthorizationHandlerContext context)
        //{
        //    if (context.Resource is AuthorizationFilterContext mvcContext &&
        //        mvcContext.RouteData.Values.ContainsKey(RouteValues.Ukprn))
        //    {
        //        var ukprn = context.User.FindFirst(_ukprnClaimFinderPredicate).Value;

        //        await _client.SetupProviderAsync(long.Parse(ukprn));
        //    }
        //}

        private bool HasDoneOncePerAuthorizedSessionActions(AuthorizationHandlerContext context)
        {
            if (!(context.Resource is AuthorizationFilterContext mvcContext))
                return false;

            var ukprn = context.User.FindFirst(_ukprnClaimFinderPredicate).Value;

            var cookieKey = GetOncePerAuthorizedSessionCookieKey(ukprn);
            var cookie = mvcContext.HttpContext.Request.Cookies[cookieKey];

            return cookie != null;
        }

        private void SetOncePerAuthorizedSessionActionsCompleted(AuthorizationHandlerContext context)
        {
            if (!(context.Resource is AuthorizationFilterContext mvcContext))
                return;

            var ukprn = context.User.FindFirst(_ukprnClaimFinderPredicate).Value;
            var cookieKey = GetOncePerAuthorizedSessionCookieKey(ukprn);

            mvcContext.HttpContext.Response.Cookies.Append(cookieKey, "1", EsfaCookieOptions.GetDefaultHttpCookieOption(_hostingEnvironment));
        }

        private string GetOncePerAuthorizedSessionCookieKey(string ukprn)
        {
            return string.Format(CookieNames.SetupProvider, ukprn);
        }
    }
}