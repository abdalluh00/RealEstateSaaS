using MediatR;
using RealEstate.Application.Features.Apartments.DTOs;
using RealEstate.Domain.Interfaces.Properties.RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Apartments.Queries.GetApartmentById
{
    public class GetApartmentByIdHandler : IRequestHandler<GetApartmentByIdQuery, ApiResponse<ApartmentDetailsDto>>
    {
        private readonly IApartmentRepository _apartmentRepository;

        public GetApartmentByIdHandler(IApartmentRepository apartmentRepository)
        {
            _apartmentRepository = apartmentRepository;
        }

        public async Task<ApiResponse<ApartmentDetailsDto>> Handle(GetApartmentByIdQuery request, CancellationToken ct)
        {
            var apartment = await _apartmentRepository.GetByIdAsync(request.ApartmentId, request.CompanyId);
            if (apartment is null)
                throw new NotFoundException("الشقة غير موجودة");

            var dto = new ApartmentDetailsDto
            {
                Id = apartment.Id,
                PropertyCode = apartment.PropertyCode,
                Title = apartment.Title,
                Description = apartment.Description,
                Purpose = apartment.Purpose,
                PropertyStatus = apartment.PropertyStatus,
                Price = apartment.Price,
                Area = apartment.Area,
                City = apartment.City,
                District = apartment.District,
                Address = apartment.Address,
                Latitude = apartment.Latitude,
                Longitude = apartment.Longitude,
                ParkingSpots = apartment.ParkingSpots,
                AgeInYears = apartment.AgeInYears,
                FacingDirection = apartment.FacingDirection,
                FurnishedStatus = apartment.FurnishedStatus,
                RegaLicenseNumber = apartment.RegaLicenseNumber,
                DeedNumber = apartment.DeedNumber,
                MunicipalityNumber = apartment.MunicipalityNumber,
                IsFeatured = apartment.IsFeatured,
                ParentPropertyId = apartment.ParentPropertyId,
                OwnerId = apartment.OwnerId ?? Guid.Empty,
                AgentId = apartment.AgentId ?? Guid.Empty,
                UnitNumber = apartment.UnitNumber,
                Bedrooms = apartment.Bedrooms,
                Bathrooms = apartment.Bathrooms,
                FloorNumber = apartment.FloorNumber,
                LivingRooms = apartment.LivingRooms,
                HasMaidRoom = apartment.HasMaidRoom,
                HasElevator = apartment.HasElevator,
                HasCentralAc = apartment.HasCentralAc,
                HasBalcony = apartment.HasBalcony,
                CreatedAt = apartment.CreatedAt,
                UpdatedAt = apartment.UpdatedAt
            };

            return ApiResponse<ApartmentDetailsDto>.Ok(dto);
        }
    }
}
