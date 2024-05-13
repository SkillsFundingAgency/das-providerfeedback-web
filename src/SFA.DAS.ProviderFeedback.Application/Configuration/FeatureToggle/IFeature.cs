namespace SFA.DAS.ProviderFeedback.Application.Configuration.FeatureToggle
{
    public interface IFeature
    {
        bool IsFeatureEnabled(string feature);
    }
}
