using Microsoft.AspNetCore.Authentication;

namespace SFA.DAS.ProviderFeedback.Web.AppStart
{
    public static class ProviderStubAuthentication
    {
        public static void AddProviderStubAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication("Provider-stub").AddScheme<AuthenticationSchemeOptions, ProviderStubAuthHandler>(
                "Provider-stub",
                options => { });
        }
    }
}