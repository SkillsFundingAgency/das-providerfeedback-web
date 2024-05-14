using System.Text.Json.Serialization;

namespace SFA.DAS.ProviderFeedback.Domain.ProviderAccounts.Api.Responses
{
    public class GetProviderAccountResponse
    {
        [JsonPropertyName("canAccessService")]
        public bool CanAccessService { get; set; }
    }
}