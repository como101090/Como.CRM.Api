using Como.CRM.Api.Common.Constants;
using Como.CRM.Api.Common.Responses;
using Como.CRM.Api.Common.Validation;
using Como.CRM.Api.Services.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Como.CRM.Api.Common.Filters;

public class ValidationFilter : IAsyncActionFilter
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IErrorMessageService _errorMessageService;

    public ValidationFilter(
        IServiceProvider serviceProvider,
        IErrorMessageService errorMessageService)
    {
        _serviceProvider = serviceProvider;
        _errorMessageService = errorMessageService;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var apiErrors = new List<ApiError>();

        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument == null)
                continue;

            var argumentType = argument.GetType();

            var validatorType = typeof(IValidator<>).MakeGenericType(argumentType);

            var validator = _serviceProvider.GetService(validatorType) as IValidator;

            if (validator == null)
                continue;

            var validationContext = new ValidationContext<object>(argument);

            var validationResult = await validator.ValidateAsync(validationContext);

            if (validationResult.IsValid)
                continue;

            apiErrors.AddRange(validationResult.Errors.Select(error =>
            {
                var code = string.IsNullOrWhiteSpace(error.ErrorCode)
                    ? ValidationCodes.InvalidValue
                    : error.ErrorCode;

                var rule = BuildRuleFromValidator(
                    validator,
                    error.PropertyName);

                return new ApiError
                {
                    Field = ToCamelCase(error.PropertyName),
                    Code = code,
                    Message = _errorMessageService.GetMessage(
                        argumentType,
                        error.PropertyName,
                        code,
                        rule),
                    Rule = rule
                };
            }));
        }

        if (apiErrors.Count > 0)
        {
            context.Result = new BadRequestObjectResult(
                ResponseFactory.Error<object>(
                    ErrorCodes.ValidationFailed,
                    StatusCodes.Status400BadRequest,
                    apiErrors));

            return;
        }

        await next();
    }

    private static ValidationRule? BuildRuleFromValidator(
        IValidator validator,
        string propertyName)
    {
        var descriptor = validator.CreateDescriptor();

        var validators = descriptor
            .GetValidatorsForMember(propertyName)
            .ToList();

        if (validators.Count == 0)
            return null;

        var rule = new ValidationRule();

        foreach (var item in validators)
        {
            var validatorName = item.Validator.GetType().Name;

            if (validatorName.Contains("NotEmpty") ||
                validatorName.Contains("NotNull"))
            {
                rule.Required = true;
            }

            //if (validatorName.Contains("Email"))
            //{
            //    rule.Format = "email";
            //}

            if (validatorName.Contains("MaximumLength"))
            {
                rule.MaxLength = GetIntProperty(item.Validator, "Max");
            }

            if (validatorName.Contains("MinimumLength"))
            {
                rule.MinLength = GetIntProperty(item.Validator, "Min");
            }

            //if (validatorName.Contains("RegularExpression"))
            //{
            //    rule.Format ??= "regex";
            //}
        }

        return rule;
    }

    private static int? GetIntProperty(object source, string propertyName)
    {
        var property = source.GetType().GetProperty(propertyName);

        var value = property?.GetValue(source);

        return value == null ? null : Convert.ToInt32(value);
    }

    private static string ToCamelCase(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return value;

        return char.ToLowerInvariant(value[0]) + value[1..];
    }
}