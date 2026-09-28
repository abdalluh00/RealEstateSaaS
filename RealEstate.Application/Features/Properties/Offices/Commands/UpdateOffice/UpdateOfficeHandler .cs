using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Offices.Commands.UpdateOffice
{
    public class UpdateOfficeHandler : IRequestHandler<UpdateOfficeCommand, ApiResponse<bool>>
    {
        private readonly IOfficeRepository _officeRepository;
       
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOfficeHandler(
            IOfficeRepository officeRepository,
            IUnitOfWork unitOfWork)
        {
            _officeRepository = officeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<bool>> Handle(UpdateOfficeCommand request, CancellationToken ct)
        {
            var office = await _officeRepository.Query()
                .FirstOrDefaultAsync(x => x.Id == request.Id
                                       && x.CompanyId == request.CompanyId, ct);

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
            office.FacingDirection = request.FacingDirection;
            office.FurnishedStatus = request.FurnishedStatus;

            office.RegaLicenseNumber = request.RegaLicenseNumber?.Trim();
            office.DeedNumber = request.DeedNumber?.Trim();
            office.MunicipalityNumber = request.MunicipalityNumber?.Trim();

            office.IsFeatured = request.IsFeatured;

            office.OwnerId = request.OwnerId;
            office.AgentId = request.AgentId;

            // Office-specific
            office.UnitNumber = request.UnitNumber?.Trim();
            office.FloorNumber = request.Floor;
            office.Bathrooms = request.Bathrooms;
            office.OfficesCount = request.OfficesCount;
            office.MeetingRooms = request.MeetingRooms;
            office.HasElevator = request.Elevator;
            office.HasCentralAC = request.CentralAC;
            office.HasReceptionArea = request.ReceptionArea;

            await _unitOfWork.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم تحديث المكتب بنجاح");
        }
    }
}
