using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.Features.Properties.Buildings.Dtos;
using RealEstate.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Buildings.Queries.GetBuildingById
{
    public record GetBuildingByIdQuery(Guid Id) : IRequest<ApiResponse<BuildingDetailsDto>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
