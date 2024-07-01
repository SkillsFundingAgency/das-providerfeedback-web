using MediatR;
using SFA.DAS.ProviderFeedback.Application.Commands.Queries.GetProviderFeedback;
using SFA.DAS.ProviderFeedback.Domain.Extensions;
using SFA.DAS.ProviderFeedback.Domain.GetProviderFeedback;
using SFA.DAS.ProviderFeedback.Domain.Interfaces;
using static SFA.DAS.ProviderFeedback.Domain.GetProviderFeedback.Feedback;

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

            var feedbackAttributesResult = ConstructEmployerFeedbackDetails(result.Body.ProviderFeedback.EmployerFeedback.FeedbackAttributes);
            result.Body.ProviderFeedback.EmployerFeedback.FeedbackAttributes = feedbackAttributesResult;

            return new GetProviderFeedbackResult
            {
                ApprenticeFeedback = result.Body.ProviderFeedback.ApprenticeFeedback,
                EmployerFeedback = result.Body.ProviderFeedback.EmployerFeedback,
                Ukprn = result.Body.ProviderId
            };
        }

        private List<EmployerFeedbackAttributeDetail> ConstructEmployerFeedbackDetails(List<EmployerFeedbackAttributeDetail> feedbackResult) 
        {
            var fullFeedbackAttributes = new List<EmployerFeedbackAttributeDetail>();

            foreach (string attr in EmployerFeedbackAttributes.AllFeedbackAttributes)
            {
                bool matchFound = false;

                foreach (EmployerFeedbackAttributeDetail f in feedbackResult)
                {
                    if (f.AttributeName == attr)
                    {
                        matchFound = true;
                        fullFeedbackAttributes.Add(f);
                        break;
                    }
                }

                if (!matchFound)
                {
                    var emptyFeedbackAttribute = new EmployerFeedbackAttributeDetail
                    {
                        AttributeName = attr,
                        Strength = 0,
                        Weakness = 0,
                        TotalVotes = 0,
                        Rating = 0
                    };

                    fullFeedbackAttributes.Add(emptyFeedbackAttribute);
                }
            }

            return fullFeedbackAttributes;
        }
    }
}