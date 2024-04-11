using SFA.DAS.ProviderFeedback.Domain.Interfaces;

namespace SFA.DAS.ProviderFeedback.Domain.GetProviderFeedback
{
    public class GetProviderFeedbackRequest : IGetApiRequest
    {
        public int ProviderId { get; set; }
        public GetProviderFeedbackRequest(int providerId)
        {
            ProviderId = providerId;
        }
        public string GetUrl => $"provider/{ProviderId}";
    }
}