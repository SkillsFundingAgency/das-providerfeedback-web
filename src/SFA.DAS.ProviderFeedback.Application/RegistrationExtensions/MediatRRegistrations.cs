using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.ProviderFeedback.Domain.GetProviderFeedback;

namespace SFA.DAS.ProviderFeedback.Application.RegistrationExtensions
{
    public static class MediatRRegistrations
    {
        public static IServiceCollection AddMediatRHandlers(this IServiceCollection services)
        {
            services.AddMediatR(options => options.RegisterServicesFromAssemblyContaining<Feedback>());
            return services;
        }
    }
}
