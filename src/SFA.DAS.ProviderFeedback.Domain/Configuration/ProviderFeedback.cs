namespace SFA.DAS.ProviderFeedback.Domain.Configuration;

public class ProviderFeedback
{
    public string? DataProtectionKeysDatabase { get; set; }
    public string? RedisConnectionString { get; set; }
    public bool UseDfESignIn { get; set; }
}