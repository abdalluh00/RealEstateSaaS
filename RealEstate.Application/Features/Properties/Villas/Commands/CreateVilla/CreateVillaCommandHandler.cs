using MediatR;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Domain.Interfaces;
using RealEstate.Domain.Interfaces.Properties;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Villas.Commands.CreateVilla
{
    public class CreateVillaCommandHandler
       : IRequestHandler<CreateVillaCommand, ApiResponse<Guid>>
    {
        private readonly IVillaRepository _villaRepository;
        private readonly IPropertyLookupRepository _propertyLookupRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateVillaCommandHandler(
            IVillaRepository villaRepository,
            IPropertyLookupRepository propertyLookupRepository,
            IUnitOfWork unitOfWork)
        {
            _villaRepository = villaRepository;
            _propertyLookupRepository = propertyLookupRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<Guid>> Handle(CreateVillaCommand request, CancellationToken ct)
        {
            // Owner required + same company
            var ownerExists = await _propertyLookupRepository.IsOwnedByCompanyAsync(
                request.OwnerId, request.CompanyId, ct);

            if (!ownerExists)
                throw new NotFoundException("المالك غير موجود ضمن نفس الشركة");

            // Agent required + same company
            var agentExists = await _propertyLookupRepository.ExistsInCompanyAsync(
                request.AgentId, request.CompanyId, ct);

            if (!agentExists)
                throw new NotFoundException("الوكيل غير موجود ضمن نفس الشركة");

            // Optional parent property
            if (request.ParentPropertyId.HasValue)
            {
                var parentExists = await _propertyLookupRepository.ExistsInCompanyAsync(
                    request.ParentPropertyId.Value, request.CompanyId, ct);

                if (!parentExists)
                    throw new NotFoundException("العقار الأب غير موجود ضمن نفس الشركة");
            }

            var entity = new VillaProperty
            {
                Title = request.Title,
                Description = request.Description,
                Purpose = request.Purpose,
                PropertyStatus = request.PropertyStatus,
                Price = request.Price,
                Area = request.Area,
                City = request.City,
                District = request.District,
                Address = request.Address,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                ParkingSpots = request.ParkingSpots,
                AgeInYears = request.AgeInYears,
                FacingDirection = request.FacingDirection,
                FurnishedStatus = request.FurnishedStatus,
                RegaLicenseNumber = request.RegaLicenseNumber,
                DeedNumber = request.DeedNumber,
                MunicipalityNumber = request.MunicipalityNumber,
                IsFeatured = request.IsFeatured,
                ParentPropertyId = request.ParentPropertyId,
                CompanyId = request.CompanyId,
                OwnerId = request.OwnerId,
                AgentId = request.AgentId,

                Bedrooms = request.Bedrooms,
                Bathrooms = request.Bathrooms,
                Floors = request.Floors,
                MaidRoom = request.MaidRoom,
                DriverRoom = request.DriverRoom,
                Pool = request.Pool,
                GardenArea = request.GardenArea
            };

            await _villaRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync(ct);

            return ApiResponse<Guid>.Ok(entity.Id, "تم إنشاء الفيلا بنجاح");
        }
    }
}
