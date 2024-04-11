using static SFA.DAS.ProviderFeedback.Domain.GetProviderFeedback.Feedback;

namespace SFA.DAS.ProviderFeedback.Application.Queries.GetProviderFeedback
{
    public class GetProviderFeedbackResult
    {
        public int Ukprn { get; set; }
        public EmployerFeedback EmployerFeedback { get; set; }
        public ApprenticeFeedback ApprenticeFeedback { get; set; }
    }

    
}