using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using SFA.DAS.ProviderFeedback.Web.Infrastructure.Authorization;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace SFA.DAS.ProviderFeedback.Web.AppStart
{
    public class ProviderStubAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProviderStubAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IHttpContextAccessor httpContextAccessor) : base(options, logger, encoder, clock)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new[]
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, "10000001"),
                new Claim(ProviderClaims.DisplayName, "AED User"),
                new Claim(ProviderClaims.Service, "DAA"),
                new Claim(ProviderClaims.ProviderUkprn, "10000001")
            };
            var identity = new ClaimsIdentity(claims, "Provider-stub");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Provider-stub");

            var result = AuthenticateResult.Success(ticket);

            _httpContextAccessor.HttpContext.Items.Add(ClaimsIdentity.DefaultNameClaimType, "10000001");
            _httpContextAccessor.HttpContext.Items.Add(ProviderClaims.DisplayName, "AED User");

            return Task.FromResult(result);
        }
    }
}