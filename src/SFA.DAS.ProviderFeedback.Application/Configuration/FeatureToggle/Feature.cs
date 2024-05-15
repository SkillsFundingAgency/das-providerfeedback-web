using SFA.DAS.ProviderFeedback.Domain.Configuration;

namespace SFA.DAS.ProviderFeedback.Application.Configuration.FeatureToggle
{
    public class Feature : IFeature
    {
        private readonly Features _configuration;
        public Feature(Features configuration)
        {
            _configuration = configuration;
        }
        public bool IsFeatureEnabled(string feature)
        {

            if (feature.Equals("ShowEmployerReviews"))
            {
                return _configuration.ShowEmployerReviews;
            }
            return false;
        }
    }
}
