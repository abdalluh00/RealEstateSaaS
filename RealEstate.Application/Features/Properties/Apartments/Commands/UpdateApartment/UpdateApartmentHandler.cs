using MediatR;
using RealEstate.Domain.Interfaces;
using RealEstate.Domain.Interfaces.Properties;
using RealEstate.Domain.Interfaces.Properties.RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Apartments.Commands.UpdateApartment
{
    public class UpdateApartmentHandler : IRequestHandler<UpdateApartmentCommand, ApiResponse<Guid>>
    {
        private readonly IApartmentRepository _apartmentRepository;
        private readonly IPropertyLookupRepository _propertyLookupRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateApartmentHandler(
            IApartmentRepository apartmentRepository,
            IPropertyLookupRepository propertyLookupRepository,
            IOwnerRepository ownerRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _apartmentRepository = apartmentRepository;
            _propertyLookupRepository = propertyLookupRepository;
            _ownerRepository = ownerRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<Guid>> Handle(UpdateApartmentCommand request, CancellationToken ct)
        {
            var apartment = await _apartmentRepository.GetByIdAsync(request.ApartmentId, request.CompanyId);
            if (apartment is null)
                throw new NotFoundException("الشقة غير موجودة");

            var dto = request.Apartment;

            var ownerExists = await _ownerRepository.ExistsInCompanyAsync(dto.OwnerId, request.CompanyId);
            if (!ownerExists)
                throw new NotFoundException("المالك غير موجود ضمن نفس الشركة");

            var agentExists = await _userRepository.ExistsInCompanyAsync(dto.AgentId, request.CompanyId);
            if (!agentExists)
                throw new NotFoundException("الوسيط/المستخدم غير موجود ضمن نفس الشركة");

            if (dto.ParentPropertyId.HasValue)
            {
                var parentExists = await _propertyLookupRepository.ExistsInCompanyAsync(
                    dto.ParentPropertyId.Value,
                    request.CompanyId);

                if (!parentExists)
                    throw new NotFoundException("العقار الأب غير موجود ضمن نفس الشركة");

                var duplicateUnit = await _apartmentRepository.UnitNumberExistsUnderParentAsync(
                    request.CompanyId,
                    dto.ParentPropertyId.Value,
                    dto.UnitNumber,
                    apartment.Id);

                if (duplicateUnit)
                    throw new ConflictException("رقم الوحدة مستخدم مسبقاً داخل العقار الأب");
            }

            apartment.Title = dto.Title;
            apartment.Description = dto.Description;
            apartment.Purpose = dto.Purpose;
            apartment.PropertyStatus = dto.PropertyStatus;
            apartment.Price = dto.Price;
            apartment.Area = dto.Area;
            apartment.City = dto.City;
            apartment.District = dto.District;
            apartment.Address = dto.Address;
            apartment.Latitude = dto.Latitude;
            apartment.Longitude = dto.Longitude;
            apartment.ParkingSpots = dto.ParkingSpots;
            apartment.AgeInYears = dto.AgeInYears;
            apartment.FacingDirection = dto.FacingDirection;
            apartment.FurnishedStatus = dto.FurnishedStatus;
            apartment.RegaLicenseNumber = dto.RegaLicenseNumber;
            apartment.DeedNumber = dto.DeedNumber;
            apartment.MunicipalityNumber = dto.MunicipalityNumber;
            apartment.IsFeatured = dto.IsFeatured;
            apartment.ParentPropertyId = dto.ParentPropertyId;
            apartment.OwnerId = dto.OwnerId;
            apartment.AgentId = dto.AgentId;

            apartment.UnitNumber = dto.UnitNumber;
            apartment.Bedrooms = dto.Bedrooms;
            apartment.Bathrooms = dto.Bathrooms;
            apartment.FloorNumber = dto.FloorNumber;
            apartment.LivingRooms = dto.LivingRooms;
            apartment.HasMaidRoom = dto.HasMaidRoom;
            apartment.HasElevator = dto.HasElevator;
            apartment.HasCentralAc = dto.HasCentralAc;
            apartment.HasBalcony = dto.HasBalcony;

            _apartmentRepository.Update(apartment);
            await _unitOfWork.SaveChangesAsync(ct);

            return ApiResponse<Guid>.Ok(apartment.Id, "تم تحديث الشقة بنجاح");
        }
    }
}
