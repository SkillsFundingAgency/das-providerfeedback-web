using System;
using SFA.DAS.ProviderFeedback.Domain.Entities;

namespace SFA.DAS.ProviderFeedback.Domain.Configuration
{
    public class ServiceParameters
    {
        public ServiceParameters(string serviceType)
        {
            VacancyType = Enum.TryParse<VacancyType>(serviceType, true, out var type)
                ? type
                : Domain.Entities.VacancyType.Apprenticeship;
        }

        public VacancyType? VacancyType { get; set; }
    }

}