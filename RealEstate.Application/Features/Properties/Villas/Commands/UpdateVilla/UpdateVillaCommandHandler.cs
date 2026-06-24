using MediatR;
using RealEstate.Domain.Interfaces;
using RealEstate.Domain.Interfaces.Properties;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Villas.Commands.UpdateVilla
{
    public class UpdateVillaCommandHandler
        : IRequestHandler<UpdateVillaCommand, ApiResponse<Guid>>
    {
        private readonly IVillaRepository _villaRepository;
        private readonly IPropertyLookupRepository _propertyLookupRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateVillaCommandHandler(
            IVillaRepository villaRepository,
            IPropertyLookupRepository propertyLookupRepository,
            IUnitOfWork unitOfWork)
        {
            _villaRepository = villaRepository;
            _propertyLookupRepository = propertyLookupRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<Guid>> Handle(UpdateVillaCommand request, CancellationToken ct)
        {
            var entity = await _villaRepository.GetByIdAsync(request.Id, request.CompanyId, ct);
            if (entity is null)
                throw new NotFoundException("الفيلا غير موجودة");

            var ownerExists = await _propertyLookupRepository.ExistsInCompanyAsync(
                request.OwnerId, request.CompanyId, ct);

            if (!ownerExists)
                throw new NotFoundException("المالك غير موجود ضمن نفس الشركة");

            var agentExists = await _propertyLookupRepository.ExistsInCompanyAsync(
                request.AgentId, request.CompanyId, ct);

            if (!agentExists)
                throw new NotFoundException("الوكيل غير موجود ضمن نفس الشركة");

            if (request.ParentPropertyId.HasValue)
            {
                if (request.ParentPropertyId.Value == request.Id)
                    throw new ValidationException("لا يمكن ربط العقار بنفسه كعقار أب");

                var parentExists = await _propertyLookupRepository.ExistsInCompanyAsync(
                    request.ParentPropertyId.Value, request.CompanyId, ct);

                if (!parentExists)
                    throw new NotFoundException("العقار الأب غير موجود ضمن نفس الشركة");
            }

            entity.Title = request.Title;
            entity.Description = request.Description;
            entity.Purpose = request.Purpose;
            entity.PropertyStatus = request.PropertyStatus;
            entity.Price = request.Price;
            entity.Area = request.Area;
            entity.City = request.City;
            entity.District = request.District;
            entity.Address = request.Address;
            entity.Latitude = request.Latitude;
            entity.Longitude = request.Longitude;
            entity.ParkingSpots = request.ParkingSpots;
            entity.AgeInYears = request.AgeInYears;
            entity.FacingDirection = request.FacingDirection;
            entity.FurnishedStatus = request.FurnishedStatus;
            entity.RegaLicenseNumber = request.RegaLicenseNumber;
            entity.DeedNumber = request.DeedNumber;
            entity.MunicipalityNumber = request.MunicipalityNumber;
            entity.IsFeatured = request.IsFeatured;
            entity.ParentPropertyId = request.ParentPropertyId;
            entity.OwnerId = request.OwnerId;
            entity.AgentId = request.AgentId;

            entity.Bedrooms = request.Bedrooms;
            entity.Bathrooms = request.Bathrooms;
            entity.Floors = request.Floors;
            entity.MaidRoom = request.MaidRoom;
            entity.DriverRoom = request.DriverRoom;
            entity.Pool = request.Pool;
            entity.GardenArea = request.GardenArea;

            _villaRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync(ct);

            return ApiResponse<Guid>.Ok(entity.Id, "تم تعديل الفيلا بنجاح");
        }
    }
}
