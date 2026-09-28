using MediatR;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
using RealEstate.Domain.Common.Enums;
namespace RealEstate.Application.Features.Properties.Apartments.Commands.UpdateApartment
{
    public sealed class UpdateApartmentCommandHandler
        : IRequestHandler<UpdateApartmentCommand, ApiResponse<bool>>
    {
        private readonly IApartmentRepository _apartments;
        private readonly IUnitOfWork _uow;

        public UpdateApartmentCommandHandler(
            IApartmentRepository apartments,
            IUnitOfWork uow)
        {
            _apartments = apartments;
            _uow = uow;
        }

        public async Task<ApiResponse<bool>> Handle(
            UpdateApartmentCommand cmd,
            CancellationToken ct)
        {
            // ── Load tracked entity via Query() ───────────
            var apartment = await _apartments.Query()
                .FirstOrDefaultAsync(x => x.Id == cmd.Id
                                       && x.CompanyId == cmd.CompanyId, ct);

            if (apartment is null)
                throw new NotFoundException("الشقة", cmd.Id);

            // ── Apply changes ─────────────────────────────
            apartment.Title = cmd.Title;
            apartment.Description = cmd.Description;
            apartment.Purpose = cmd.Purpose;
            apartment.Price = cmd.Price;
            apartment.Area = cmd.Area;
            apartment.City = cmd.City;
            apartment.District = cmd.District;
            apartment.Address = cmd.Address;
            apartment.Latitude = cmd.Latitude;
            apartment.Longitude = cmd.Longitude;
            apartment.ParkingSpots = cmd.ParkingSpots;
            apartment.AgeInYears = cmd.AgeInYears;
            apartment.FacingDirection =(FacingDirection) cmd.FacingDirection!;
            apartment.RegaLicenseNumber = cmd.RegaLicenseNumber;
            apartment.DeedNumber = cmd.DeedNumber;
            apartment.MunicipalityNumber = cmd.MunicipalityNumber;
            apartment.OwnerId = cmd.OwnerId;
            apartment.AgentId = cmd.AgentId;
            apartment.IsFeatured = cmd.IsFeatured;
            apartment.IsPublished = cmd.IsPublished;
            apartment.Bedrooms = cmd.Bedrooms;
            apartment.Bathrooms = cmd.Bathrooms;
            apartment.LivingRooms = cmd.LivingRooms;
            apartment.FloorNumber = cmd.FloorNumber;
            apartment.HasMaidRoom = cmd.HasMaidRoom;
            apartment.HasElevator = cmd.HasElevator;
            apartment.HasCentralAC = cmd.HasCentralAC;
            apartment.HasBalcony = cmd.HasBalcony;
            apartment.HasStorage = cmd.HasStorage;
            apartment.FurnishedStatus = cmd.FurnishedStatus;
            // UpdatedAt stamped automatically by SaveChangesAsync

            await _uow.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم تحديث الشقة بنجاح");
        }
    }
}
