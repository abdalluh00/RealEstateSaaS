using Microsoft.AspNetCore.Http;
using RealEstate.Application.Common.Interfaces;
using System.Security.Claims;

namespace RealEstate.Infrastructure.Services
{
    public class TenantService : ITenantService
    {
        private readonly IHttpContextAccessor _http;

        public TenantService(IHttpContextAccessor http) => _http = http;

        private ClaimsPrincipal? User => _http.HttpContext?.User;

        public Guid CompanyId =>
            Guid.TryParse(User?.FindFirst("CompanyId")?.Value, out var id)
                ? id : Guid.Empty;

        public Guid UserId =>
            Guid.TryParse(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id)
                ? id : Guid.Empty;

        public string Role =>
            User?.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

        public bool IsOwner => Role == "Owner";
        public bool IsAdmin => Role == "Admin";
        public bool IsAgent => Role == "Agent";
    }
}
