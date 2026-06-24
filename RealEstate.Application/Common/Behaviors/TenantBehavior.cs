using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Common.Behaviors
{
    
    public class TenantBehavior<TRequest, TResponse>
     : IPipelineBehavior<TRequest, TResponse>
     where TRequest : IRequest<TResponse>
    {
        private readonly ITenantService _tenant;

        public TenantBehavior(ITenantService tenant) => _tenant = tenant;

        public async Task<TResponse> Handle(
     TRequest request,
     RequestHandlerDelegate<TResponse> next,
     CancellationToken ct)
        {
            if (request is IAuthRequest)
                return await next();

            if (_tenant.CompanyId == Guid.Empty)
                throw new UnauthorizedException("يجب تسجيل الدخول أولاً");

            // إذا الـ Request يدعم AutoTenant — ضع الـ CompanyId تلقائياً
            if (request is IAutoTenantRequest autoRequest)
                autoRequest.SetCompanyId(_tenant.CompanyId);

            // إذا الـ Request يحتوي CompanyId — تحقق منه
            if (request is ITenantRequest tenantRequest)
            if (tenantRequest.CompanyId != Guid.Empty &&
                    tenantRequest.CompanyId != _tenant.CompanyId)
                    throw new ForbiddenException("لا يمكنك الوصول لبيانات شركة أخرى");

            return await next();
        }
    }
}
