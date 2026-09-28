using MediatR;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
namespace RealEstate.Application.Features.Properties.Apartments.Commands.CreateApartment
{
    public sealed class CreateApartmentCommandHandler
         : IRequestHandler<CreateApartmentCommand, ApiResponse<Guid>>
    {
        private readonly IApartmentRepository _apartments;
        private readonly IPropertyRepository _properties;
        private readonly IUnitOfWork _uow;
        private readonly IPropertyCodeGenerator _codeGenerator;

        public CreateApartmentCommandHandler(
            IApartmentRepository apartments,
            IPropertyRepository properties,
            IUnitOfWork uow,
            IPropertyCodeGenerator codeGenerator)
        {
            _apartments = apartments;
            _properties = properties;
            _uow = uow;
            _codeGenerator = codeGenerator;
        }

        public async Task<ApiResponse<Guid>> Handle(
            CreateApartmentCommand cmd,
            CancellationToken ct)
        {
            // ── Validate parent exists if provided ────────
            if (cmd.ParentPropertyId.HasValue)
            {
                var parentExists = await _properties.ExistsAsync(
                    cmd.ParentPropertyId.Value, cmd.CompanyId, ct);

                if (!parentExists)
                    throw new NotFoundException("العقار الأب", cmd.ParentPropertyId.Value);

                // ── Unit number must be unique inside parent
                var unitTaken = await _apartments.UnitNumberExistsAsync(
                    cmd.UnitNumber!, cmd.ParentPropertyId.Value, ct);

                if (unitTaken)
                    throw new ConflictException("رقم الوحدة مستخدم بالفعل في هذا العقار");
            }

            // ── Generate unique property code ─────────────
            var code = await _codeGenerator.GenerateAsync(cmd.CompanyId,"PROP", ct);

            // ── Build entity ──────────────────────────────
            var apartment = new ApartmentProperty
            {
                // Type is set in constructor automatically
                PropertyCode = code,
                Title = cmd.Title,
                Description = cmd.Description,
                Purpose = cmd.Purpose,
                PropertyStatus = PropertyStatus.Available,
                Price = cmd.Price,
                Area = cmd.Area,
                City = cmd.City,
                District = cmd.District,
                Address = cmd.Address,
                Latitude = cmd.Latitude,
                Longitude = cmd.Longitude,
                ParkingSpots = cmd.ParkingSpots,
                AgeInYears = cmd.AgeInYears,
                FacingDirection =(FacingDirection) cmd.FacingDirection!,
                RegaLicenseNumber = cmd.RegaLicenseNumber,
                DeedNumber = cmd.DeedNumber,
                MunicipalityNumber = cmd.MunicipalityNumber,
                CompanyId = cmd.CompanyId,
                OwnerId = cmd.OwnerId,
                AgentId = cmd.AgentId,
                ParentPropertyId = cmd.ParentPropertyId,
                UnitNumber = cmd.UnitNumber,

                // ── Apartment specific ────────────────────
                Bedrooms = cmd.Bedrooms,
                Bathrooms = cmd.Bathrooms,
                LivingRooms = cmd.LivingRooms,
                FloorNumber = cmd.FloorNumber,
                HasMaidRoom = cmd.HasMaidRoom,
                HasElevator = cmd.HasElevator,
                HasCentralAC = cmd.HasCentralAC,
                HasBalcony = cmd.HasBalcony,
                HasStorage = cmd.HasStorage,
                FurnishedStatus = cmd.FurnishedStatus
            };

            _apartments.Add(apartment);
            await _uow.SaveChangesAsync(ct);

            return ApiResponse<Guid>.Ok(apartment.Id, "تم إنشاء الشقة بنجاح");
        }
    }
}
