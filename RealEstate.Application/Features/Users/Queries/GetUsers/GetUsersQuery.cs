using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.Features.Users.DTO;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.Users.Queries.GetUsers
{
    public class GetUsersQuery
    : IRequest<ApiResponse<List<UserDto>>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
