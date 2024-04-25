using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.WsFederation;
using SFA.DAS.ProviderFeedback.Domain.Configuration;
using SFA.DAS.ProviderFeedback.Web.Infrastructure.Authorization;

namespace SFA.DAS.ProviderFeedback.Web.AppStart
{
    public static class AuthenticationProviderExtensions
    {
        public static void AddAndConfigureProviderAuthentication(this IServiceCollection services, ProviderIdams idams)
        {
            var cookieOptions = new Action<CookieAuthenticationOptions>(options =>
            {
                options.CookieManager = new ChunkingCookieManager {ChunkSize = 3000};
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.AccessDeniedPath = "/error/403";
            });

            services.AddAuthentication(sharedOptions =>
                {
                    sharedOptions.DefaultScheme =
                        CookieAuthenticationDefaults.AuthenticationScheme;
                    sharedOptions.DefaultSignInScheme =
                        CookieAuthenticationDefaults.AuthenticationScheme;
                    sharedOptions.DefaultChallengeScheme =
                        WsFederationDefaults.AuthenticationScheme;
                })
                .AddWsFederation(options =>
                {
                    options.MetadataAddress = idams.MetadataAddress;
                    options.Wtrealm = idams.Wtrealm;
                    options.CallbackPath = "/{ukprn}/employer-demand";
                    options.Events.OnSecurityTokenValidated = async (ctx) =>
                    {
                        await PopulateProviderClaims(ctx.HttpContext, ctx.Principal);
                    };
                }).AddCookie(cookieOptions);

        }

        private static Task PopulateProviderClaims(HttpContext httpContext, ClaimsPrincipal principal)
        {
            var providerId = principal.Claims.First(c => c.Type.Equals(ProviderClaims.ProviderUkprn)).Value;
            var displayName = principal.Claims.First(c => c.Type.Equals(ProviderClaims.DisplayName)).Value;
            httpContext.Items.Add(ClaimsIdentity.DefaultNameClaimType,providerId);
            httpContext.Items.Add(ProviderClaims.DisplayName,displayName);
            principal.Identities.First().AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, providerId));
            principal.Identities.First().AddClaim(new Claim(ProviderClaims.DisplayName, displayName));
            return Task.CompletedTask;
        }
    }
}