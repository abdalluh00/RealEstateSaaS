using MediatR;
using Microsoft.AspNetCore.Http;
using RealEstate.Shared.Common.Exceptions;
using System.Reflection;

namespace RealEstate.Application.Common.Behaviors
{
    // Attribute لتحديد الـ Roles المسموحة لكل Command/Query
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AuthorizeAttribute : Attribute
    {
        public string Roles { get; set; } = string.Empty;
        public string Policy { get; set; } = string.Empty;
    }

    public class AuthorizationBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IHttpContextAccessor _httpContext;

        public AuthorizationBehavior(IHttpContextAccessor httpContext) =>
            _httpContext = httpContext;

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken ct)
        {
            // جيب الـ Authorize Attributes من الـ Request
            var authorizeAttributes = request.GetType()
                .GetCustomAttributes<AuthorizeAttribute>()
                .ToList();

            // إذا ما في Attributes — اكمل بدون تحقق
            if (!authorizeAttributes.Any())
                return await next();

            var user = _httpContext.HttpContext?.User;

            // تحقق من تسجيل الدخول
            if (user?.Identity?.IsAuthenticated != true)
                throw new UnauthorizedException("يجب تسجيل الدخول أولاً");

            // تحقق من الـ Roles
            foreach (var attribute in authorizeAttributes
                .Where(a => !string.IsNullOrEmpty(a.Roles)))
            {
                var roles = attribute.Roles.Split(',');
                var hasRole = roles.Any(role =>
                    user.IsInRole(role.Trim()));

                if (!hasRole)
                    throw new ForbiddenException("ليس لديك صلاحية لهذا الإجراء");
            }

            return await next();
        }
    }
}
