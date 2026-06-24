using MediatR;
using RealEstate.Application.Features.Properties.Villas.DTOs;
using RealEstate.Domain.Interfaces.Properties;
using RealEstate.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Villas.Queries.GetVillasPaged
{
    public class GetVillasPagedQueryHandler
        : IRequestHandler<GetVillasPagedQuery, ApiResponse<PagedResult<VillaDto>>>
    {
        private readonly IVillaRepository _villaRepository;

        public GetVillasPagedQueryHandler(IVillaRepository villaRepository)
        {
            _villaRepository = villaRepository;
        }

        public async Task<ApiResponse<PagedResult<VillaDto>>> Handle(GetVillasPagedQuery request, CancellationToken ct)
        {
            var paged = await _villaRepository.GetPagedAsync(
                request.CompanyId,
                request.Page,
                request.PageSize,
                ct);

            var result = new PagedResult<VillaDto>
            {
                Page = paged.Page,
                PageSize = paged.PageSize,
                TotalCount = paged.TotalCount,
                Items = paged.Items.Select(entity => new VillaDto
                {
                    Id = entity.Id,
                    PropertyCode = entity.PropertyCode,
                    ParentPropertyId = entity.ParentPropertyId,
                    Title = entity.Title,
                    Description = entity.Description,
                    Purpose = entity.Purpose,
                    PropertyStatus = entity.PropertyStatus,
                    Price = entity.Price,
                    Area = entity.Area,
                    City = entity.City,
                    District = entity.District,
                    Address = entity.Address,
                    Latitude = entity.Latitude,
                    Longitude = entity.Longitude,
                    ParkingSpots = entity.ParkingSpots,
                    AgeInYears = entity.AgeInYears,
                    FacingDirection = entity.FacingDirection,
                    FurnishedStatus = entity.FurnishedStatus,
                    RegaLicenseNumber = entity.RegaLicenseNumber,
                    DeedNumber = entity.DeedNumber,
                    MunicipalityNumber = entity.MunicipalityNumber,
                    IsFeatured = entity.IsFeatured,
                    CompanyId = entity.CompanyId,
                    OwnerId = entity.OwnerId,
                    AgentId = entity.AgentId,

                    Bedrooms = entity.Bedrooms,
                    Bathrooms = entity.Bathrooms,
                    Floors = entity.Floors,
                    MaidRoom = entity.MaidRoom,
                    DriverRoom = entity.DriverRoom,
                    Pool = entity.Pool,
                    GardenArea = entity.GardenArea
                }).ToList()
            };

            return ApiResponse<PagedResult<VillaDto>>.Ok(result);
        }
    }
}
