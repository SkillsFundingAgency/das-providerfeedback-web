using static SFA.DAS.ProviderFeedback.Domain.GetProviderFeedbackAnnual.FeedbackAnnual;

namespace SFA.DAS.ProviderFeedback.Domain.GetProviderFeedbackAnnual
{
    public class GetProviderFeedbackAnnualResponse
    {
        public int ProviderId { get; set; }
        public ProviderFeedbackAnnualModel ProviderFeedback { get; set; }
    }

    

}

