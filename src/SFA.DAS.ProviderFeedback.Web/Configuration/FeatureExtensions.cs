using SFA.DAS.ProviderFeedback.Application.Configuration.FeatureToggle;

namespace SFA.DAS.ProviderFeedback.Web.Configuration
{
    public static class FeatureExtensions
    {
        public static void AddFeatureToggle(this IServiceCollection services)
        {
            services.AddSingleton<IFeature, Feature>();
        }
    }
}
