using MediatR;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Properties.Offices.Commands.CreateOffice
{
    public class CreateOfficeHandler : IRequestHandler<CreateOfficeCommand, ApiResponse<Guid>>
    {
        private readonly IOfficeRepository _officeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateOfficeHandler(
            IOfficeRepository officeRepository,
            IUnitOfWork unitOfWork)
        {
            _officeRepository = officeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<Guid>> Handle(CreateOfficeCommand request, CancellationToken ct)
        {
          

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
                FacingDirection = request.FacingDirection,
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
                FloorNumber = request.Floor,
                Bathrooms = request.Bathrooms,
                OfficesCount = request.OfficesCount,
                MeetingRooms = request.MeetingRooms,
                HasElevator = request.Elevator,
                HasCentralAC = request.CentralAC,
                HasReceptionArea = request.ReceptionArea
            };

             _officeRepository.Add(office);
            await _unitOfWork.SaveChangesAsync(ct);

            return ApiResponse<Guid>.Ok(office.Id, "تم إنشاء المكتب بنجاح");
        }
    }
}
