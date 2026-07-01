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

builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection(JwtOptions.SectionName));

builder.Services.Configure<SmtpOptions>(
    builder.Configuration.GetSection(SmtpOptions.SectionName));

builder.Services.Configure<AppOptions>(
    builder.Configuration.GetSection("App"));

var jwtConfig = builder.Configuration
    .GetSection(JwtOptions.SectionName)
    .Get<JwtOptions>()
    ?? throw new InvalidOperationException("Jwt configuration is missing.");

var appOptions = builder.Configuration
    .GetSection("App")
    .Get<AppOptions>()
    ?? throw new InvalidOperationException("App configuration is missing.");

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<ICurrentTenantService, CurrentTenantService>();
builder.Services.AddScoped<ICurrentLanguage, CurrentLanguage>();

builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

builder.Services.AddScoped<IErrorMessageService, ErrorMessageService>();
builder.Services.AddSingleton<IBusinessMessageProvider, TenantBusinessMessages>();

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

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtConfig.Issuer,

            ValidateAudience = true,
            ValidAudience = jwtConfig.Audience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtConfig.SecretKey)),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
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

    options.AddServer(new OpenApiServer
    {
        Url = "{baseUrl}",
        Description = "Postman base URL",
        Variables = new Dictionary<string, OpenApiServerVariable>
        {
            ["baseUrl"] = new OpenApiServerVariable
            {
                Default = $"https://{appOptions.BaseDomain}",
                Description = "API base URL"
            }
        }
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

app.UseMiddleware<TenantResolutionMiddleware>();

app.UseAuthentication();

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