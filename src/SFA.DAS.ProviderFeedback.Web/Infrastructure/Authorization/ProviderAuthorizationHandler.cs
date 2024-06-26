using Microsoft.AspNetCore.Authorization;

namespace SFA.DAS.ProviderFeedback.Web.Infrastructure.Authorization
{
    public class ProviderAuthorizationHandler : AuthorizationHandler<ProviderUkPrnRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProviderAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ProviderUkPrnRequirement requirement)
        {
            if (!IsProviderAuthorised(context))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            context.Succeed(requirement);

            return Task.CompletedTask;
        }

        private bool IsProviderAuthorised(AuthorizationHandlerContext context)
        {
            if (!context.User.HasClaim(c => c.Type.Equals(ProviderClaims.ProviderUkprn)))
            {
                return false;
            }

            if (_httpContextAccessor.HttpContext.Request.RouteValues.ContainsKey("ukprn"))
            {
                var ukPrnFromUrl = _httpContextAccessor.HttpContext.Request.RouteValues["ukprn"].ToString();
                var ukPrn = context.User.FindFirst(c => c.Type.Equals(ProviderClaims.ProviderUkprn)).Value;

                return ukPrn.Equals(ukPrnFromUrl);
            }

            return true;
        }
    }
}