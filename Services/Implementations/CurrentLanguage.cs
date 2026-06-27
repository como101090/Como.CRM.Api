using Como.CRM.Api.Enums;
using Como.CRM.Api.Services.Abstractions;

namespace Como.CRM.Api.Services.Implementations
{
    public class CurrentLanguage : ICurrentLanguage
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentLanguage(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Language Language
        {
            get
            {
                var value = _httpContextAccessor.HttpContext?
                    .Request
                    .Headers["Accept-Language"]
                    .FirstOrDefault();

                if (string.IsNullOrWhiteSpace(value))
                    return Language.En;

                value = value.ToLowerInvariant();

                if (value.StartsWith("hy"))
                    return Language.Hy;

                if (value.StartsWith("ru"))
                    return Language.Ru;

                if(value.StartsWith("ka"))
                    return Language.Ka;

                return Language.En;
            }
        }
    }
}
