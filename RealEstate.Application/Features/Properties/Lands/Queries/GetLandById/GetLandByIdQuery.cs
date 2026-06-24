using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.Features.Properties.Lands.Dtos;
using RealEstate.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Lands.Queries.GetLandById
{
    public record GetLandByIdQuery(Guid Id) : IRequest<ApiResponse<LandDetailsDto>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
