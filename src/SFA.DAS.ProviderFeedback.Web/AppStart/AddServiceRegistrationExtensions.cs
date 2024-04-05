using SFA.DAS.ProviderFeedback.Domain.Interfaces;
using SFA.DAS.ProviderFeedback.Infrastructure.Api;

namespace SFA.DAS.ProviderFeedback.Web.AppStart;

public static class AddServiceRegistrationExtension
{
    public static void AddServiceRegistration(this IServiceCollection services)
    {
        services.AddHttpClient<IApiClient, ApiClient>();
    }
}