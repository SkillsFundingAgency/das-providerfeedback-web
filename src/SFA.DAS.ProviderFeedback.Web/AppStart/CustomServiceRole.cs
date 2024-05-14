using SFA.DAS.DfESignIn.Auth.Enums;
using SFA.DAS.DfESignIn.Auth.Interfaces;
using SFA.DAS.ProviderFeedback.Web.Infrastructure.Authorization;

namespace SFA.DAS.ProviderFeedback.Web.AppStart;

public class CustomServiceRole : ICustomServiceRole
{
    public string RoleClaimType => ProviderClaims.Service;

    // <inherit-doc/>
    public CustomServiceRoleValueType RoleValueType => CustomServiceRoleValueType.Code;
}