using Como.CRM.Api.Common.Constants;
using Como.CRM.Api.DTOs.Tenants;
using FluentValidation;

namespace Como.CRM.Api.Validators.Tenants
{
    public class RegisterTenantRequestValidator : AbstractValidator<RegisterTenantRequest>
    {
        public RegisterTenantRequestValidator()
        {
            RuleFor(x => x.BrandName)
                .NotEmpty()
                .WithErrorCode(ValidationCodes.Required)
                .MinimumLength(2)
                .WithErrorCode(ValidationCodes.MinLength)
                .MaximumLength(25)
                .WithErrorCode(ValidationCodes.MaxLength)                 
                .Matches("^[a-zA-Z0-9]+$")
                .WithErrorCode(ValidationCodes.InvalidFormat);

            RuleFor(x => x.LegalName)
                .NotEmpty()
                
                .WithErrorCode(ValidationCodes.Required)
                .MinimumLength(2)
                .WithErrorCode(ValidationCodes.MinLength)
                .MaximumLength(200)
                .WithErrorCode(ValidationCodes.MaxLength)
                .Matches(@"^[Ա-Ֆա-ֆև0-9\s-]+$")
                .WithErrorCode(ValidationCodes.InvalidFormat);


            RuleFor(x => x.CompanyEmail)
                .NotEmpty()
                .WithErrorCode(ValidationCodes.Required)
                
                .MinimumLength(6)
                .WithErrorCode(ValidationCodes.MinLength)
                .MaximumLength(50)
                 .WithErrorCode(ValidationCodes.MaxLength)
                .EmailAddress()
                .WithErrorCode(ValidationCodes.InvalidFormat);

            RuleFor(x => x.ContactPhone)
                .NotEmpty()
                .WithErrorCode(ValidationCodes.Required)
                .Matches(@"^\+?[0-9\s\-()]{6,30}$")
                .WithErrorCode(ValidationCodes.InvalidFormat)
                .MinimumLength(8)
                .WithErrorCode(ValidationCodes.MinLength)
                .MaximumLength(15)
                .WithErrorCode(ValidationCodes.MaxLength);
                 

        }
    }
}
