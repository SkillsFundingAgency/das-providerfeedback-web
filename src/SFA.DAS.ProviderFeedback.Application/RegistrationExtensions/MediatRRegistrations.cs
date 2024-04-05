using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.ProviderFeedback.Application.Commands;

namespace SFA.DAS.ProviderFeedback.Application.RegistrationExtensions
{
    public static class MediatRRegistrations
    {
        public static IServiceCollection AddMediatRHandlers(this IServiceCollection services)
        {
            services.AddMediatR(options => options.RegisterServicesFromAssemblyContaining<Dummy>());
            return services;
        }
    }
}
