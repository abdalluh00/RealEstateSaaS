using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Queries.GetDashboardStats
{
    public sealed class GetDashboardStatsQuery
        : IRequest<ApiResponse<PropertyDashboardDto>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }

        // How many featured to show in dashboard widget
        public int FeaturedLimit { get; init; } = 6;

        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
