using Como.CRM.Api.Common.Constants;
using Como.CRM.Api.DTOs.Auth;
using Como.CRM.Api.DTOs.Tenants;
using FluentValidation;

namespace Como.CRM.Api.Validators.Auth
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.UserName)
     .Must(x => !string.IsNullOrWhiteSpace(x))
     .WithErrorCode(ValidationCodes.Required);

            RuleFor(x => x.Password)
                .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithErrorCode(ValidationCodes.Required);

        }
    }

    
}
