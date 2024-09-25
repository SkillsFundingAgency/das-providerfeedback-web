using MediatR;
using SFA.DAS.ProviderFeedback.Application.Commands.Queries.GetProviderFeedbackAnnual;
using SFA.DAS.ProviderFeedback.Domain.Extensions;
using SFA.DAS.ProviderFeedback.Domain.GetProviderFeedbackAnnual;
using SFA.DAS.ProviderFeedback.Domain.Interfaces;
using static SFA.DAS.ProviderFeedback.Domain.GetProviderFeedbackAnnual.FeedbackAnnual;

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

            result.Body.ProviderFeedback.EmployerFeedback.AnnualEmployerFeedbackDetails
                .ForEach(detail => detail.FeedbackAttributes = ConstructEmployerFeedbackDetails(detail.FeedbackAttributes));

            return new GetProviderFeedbackAnnualResult
            {
                ApprenticeFeedback = result.Body.ProviderFeedback.ApprenticeFeedback,
                EmployerFeedback = result.Body.ProviderFeedback.EmployerFeedback,
                Ukprn = result.Body.ProviderId,
                IsEmployerProvider= result.Body.ProviderFeedback.IsEmployerProvider
            };
        }
        private List<EmployerFeedbackAnnualAttributeDetail> ConstructEmployerFeedbackDetails(List<EmployerFeedbackAnnualAttributeDetail> feedbackResult)
        {
            var allAttributes = EmployerFeedbackAttributes.AllFeedbackAttributes;

            var fullFeedbackAttributes = allAttributes.Select(attr =>
            {
                var existingAttribute = feedbackResult.FirstOrDefault(f => f.AttributeName == attr);

                return existingAttribute ?? new EmployerFeedbackAnnualAttributeDetail
                {
                    AttributeName = attr,
                    Strength = 0,
                    Weakness = 0,
                    TotalVotes = 0,
                    Rating = 0
                };
            }).ToList();

            return fullFeedbackAttributes;
        }
    }
}