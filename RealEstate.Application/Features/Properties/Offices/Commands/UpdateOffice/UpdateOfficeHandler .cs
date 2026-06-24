using MediatR;
using RealEstate.Domain.Interfaces;
using RealEstate.Domain.Interfaces.Properties;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Offices.Commands.UpdateOffice
{
    public class UpdateOfficeHandler : IRequestHandler<UpdateOfficeCommand, ApiResponse<Guid>>
    {
        private readonly IOfficeRepository _officeRepository;
        private readonly IPropertyLookupRepository _propertyLookupRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOfficeHandler(
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

        public async Task<ApiResponse<Guid>> Handle(UpdateOfficeCommand request, CancellationToken ct)
        {
            var office = await _officeRepository.GetByIdForCompanyAsync(request.Id, request.CompanyId, ct);
            if (office is null)
                throw new NotFoundException("المكتب غير موجود");

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

            office.PropertyCode = request.PropertyCode.Trim();
            office.ParentPropertyId = request.ParentPropertyId;

            office.Title = request.Title.Trim();
            office.Description = request.Description?.Trim();

            office.Purpose = request.Purpose;
            office.PropertyStatus = request.PropertyStatus;

            office.Price = request.Price;
            office.Area = request.Area;

            office.City = request.City.Trim();
            office.District = request.District.Trim();
            office.Address = request.Address?.Trim();
            office.Latitude = request.Latitude;
            office.Longitude = request.Longitude;

            office.ParkingSpots = request.ParkingSpots;
            office.AgeInYears = request.AgeInYears;
            office.FacingDirection = request.FacingDirection?.Trim();
            office.FurnishedStatus = request.FurnishedStatus;

            office.RegaLicenseNumber = request.RegaLicenseNumber?.Trim();
            office.DeedNumber = request.DeedNumber?.Trim();
            office.MunicipalityNumber = request.MunicipalityNumber?.Trim();

            office.IsFeatured = request.IsFeatured;

            office.OwnerId = request.OwnerId;
            office.AgentId = request.AgentId;

            // Office-specific
            office.UnitNumber = request.UnitNumber?.Trim();
            office.Floor = request.Floor;
            office.Bathrooms = request.Bathrooms;
            office.OfficesCount = request.OfficesCount;
            office.MeetingRooms = request.MeetingRooms;
            office.Elevator = request.Elevator;
            office.CentralAC = request.CentralAC;
            office.ReceptionArea = request.ReceptionArea;

            await _unitOfWork.SaveChangesAsync(ct);

            return ApiResponse<Guid>.Ok(office.Id, "تم تحديث المكتب بنجاح");
        }
    }
}
