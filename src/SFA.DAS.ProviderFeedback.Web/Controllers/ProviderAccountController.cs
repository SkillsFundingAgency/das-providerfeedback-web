using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SFA.DAS.ProviderFeedback.Web.Infrastructure;

namespace SFA.DAS.ProviderFeedback.Web.Controllers
{
    public class ProviderAccountController : ControllerBase
    {
        private readonly Domain.Configuration.ProviderFeedback _config;

        public ProviderAccountController(IOptions<Domain.Configuration.ProviderFeedback> config)
        {
            _config = config.Value;
        }

        [Route("signout",Name = RouteNames.ProviderSignOut)]
        public IActionResult SignOut()
        {
            var authScheme = _config.UseDfESignIn
                ? OpenIdConnectDefaults.AuthenticationScheme
                : WsFederationDefaults.AuthenticationScheme;

            return SignOut(
                new Microsoft.AspNetCore.Authentication.AuthenticationProperties
                {
                    RedirectUri = "",
                    AllowRefresh = true
                },
                CookieAuthenticationDefaults.AuthenticationScheme,
                authScheme);
        }
    }
}