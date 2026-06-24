using MediatR;
using RealEstate.Application.Common.Extensions;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Queries.GetFeaturedProperties
{
    public sealed class GetFeaturedPropertiesQueryHandler
        : IRequestHandler<GetFeaturedPropertiesQuery, ApiResponse<PagedResult<PropertyListDto>>>
    {
        private readonly IPropertyRepository _propertyRepo;

        public GetFeaturedPropertiesQueryHandler(IPropertyRepository propertyRepo)
            => _propertyRepo = propertyRepo;

        public async Task<ApiResponse<PagedResult<PropertyListDto>>> Handle(
            GetFeaturedPropertiesQuery query,
            CancellationToken ct)
        {
            // Repo already filters IsFeatured = true && IsPublished = true
            var result = await _propertyRepo.GetFeaturedAsync(
                companyId: query.CompanyId,
                limit: query.PageSize,
                ct: ct);

            var dtos = result
                .Select(x => new PropertyListDto
                {
                    Id = x.Id,
                    PropertyCode = x.PropertyCode,
                    Title = x.Title,
                    Purpose = x.Purpose.ToArabicString(),
                    PropertyStatus = x.PropertyStatus.ToArabicString(),
                    Price = x.Price,
                    Area = x.Area,
                    City = x.City,
                    District = x.District,
                    IsFeatured = x.IsFeatured,
                    IsPublished = x.IsPublished,
                    CreatedAt = x.CreatedAt
                })
                .ToList();

            var paged = PagedResult<PropertyListDto>.Create(
                items: dtos,
                totalCount: dtos.Count,
                page: query.Page,
                pageSize: query.PageSize);

            return ApiResponse<PagedResult<PropertyListDto>>.Ok(paged);
        }
    }
}
