using TaskFlow.Application.Abstractions;

namespace TaskFlow.Infrastructure.Security
{
    public class JwtOptions : IJwtOptions
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        public int ExpirationInMinutes { get; set; }
    }
}
