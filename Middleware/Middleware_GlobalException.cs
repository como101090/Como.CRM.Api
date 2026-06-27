
using Como.CRM.Api.Common.Constants;
using Como.CRM.Api.Common.Exceptions;
using Como.CRM.Api.Common.Responses;
using Como.CRM.Api.Enums;
using Como.CRM.Api.Services.Abstractions;
using Como.CRM.Api.Services.Implementations;
using System.Text.Json;

namespace Como.CRM.Api.Middleware;

public sealed class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IBusinessMessageProvider _businessErrorMessageService;


    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger,
        IBusinessMessageProvider businessErrorMessageService)
    {
        _next = next;
        _logger = logger;
      
        _businessErrorMessageService = businessErrorMessageService;
    }

    public async Task InvokeAsync(HttpContext context, ICurrentLanguage language)
    {
        try
        {
            await _next(context);
        }
        catch (BusinessException ex)
        {
            _logger.LogWarning(ex, ex.ErrorCode);

            var errors = ex.Errors;

            if (errors == null || errors.Count == 0)
            {
                errors = new List<ApiError>
                {
                    new ApiError
                    {
                        Field = string.Empty,
                        Code = ex.ErrorCode,
                        Message = _businessErrorMessageService.GetMessage(ex.ErrorCode, language.Language)
                    }
                };
            }
            else
            {
                foreach (var error in errors)
                {
                    if (string.IsNullOrWhiteSpace(error.Message))
                    {
                        error.Message = _businessErrorMessageService.GetMessage(error.Code, language.Language);
                    }
                }
            }

            await WriteResponseAsync(
                context,
                ex.StatusCode,
                ex.ErrorCode,
                errors);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, ex.Message);

            await WriteResponseAsync(
                context,
                StatusCodes.Status401Unauthorized,
                ErrorCodes.Unauthorized);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, ex.Message);

            await WriteResponseAsync(
                context,
                StatusCodes.Status404NotFound,
                ErrorCodes.NotFound);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, ex.Message);

            await WriteResponseAsync(
                context,
                StatusCodes.Status400BadRequest,
                ErrorCodes.BadRequest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            await WriteResponseAsync(
                context,
                StatusCodes.Status500InternalServerError,
                ErrorCodes.InternalServerError);
        }
    }

    private static async Task WriteResponseAsync(
        HttpContext context,
        int statusCode,
        string errorCode,
        List<ApiError>? errors = null)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = ResponseFactory.Error<object>(
            errorCode,
            statusCode,
            errors);

        var json = JsonSerializer.Serialize(
            response,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

        await context.Response.WriteAsync(json);
    }
}