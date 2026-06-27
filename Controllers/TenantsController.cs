using Como.CRM.Api.Common.Business.Tenant;
using Como.CRM.Api.Common.EmailTemplates;
using Como.CRM.Api.Common.Exceptions;
using Como.CRM.Api.Common.Responses;
using Como.CRM.Api.Domain.Entities;
using Como.CRM.Api.DTOs.Tenants;
using Como.CRM.Api.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Como.CRM.Api.Controllers;

[ApiController]
[Route("api/tenants")]
public class TenantsController : ControllerBase
{
    private readonly ITenantService _tenantService;


    public TenantsController(ITenantService tenantService, IEmailService emailService)
    {
        _tenantService = tenantService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<RegisterTenantResponse>>> Register(RegisterTenantRequest request, CancellationToken cancellationToken)
    {
        var result = await _tenantService.RegisterAsync(request, cancellationToken);

        return StatusCode(
                    StatusCodes.Status201Created,
                    ResponseFactory.Created(result));

    }
}
