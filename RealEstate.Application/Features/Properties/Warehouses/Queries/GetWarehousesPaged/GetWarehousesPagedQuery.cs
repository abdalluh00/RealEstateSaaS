using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.DTOs.Properties.Warehouse;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.Properties.Warehouses.Queries.GetWarehousesPaged
{
    public sealed record GetWarehousesPagedQuery(
       
        string? Search = null)
        : IRequest<ApiResponse<PagedResult<WarehouseListDto>>>, IAutoTenantRequest
    {
        public PropertyStatus? status { get; init; }
        public PropertyPurpose? purpose { get; init; }
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public Guid CompanyId { get; private set; }
        public bool? HasColdStorage { get; init; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
