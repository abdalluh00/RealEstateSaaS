using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.Features.Owners.DTO;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Owners.Queries.GetOwners
{
    public class GetOwnersQuery
     : IRequest<ApiResponse<List<OwnerDto>>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }


}
