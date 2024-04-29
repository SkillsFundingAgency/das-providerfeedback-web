using static SFA.DAS.ProviderFeedback.Domain.GetProviderFeedback.Feedback;

namespace SFA.DAS.ProviderFeedback.Domain.GetProviderFeedback
{
    public class GetProviderFeedbackResponse
    {
        public int ProviderId { get; set; }
        public ProviderFeedbackModel ProviderFeedback { get; set; }
    }

    

}

