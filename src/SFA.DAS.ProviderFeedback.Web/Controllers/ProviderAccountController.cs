using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
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

        [Route("signout", Name = RouteNames.ProviderSignOut)]
        public IActionResult SignOut()
        {
            return SignOut(
                new Microsoft.AspNetCore.Authentication.AuthenticationProperties
                {
                    RedirectUri = "",
                    AllowRefresh = true
                },
                OpenIdConnectDefaults.AuthenticationScheme);
        }

        //[AllowAnonymous]
        //[Route("signout", Name = RouteNames.ProviderSignOut)]
        //public async Task<IActionResult> Logout()
        //{
        //    var idToken = await HttpContext.GetTokenAsync("id_token");

        //    var authenticationProperties = new AuthenticationProperties();
        //    authenticationProperties.Parameters.Clear();
        //    authenticationProperties.Parameters.Add("id_token", idToken);
        //    var schemes = new List<string>
        //    {
        //        CookieAuthenticationDefaults.AuthenticationScheme,
        //        OpenIdConnectDefaults.AuthenticationScheme
        //    };

        //    return SignOut(authenticationProperties, schemes.ToArray());
        //}
    }
}