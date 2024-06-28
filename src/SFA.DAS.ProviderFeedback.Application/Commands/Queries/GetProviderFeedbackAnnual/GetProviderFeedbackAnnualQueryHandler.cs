using MediatR;
using SFA.DAS.ProviderFeedback.Domain.Extensions;
using SFA.DAS.ProviderFeedback.Domain.GetProviderFeedbackAnnual;
using SFA.DAS.ProviderFeedback.Domain.Interfaces;

namespace SFA.DAS.ProviderFeedback.Application.Queries.GetProviderFeedbackAnnual
{
    public class GetProviderFeedbackAnnualQueryHandler : IRequestHandler<GetProviderFeedbackAnnualQuery, GetProviderFeedbackAnnualResult>
    {
        private readonly IApiClient _apiClient;

        public GetProviderFeedbackAnnualQueryHandler(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetProviderFeedbackAnnualResult> Handle(GetProviderFeedbackAnnualQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.Get<GetProviderFeedbackAnnualResponse>(new GetProviderFeedbackAnnualRequest(request.ProviderId));

            result.EnsureSuccessStatusCode();

            return new GetProviderFeedbackAnnualResult
            {
                ApprenticeFeedback = result.Body.ProviderFeedback.ApprenticeFeedback,
                EmployerFeedback = result.Body.ProviderFeedback.EmployerFeedback,
                Ukprn = result.Body.ProviderId
            };
        }
    }
}