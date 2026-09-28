using MediatR;
using RealEstate.Application.Common.Extensions;
using RealEstate.Application.DTOs.Properties.Base;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Queries.GetPagedProperties
{
    public sealed class GetPagedPropertiesQueryHandler
        : IRequestHandler<GetPagedPropertiesQuery, ApiResponse<PagedResult<PropertyListDto>>>
    {
        private readonly IPropertyRepository _propertyRepo;

        public GetPagedPropertiesQueryHandler(IPropertyRepository propertyRepo)
            => _propertyRepo = propertyRepo;

        public async Task<ApiResponse<PagedResult<PropertyListDto>>> Handle(
            GetPagedPropertiesQuery query,
            CancellationToken ct)
        {
            var result = await _propertyRepo.GetPagedAsync(
                companyId: query.CompanyId,
                page: query.Page,
                pageSize: query.PageSize,
                status: query.Status,
                purpose: query.Purpose,
                city: query.City,
                ct: ct);

            // Map ReadModel → Dto (lightweight, no DB call)
            var dto = PagedResult<PropertyListDto>.Create(
                items: result.Items.Select(x => new PropertyListDto
                {
                    Id = x.Id,
                    PropertyCode = x.PropertyCode,
                    Title = x.Title,
                    Purpose = x.Purpose,       // enum → localized
                    Status = x.Status,
                    Price = x.Price,
                    Area = x.Area,
                    City = x.City,
                    District = x.District,
                    IsFeatured = x.IsFeatured,
                    //IsPublished = x.IsPublished,
                    CreatedAt = x.CreatedAt
                }).ToList(),
                totalCount: result.TotalCount,
                page: result.Page,
                pageSize: result.PageSize);

            return ApiResponse<PagedResult<PropertyListDto>>.Ok(dto);
        }
    }
}
