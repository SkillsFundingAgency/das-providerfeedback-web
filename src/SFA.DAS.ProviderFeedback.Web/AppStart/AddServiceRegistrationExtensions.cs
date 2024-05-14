using SFA.DAS.AutoConfiguration;
using SFA.DAS.ProviderFeedback.Domain.Interfaces;
using SFA.DAS.ProviderFeedback.Infrastructure.Api;
using SFA.DAS.ProviderUrlHelper;

namespace SFA.DAS.ProviderFeedback.Web.AppStart;

public static class AddServiceRegistrationExtension
{
    public static void AddServiceRegistration(this IServiceCollection services)
    {
        services.AddHttpClient<IApiClient, ApiClient>();
        services.AddTransient<IAzureTableStorageConnectionAdapter, AzureTableStorageConnectionAdapter>();
        services.AddTransient<IEnvironmentService, EnvironmentService>();
        services.AddTransient<IAutoConfigurationService, TableStorageConfigurationService>();
        services.AddSingleton<ILinkGenerator, ProviderUrlHelper.LinkGenerator>();
    }
}