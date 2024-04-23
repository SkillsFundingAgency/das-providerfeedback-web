using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.WsFederation;
using SFA.DAS.ProviderFeedback.Web.Middleware;
using SFA.DAS.ProviderFeedback.Web.Configuration.Routing;
using SFA.DAS.ProviderFeedback.Domain.Entities;
using SFA.DAS.ProviderFeedback.Web.Extensions;

namespace SFA.DAS.ProviderFeedback.Web.Configuration
{
    public static class ConfigurationExtensions
    {
        private const int SessionTimeoutMinutes = 30;

        public static void AddAuthorizationService(this IServiceCollection services, bool useDfESignIn)
        {
            var ukPrnClaimName = useDfESignIn
                ? ProviderRecruitClaims.DfEUkprnClaimsTypeIdentifier
                : ProviderRecruitClaims.IdamsUserUkprnClaimsTypeIdentifier;

            var serviceClaimName = useDfESignIn
                ? ProviderRecruitClaims.DfEUserServiceTypeClaimTypeIdentifier
                : ProviderRecruitClaims.IdamsUserServiceTypeClaimTypeIdentifier;

            services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyNames.ProviderPolicyName, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ukPrnClaimName);
                    policy.RequireClaim(serviceClaimName);
                    policy.Requirements.Add(new ProviderAccountRequirement());
                    //policy.Requirements.Add(new TrainingProviderAllRolesRequirement());
                });
            });

            services.AddTransient<IAuthorizationHandler, ProviderAccountHandler>();
        }
           

        public static void AddAuthenticationService(this IServiceCollection services, AuthenticationConfiguration authConfig)
        {
            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = WsFederationDefaults.AuthenticationScheme;
                sharedOptions.DefaultSignOutScheme = WsFederationDefaults.AuthenticationScheme;
            })
            .AddWsFederation(options =>
            {
                options.Wtrealm = authConfig.WtRealm;
                options.MetadataAddress = authConfig.MetaDataAddress;
                options.UseTokenLifetime = false;
                
            })
            .AddCookie(options =>
            {
                options.Cookie.Name = CookieNames.RecruitData;
                options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
                options.CookieManager = new ChunkingCookieManager() { ChunkSize = 3000 };
                options.AccessDeniedPath = RoutePaths.AccessDeniedPath;
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(SessionTimeoutMinutes);
            });
            services
                .AddOptions<WsFederationOptions>(WsFederationDefaults.AuthenticationScheme);
                //.Configure<IRecruitVacancyClient>((options, recruitVacancyClient) =>
                //{
                //    options.Events.OnSecurityTokenValidated = async (ctx) =>
                //    {
                //        await HandleUserSignedIn(ctx, recruitVacancyClient);
                //    };
                //}
                //
                //);
        }

        //private static async Task HandleUserSignedIn(SecurityTokenValidatedContext ctx, IRecruitVacancyClient vacancyClient)
        //{
        //    var user = ctx.Principal.ToVacancyUser();
        //    await vacancyClient.UserSignedInAsync(user, UserType.Provider);
        //}
    }
}