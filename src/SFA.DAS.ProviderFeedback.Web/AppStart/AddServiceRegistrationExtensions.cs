using das_providerfeedback_web.Configuration;
using Microsoft.AspNetCore.Authentication;
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
        //services.AddTransient<IAutoConfigurationService, TableStorageConfigurationService>();
        //services.AddSingleton<ILinkGenerator, ProviderUrlHelper.LinkGenerator>();

        //services.AddTransient<IModelMapper, ModelMapper>();

        //services.AddSingleton<IAcademicYearDateProvider, AcademicYearDateProvider>();
        //services.AddTransient<IPolicyAuthorizationWrapper, PolicyAuthorizationWrapper>();
        //services.AddTransient<IAuthorizationContextProvider, AuthorizationContextProvider>();

        services.AddTransient<IAzureTableStorageConnectionAdapter, AzureTableStorageConnectionAdapter>();
        services.AddTransient<IEnvironmentService, EnvironmentService>();
        services.AddTransient<IAutoConfigurationService, TableStorageConfigurationService>();
        services.AddSingleton<ILinkGenerator, ProviderUrlHelper.LinkGenerator>();



    }
}