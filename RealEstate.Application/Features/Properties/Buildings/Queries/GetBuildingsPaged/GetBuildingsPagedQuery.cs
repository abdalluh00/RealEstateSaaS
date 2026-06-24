using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.Features.Properties.Buildings.Dtos;
using RealEstate.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Buildings.Queries.GetBuildingsPaged
{
    public record GetBuildingsPagedQuery(int Page = 1, int PageSize = 10)
       : IRequest<ApiResponse<PagedResult<BuildingListItemDto>>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
