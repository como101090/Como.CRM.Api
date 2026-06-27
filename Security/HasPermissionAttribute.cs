using Microsoft.AspNetCore.Authorization;

namespace Como.CRM.Api.Security;

public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(string permission)
    {
        Policy = PermissionAuthorizationPolicyProvider.PolicyPrefix + permission;
    }
}
