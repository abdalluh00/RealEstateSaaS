using MediatR;
using Microsoft.AspNetCore.Authorization;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.DTOs.Properties.Villa;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Properties.Villas.Queries.GetVillasPaged
{
    [Authorize(Roles = "Owner,Admin,Agent")]
    public sealed record GetVillasPagedQuery
       : IRequest<ApiResponse<PagedResult<VillaListDto>>>, IAutoTenantRequest
    {
        public int? MinBedrooms { get; init; }
        public int? MaxBedrooms { get; init; }
        public  PropertyStatus? status { get; init; }
        public PropertyPurpose? purpose { get; init; }
        public FurnishedStatus? FurnishedStatus { get; init; }
        public bool? HasPool { get; init; }
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
