using Microsoft.AspNetCore.Authorization;

namespace RealEstate.API.Authorization
{
    public class SameCompanyHandler : AuthorizationHandler<SameCompanyRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            SameCompanyRequirement requirement)
        {
            // جيب الـ CompanyId من الـ Token
            var tokenCompanyId = context.User
                .FindFirst("CompanyId")?.Value;

            // جيب الـ CompanyId من الـ Route
            var httpContext = context.Resource as HttpContext;
            var routeCompanyId = httpContext?.Request.RouteValues["companyId"]?.ToString();

            // إذا ما في companyId في الـ Route — وافق (الـ Handler الثاني يتحقق)
            if (string.IsNullOrEmpty(routeCompanyId))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            // تحقق إن المستخدم من نفس الشركة
            if (tokenCompanyId == routeCompanyId)
                context.Succeed(requirement);
            else
                context.Fail();

            return Task.CompletedTask;
        }
    }
}
