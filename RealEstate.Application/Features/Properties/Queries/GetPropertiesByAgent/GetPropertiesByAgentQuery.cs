using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.DTOs.Properties.Base;
using RealEstate.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Queries.GetPropertiesByAgent
{
    public sealed class GetPropertiesByAgentQuery
        : IRequest<ApiResponse<PagedResult<PropertyListDto>>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public Guid AgentId { get; init; }
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 10;

        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
