using MediatR;
using RealEstate.Application.Features.Properties.Villas.DTOs;
using RealEstate.Domain.Interfaces.Properties;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Villas.Queries.GetVillaById
{
    public class GetVillaByIdQueryHandler
       : IRequestHandler<GetVillaByIdQuery, ApiResponse<VillaDto>>
    {
        private readonly IVillaRepository _villaRepository;

        public GetVillaByIdQueryHandler(IVillaRepository villaRepository)
        {
            _villaRepository = villaRepository;
        }

        public async Task<ApiResponse<VillaDto>> Handle(GetVillaByIdQuery request, CancellationToken ct)
        {
            var entity = await _villaRepository.GetByIdAsync(request.Id, request.CompanyId, ct);
            if (entity is null)
                throw new NotFoundException("الفيلا غير موجودة");

            var dto = new VillaDto
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
            };

            return ApiResponse<VillaDto>.Ok(dto);
        }
    }
}
