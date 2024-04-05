using System.Text.Json.Serialization;

namespace SFA.DAS.ProviderFeedback.Domain.Interfaces;

public interface IGetApiRequest
{
    [JsonIgnore]
    string GetUrl { get;}
}