using MediatR;
using SFA.DAS.ProviderFeedback.Domain.Extensions;
using SFA.DAS.ProviderFeedback.Domain.GetProviderFeedback;
using SFA.DAS.ProviderFeedback.Domain.Interfaces;

namespace SFA.DAS.ProviderFeedback.Application.Queries.GetProviderFeedback
{
    public class GetProviderFeedbackQueryHandler : IRequestHandler<GetProviderFeedbackQuery, GetProviderFeedbackResult>
    {
        private readonly IApiClient _apiClient;

        public GetProviderFeedbackQueryHandler(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetProviderFeedbackResult> Handle(GetProviderFeedbackQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.Get<GetProviderFeedbackResponse>(new GetProviderFeedbackRequest(request.ProviderId));

            result.EnsureSuccessStatusCode();

            return new GetProviderFeedbackResult
            {
                ApprenticeFeedback = result.Body.ProviderFeedback.ApprenticeFeedback,
                EmployerFeedback = result.Body.ProviderFeedback.EmployerFeedback,
                Ukprn = result.Body.ProviderId
            };
        }
    }
}