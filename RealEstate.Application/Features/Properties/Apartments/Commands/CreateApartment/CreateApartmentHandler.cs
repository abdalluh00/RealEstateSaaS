using MediatR;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Domain.Interfaces;
using RealEstate.Domain.Interfaces.Properties;
using RealEstate.Domain.Interfaces.Properties.RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Apartments.Commands.CreateApartment
{
    public class CreateApartmentHandler : IRequestHandler<CreateApartmentCommand, ApiResponse<Guid>>
    {
        private readonly IApartmentRepository _apartmentRepository;
        private readonly IPropertyLookupRepository _propertyLookupRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateApartmentHandler(
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

        public async Task<ApiResponse<Guid>> Handle(CreateApartmentCommand request, CancellationToken ct)
        {
            var dto = request.Apartment;

            // Validate owner in same company
            var ownerExists = await _ownerRepository.ExistsInCompanyAsync(dto.OwnerId, request.CompanyId);
            if (!ownerExists)
                throw new NotFoundException("المالك غير موجود ضمن نفس الشركة");

            // Validate agent in same company
            var agentExists = await _userRepository.ExistsInCompanyAsync(dto.AgentId, request.CompanyId);
            if (!agentExists)
                throw new NotFoundException("الوسيط/المستخدم غير موجود ضمن نفس الشركة");

            // Validate parent property if provided
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
                    dto.UnitNumber);

                if (duplicateUnit)
                    throw new ConflictException("رقم الوحدة مستخدم مسبقاً داخل العقار الأب");
            }

            var apartment = new ApartmentProperty
            {
                CompanyId = request.CompanyId,
                PropertyCode = string.Empty, // temporary until property code generator is added

                Title = dto.Title,
                Description = dto.Description,
                Purpose = dto.Purpose,
                PropertyStatus = dto.PropertyStatus,
                Price = dto.Price,
                Area = dto.Area,
                City = dto.City,
                District = dto.District,
                Address = dto.Address,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                ParkingSpots = dto.ParkingSpots,
                AgeInYears = dto.AgeInYears,
                FacingDirection = dto.FacingDirection,
                FurnishedStatus = dto.FurnishedStatus,
                RegaLicenseNumber = dto.RegaLicenseNumber,
                DeedNumber = dto.DeedNumber,
                MunicipalityNumber = dto.MunicipalityNumber,
                IsFeatured = dto.IsFeatured,
                ParentPropertyId = dto.ParentPropertyId,
                OwnerId = dto.OwnerId,
                AgentId = dto.AgentId,

                UnitNumber = dto.UnitNumber,
                Bedrooms = dto.Bedrooms,
                Bathrooms = dto.Bathrooms,
                FloorNumber = dto.FloorNumber,
                LivingRooms = dto.LivingRooms,
                HasMaidRoom = dto.HasMaidRoom,
                HasElevator = dto.HasElevator,
                HasCentralAc = dto.HasCentralAc,
                HasBalcony = dto.HasBalcony
            };

            await _apartmentRepository.AddAsync(apartment);
            await _unitOfWork.SaveChangesAsync(ct);

            return ApiResponse<Guid>.Ok(apartment.Id, "تم إنشاء الشقة بنجاح");
        }
    }
}
