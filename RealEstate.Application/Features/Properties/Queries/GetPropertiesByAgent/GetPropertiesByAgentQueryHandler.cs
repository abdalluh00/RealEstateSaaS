using MediatR;
using RealEstate.Application.Common.Extensions;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Queries.GetPropertiesByAgent
{
    public sealed class GetPropertiesByAgentQueryHandler
         : IRequestHandler<GetPropertiesByAgentQuery, ApiResponse<PagedResult<PropertyListDto>>>
    {
        private readonly IPropertyRepository _propertyRepo;

        public GetPropertiesByAgentQueryHandler(IPropertyRepository propertyRepo)
            => _propertyRepo = propertyRepo;

        public async Task<ApiResponse<PagedResult<PropertyListDto>>> Handle(
            GetPropertiesByAgentQuery query,
            CancellationToken ct)
        {
            var result = await _propertyRepo.GetByAgentAsync(
                agentId: query.AgentId,
                companyId: query.CompanyId,
                page: query.Page,
                pageSize: query.PageSize,
                ct: ct);

            var dto = PagedResult<PropertyListDto>.Create(
                items: result.Items
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
                    .ToList(),
                totalCount: result.TotalCount,
                page: result.Page,
                pageSize: result.PageSize);

            return ApiResponse<PagedResult<PropertyListDto>>.Ok(dto);
        }
    }
}
