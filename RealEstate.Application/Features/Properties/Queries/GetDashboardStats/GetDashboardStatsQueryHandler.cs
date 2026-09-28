using MediatR;
using RealEstate.Application.Common.Extensions;
using RealEstate.Application.DTOs.Properties;
using RealEstate.Application.DTOs.Properties.Base;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Queries.GetDashboardStats
{
    public sealed class GetDashboardStatsQueryHandler
         : IRequestHandler<GetDashboardStatsQuery, ApiResponse<PropertyDashboardDto>>
    {
        private readonly IPropertyRepository _propertyRepo;

        public GetDashboardStatsQueryHandler(IPropertyRepository propertyRepo)
            => _propertyRepo = propertyRepo;

        public async Task<ApiResponse<PropertyDashboardDto>> Handle(
            GetDashboardStatsQuery query,
            CancellationToken ct)
        {
            // Two parallel DB calls — no dependency between them
            var statsTask = _propertyRepo.GetDashboardStatsAsync(query.CompanyId, query.FeaturedLimit, ct);
            var featuredTask = _propertyRepo.GetFeaturedAsync(query.CompanyId, query.FeaturedLimit, ct);

            await Task.WhenAll(statsTask, featuredTask);

            var stats = await statsTask;
            var featured = await featuredTask;

            var dto = new PropertyDashboardDto
            {
                TotalProperties = stats.TotalProperties,
                Available = stats.Available,
                Rented = stats.Rented,
                Sold = stats.Sold,
                Reserved = stats.Reserved,
                Featured = stats.Featured,
                Published = stats.Published,
                TopFeatured = featured
                    .Select(x => new PropertyListDto
                    {
                        Id = x.Id,
                        PropertyCode = x.PropertyCode,
                        Title = x.Title,
                        Purpose = x.Purpose,
                        Status = x.Status,
                        Price = x.Price,
                        Area = x.Area,
                        City = x.City,
                        District = x.District,
                        IsFeatured = x.IsFeatured,
                        //IsPublished = x.IsPublished,
                        CreatedAt = x.CreatedAt
                    })
                    .ToList()
            };

            return ApiResponse<PropertyDashboardDto>.Ok(dto);
        }
    }
}
