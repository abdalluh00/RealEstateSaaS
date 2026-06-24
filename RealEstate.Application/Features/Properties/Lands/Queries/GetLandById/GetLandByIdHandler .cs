using MediatR;
using RealEstate.Application.Features.Properties.Lands.Dtos;
using RealEstate.Domain.Interfaces.Properties;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Lands.Queries.GetLandById
{
    public class GetLandByIdHandler : IRequestHandler<GetLandByIdQuery, ApiResponse<LandDetailsDto>>
    {
        private readonly ILandRepository _landRepository;

        public GetLandByIdHandler(ILandRepository landRepository)
        {
            _landRepository = landRepository;
        }

        public async Task<ApiResponse<LandDetailsDto>> Handle(GetLandByIdQuery request, CancellationToken ct)
        {
            var entity = await _landRepository.GetByIdWithDetailsAsync(request.Id, request.CompanyId);
            if (entity is null)
                throw new NotFoundException("الأرض غير موجودة");

            var dto = new LandDetailsDto
            {
                Id = entity.Id,
                PropertyCode = entity.PropertyCode,
                ParentPropertyId = entity.ParentPropertyId,

                Title = entity.Title,
                Description = entity.Description,

                Purpose = entity.Purpose.ToString(),
                PropertyStatus = entity.PropertyStatus.ToString(),

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
                FurnishedStatus = entity.FurnishedStatus.ToString(),

                RegaLicenseNumber = entity.RegaLicenseNumber,
                DeedNumber = entity.DeedNumber,
                MunicipalityNumber = entity.MunicipalityNumber,

                IsFeatured = entity.IsFeatured,

                OwnerId = entity.OwnerId ?? Guid.Empty,
                AgentId = entity.AgentId ?? Guid.Empty,

                StreetWidth = entity.StreetWidth,
                ZoningType = entity.ZoningType,
                CornerLand = entity.CornerLand,
                NumberOfStreets = entity.NumberOfStreets
            };

            return ApiResponse<LandDetailsDto>.Ok(dto);
        }
    }
}
