using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.DTOs.Properties.Warehouse;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Properties.Warehouses.Queries.GetWarehouseById
{
    public record GetWarehouseByIdQuery(Guid Id)
         : IRequest<ApiResponse<WarehouseDetailDto>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
