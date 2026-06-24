using MediatR;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Domain.Interfaces;
using RealEstate.Domain.Interfaces.Properties;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Offices.Commands.CreateOffice
{
    public class CreateOfficeHandler : IRequestHandler<CreateOfficeCommand, ApiResponse<Guid>>
    {
        private readonly IOfficeRepository _officeRepository;
        private readonly IPropertyLookupRepository _propertyLookupRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateOfficeHandler(
            IOfficeRepository officeRepository,
            IPropertyLookupRepository propertyLookupRepository,
            IOwnerRepository ownerRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _officeRepository = officeRepository;
            _propertyLookupRepository = propertyLookupRepository;
            _ownerRepository = ownerRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<Guid>> Handle(CreateOfficeCommand request, CancellationToken ct)
        {
            // Validate optional owner
            if (request.OwnerId.HasValue)
            {
                var owner = await _ownerRepository.GetByIdAsync(request.OwnerId.Value);
                if (owner is null || owner.CompanyId != request.CompanyId)
                    throw new ValidationException("المالك غير موجود أو لا يتبع لنفس الشركة");
            }

            // Validate optional agent
            if (request.AgentId.HasValue)
            {
                var agent = await _userRepository.GetByIdAsync(request.AgentId.Value);
                if (agent is null || agent.CompanyId != request.CompanyId)
                    throw new ValidationException("الوسيط غير موجود أو لا يتبع لنفس الشركة");
            }

            // Validate optional parent property
            if (request.ParentPropertyId.HasValue)
            {
                var parentBelongsToCompany = await _propertyLookupRepository.ExistsInCompanyAsync(
                    request.ParentPropertyId.Value,
                    request.CompanyId,
                    ct);

                if (!parentBelongsToCompany)
                    throw new ValidationException("العقار الأب غير موجود أو لا يتبع لنفس الشركة");
            }

            var office = new OfficeProperty
            {
                PropertyCode = request.PropertyCode.Trim(),
                ParentPropertyId = request.ParentPropertyId,

                Title = request.Title.Trim(),
                Description = request.Description?.Trim(),

                Purpose = request.Purpose,
                PropertyStatus = request.PropertyStatus,

                Price = request.Price,
                Area = request.Area,

                City = request.City.Trim(),
                District = request.District.Trim(),
                Address = request.Address?.Trim(),
                Latitude = request.Latitude,
                Longitude = request.Longitude,

                ParkingSpots = request.ParkingSpots,
                AgeInYears = request.AgeInYears,
                FacingDirection = request.FacingDirection?.Trim(),
                FurnishedStatus = request.FurnishedStatus,

                RegaLicenseNumber = request.RegaLicenseNumber?.Trim(),
                DeedNumber = request.DeedNumber?.Trim(),
                MunicipalityNumber = request.MunicipalityNumber?.Trim(),

                IsFeatured = request.IsFeatured,

                CompanyId = request.CompanyId,
                OwnerId = request.OwnerId,
                AgentId = request.AgentId,

                // Office-specific
                UnitNumber = request.UnitNumber?.Trim(),
                Floor = request.Floor,
                Bathrooms = request.Bathrooms,
                OfficesCount = request.OfficesCount,
                MeetingRooms = request.MeetingRooms,
                Elevator = request.Elevator,
                CentralAC = request.CentralAC,
                ReceptionArea = request.ReceptionArea
            };

            await _officeRepository.AddAsync(office);
            await _unitOfWork.SaveChangesAsync(ct);

            return ApiResponse<Guid>.Ok(office.Id, "تم إنشاء المكتب بنجاح");
        }
    }
}
