using Microsoft.Extensions.Options;
using SFA.DAS.ProviderFeedback.Domain.Configuration;

namespace SFA.DAS.ProviderFeedback.Web.AppStart;

public static class AddConfigurationOptionsExtension
{
    public static void AddConfigurationOptions(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<ProviderFeedbackOuterApi>(configuration.GetSection(nameof(ProviderFeedbackOuterApi)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<ProviderFeedbackOuterApi>>().Value);

        services.Configure<Features>(configuration.GetSection(nameof(Features)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<Features>>().Value);
    }
}