using Como.CRM.Api.Common.Constants;
using Como.CRM.Api.DTOs.Auth;
using FluentValidation;

namespace Como.CRM.Api.Validators.Auth
{
    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(x => x.CurrentPassword)
                    .Must(x => !string.IsNullOrWhiteSpace(x))
                    .WithErrorCode(ValidationCodes.Required);

            RuleFor(x => x.NewPassword)
                .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithErrorCode(ValidationCodes.Required);

            RuleFor(x => x.ConfirmNewPassword)
                .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithErrorCode(ValidationCodes.Required);
        }
    }
}
