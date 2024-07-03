using SFA.DAS.ProviderFeedback.Domain.Interfaces;

namespace SFA.DAS.ProviderFeedback.Domain.GetProviderFeedbackAnnual
{
    public class GetProviderFeedbackAnnualRequest : IGetApiRequest
    {
        public int ProviderId { get; set; }
        public GetProviderFeedbackAnnualRequest(int providerId)
        {
            ProviderId = providerId;
        }
        public string GetUrl => $"providerFeedback/{ProviderId}/annual";
    }
}