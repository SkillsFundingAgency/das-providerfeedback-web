using MediatR;
using SFA.DAS.ProviderFeedback.Application.Commands.Queries.GetProviderFeedback;
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


            foreach (var annualFeedbackDetail in result.Body.ProviderFeedback.EmployerFeedback.AnnualEmployerFeedbackDetails)
            {
                var feedbackAttributesResult = ConstructEmployerFeedbackDetails(annualFeedbackDetail.FeedbackAttributes);
                annualFeedbackDetail.FeedbackAttributes = feedbackAttributesResult;
            }

            return new GetProviderFeedbackAnnualResult
            {
                ApprenticeFeedback = result.Body.ProviderFeedback.ApprenticeFeedback,
                EmployerFeedback = result.Body.ProviderFeedback.EmployerFeedback,
                Ukprn = result.Body.ProviderId
            };
        }
        private List<EmployerFeedbackAnnualAttributeDetail> ConstructEmployerFeedbackDetails(List<EmployerFeedbackAnnualAttributeDetail> feedbackResult)
        {
            var fullFeedbackAttributes = new List<EmployerFeedbackAnnualAttributeDetail>();

            foreach (string attr in EmployerFeedbackAttributes.AllFeedbackAttributes)
            {
                bool matchFound = false;

                foreach (EmployerFeedbackAnnualAttributeDetail f in feedbackResult)
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
                    var emptyFeedbackAttribute = new EmployerFeedbackAnnualAttributeDetail
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