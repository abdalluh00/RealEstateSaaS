using Microsoft.AspNetCore.Http;
using RealEstate.Application.Common.Interfaces;
using System.Security.Claims;
namespace RealEstate.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContext;

        public CurrentUserService(IHttpContextAccessor httpContext) =>
            _httpContext = httpContext;

        private ClaimsPrincipal? User =>
            _httpContext.HttpContext?.User;

        public Guid UserId =>
            Guid.TryParse(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id)
                ? id : Guid.Empty;

        public Guid CompanyId =>
            Guid.TryParse(User?.FindFirst("CompanyId")?.Value, out var id)
                ? id : Guid.Empty;

        public string Role =>
            User?.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

        public string Email =>
            User?.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;

        public bool IsAuthenticated =>
            User?.Identity?.IsAuthenticated ?? false;
    }
}
