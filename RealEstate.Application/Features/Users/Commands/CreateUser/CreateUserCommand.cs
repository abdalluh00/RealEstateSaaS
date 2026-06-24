using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Users.Commands.CreateUser
{
    public record CreateUserCommand(
     string FullName,
     string Email,
     string Phone,
     string Password,
     UserRole Role
 ) : IRequest<ApiResponse<Guid>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    };
}
