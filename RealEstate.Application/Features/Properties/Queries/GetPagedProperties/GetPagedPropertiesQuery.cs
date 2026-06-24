using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Queries.GetPagedProperties
{
    public sealed class GetPagedPropertiesQuery
       : IRequest<ApiResponse<PagedResult<PropertyListDto>>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }

        // ── Filters ───────────────────────────────────────
        public PropertyStatus? Status { get; init; }
        public PropertyPurpose? Purpose { get; init; }
        public string? City { get; init; }

        // ── Pagination ────────────────────────────────────
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 10;

        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}

public record PropertyListDto
{
    public Guid Id { get; init; }
    public string PropertyCode { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Purpose { get; init; } = string.Empty;       // localized string
    public string PropertyStatus { get; init; } = string.Empty; // localized string
    public decimal Price { get; init; }
    public decimal Area { get; init; }
    public string City { get; init; } = string.Empty;
    public string District { get; init; } = string.Empty;
    public bool IsFeatured { get; init; }
    public bool IsPublished { get; init; }
    public DateTime CreatedAt { get; init; }
}
