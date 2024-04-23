using SFA.DAS.DfESignIn.Auth.Enums;
using SFA.DAS.DfESignIn.Auth.Interfaces;

namespace SFA.DAS.ProviderFeedback.Web.Configuration
{
    public class CustomServiceRole : ICustomServiceRole
    {
        public string RoleClaimType => ProviderRecruitClaims.DfEUserServiceTypeClaimTypeIdentifier;

        // <inherit-doc/>
        public CustomServiceRoleValueType RoleValueType => CustomServiceRoleValueType.Code;
    }
}