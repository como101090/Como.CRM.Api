using Como.CRM.Api.Common.Business.Tenant;
using Como.CRM.Api.Common.Filters;
using Como.CRM.Api.Data;
using Como.CRM.Api.Middleware;
using Como.CRM.Api.Options;
using Como.CRM.Api.Security;
using Como.CRM.Api.Services.Abstractions;
using Como.CRM.Api.Services.Implementations;
using Como.CRM.Api.Validators.Tenants;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));
builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection(SmtpOptions.SectionName));

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<ICurrentTenantService, CurrentTenantService>();


builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

builder.Services.AddScoped<IErrorMessageService, ErrorMessageService>();


builder.Services.AddSingleton<IBusinessMessageProvider, TenantBusinessMessages>();
builder.Services.AddScoped<ICurrentLanguage, CurrentLanguage>();

builder.Services.AddScoped(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));

builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddValidatorsFromAssemblyContaining<RegisterTenantRequestValidator>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Como CRM API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT Bearer token"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("React", policy => policy
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .SetIsOriginAllowed(_ => true));
});

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("React");

// Tenant-ը որոշվում է միայն Host/Subdomain-ից։
// Production: https://crystaldent.comocrm.am
// Development: https://crystaldent.localhost:7080
app.UseMiddleware<TenantResolutionMiddleware>();

app.UseAuthentication();

// Token-ի tenant_id claim-ը պետք է համընկնի request-ի Host-ից գտնված Tenant-ի հետ։
app.UseMiddleware<TenantTokenValidationMiddleware>();

app.UseAuthorization();

app.MapGet("/", () => "Como CRM API Running");
app.MapGet("/health", () => Results.Ok(new
{
    status = "OK",
    app = "Como CRM API",
    time = DateTime.UtcNow
}));

app.MapControllers();

app.Run();
