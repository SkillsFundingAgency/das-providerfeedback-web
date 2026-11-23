using Azure.Monitor.OpenTelemetry.AspNetCore;

namespace SFA.DAS.ProviderFeedback.Web.AppStart
{
    public static class AddOpenTelemetryExtensions
    {
        public static void AddOpenTelemetryRegistration(this IServiceCollection services, string? appInsightsConnectionString)
        {
            if (!string.IsNullOrWhiteSpace(appInsightsConnectionString))
            {
                services.AddOpenTelemetry().UseAzureMonitor(options =>
                {
                    options.ConnectionString = appInsightsConnectionString;
                });
            }
        }
    }
}