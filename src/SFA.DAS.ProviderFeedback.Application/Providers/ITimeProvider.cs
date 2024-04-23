using System;

namespace SFA.DAS.ProviderFeedback.Application.Providers
{
    public interface ITimeProvider
    {
        DateTime Now { get; }
        DateTime OneHour { get; }
        DateTime Today { get; }
        DateTime NextDay { get; }
        DateTime NextDay6am { get; }
    }
}
