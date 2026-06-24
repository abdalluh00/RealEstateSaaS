using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Domain.ReadModels;
using RealEstate.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Warehouses.Queries.GetWarehousesPaged
{
    public record GetWarehousesPagedQuery(
        int Page = 1,
        int PageSize = 20,
        string? Search = null)
        : IRequest<ApiResponse<PagedResult<WarehouseListItemDto>>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
