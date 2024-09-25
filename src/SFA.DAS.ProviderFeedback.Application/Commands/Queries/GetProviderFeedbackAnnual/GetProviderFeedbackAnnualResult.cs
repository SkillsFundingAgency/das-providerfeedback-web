using static SFA.DAS.ProviderFeedback.Domain.GetProviderFeedbackAnnual.FeedbackAnnual;

namespace SFA.DAS.ProviderFeedback.Application.Queries.GetProviderFeedbackAnnual
{
    public class GetProviderFeedbackAnnualResult
    {
        public int Ukprn { get; set; }
        public bool IsEmployerProvider { get; set; }
        public EmployerFeedbackAnnual EmployerFeedback { get; set; }
        public ApprenticeFeedbackAnnual ApprenticeFeedback { get; set; }
    }

    
}