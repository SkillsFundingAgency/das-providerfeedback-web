
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.ProviderFeedback.Application.Queries.GetProviderFeedback;
using SFA.DAS.ProviderFeedback.Domain.Messaging;

namespace SFA.DAS.ProviderFeedback.Application.RegistrationExtensions
{
    public static class MediatRRegistrations
    {
        public static IServiceCollection AddMediatRHandlers(this IServiceCollection services)
        {
            services.AddMediatR(options => options.RegisterServicesFromAssemblyContaining<GetProviderFeedbackQuery>());
            services.AddTransient<IMessaging, MediatrMessaging>();


            return services;
        }
    }
}
