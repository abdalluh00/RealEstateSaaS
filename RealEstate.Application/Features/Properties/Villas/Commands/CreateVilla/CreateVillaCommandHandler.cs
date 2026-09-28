using MediatR;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.Properties.Villas.Commands.CreateVilla
{
    public class CreateVillaCommandHandler
       : IRequestHandler<CreateVillaCommand, ApiResponse<Guid>>
    {
        private readonly IVillaRepository _villaRepository;
        private readonly IUnitOfWork _unitOfWork;
        IPropertyCodeGenerator _codeGenerator;

        public CreateVillaCommandHandler(
            IVillaRepository villaRepository,
             IPropertyCodeGenerator codeGenerator,
            IUnitOfWork unitOfWork)
        {
            _villaRepository = villaRepository;
            _codeGenerator = codeGenerator;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<Guid>> Handle(CreateVillaCommand cmd, CancellationToken ct)
        {
            var code = await _codeGenerator.GenerateAsync(cmd.CompanyId, "", ct);
            var villa = new VillaProperty
            {
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
                FacingDirection = cmd.FacingDirection,
                RegaLicenseNumber = cmd.RegaLicenseNumber,
                DeedNumber = cmd.DeedNumber,
                MunicipalityNumber = cmd.MunicipalityNumber,
                CompanyId = cmd.CompanyId,
                OwnerId = cmd.OwnerId,
                AgentId = cmd.AgentId,
                ParentPropertyId = cmd.ParentPropertyId,
                UnitNumber = cmd.UnitNumber,
                Bedrooms = cmd.Bedrooms,
                Bathrooms = cmd.Bathrooms,
                LivingRooms = cmd.LivingRooms,
                Floors = cmd.Floors,
                HasMaidRoom = cmd.MaidRoom,
                HasDriverRoom = cmd.DriverRoom,
                HasPool = cmd.Pool,
                HasGarden = cmd.HasGarden,
                GardenArea = cmd.GardenArea,
                HasElevator = cmd.HasElevator,
                HasMosque = cmd.HasMosque,
                HasMajlis = cmd.HasMajlis,
                HasStorage = cmd.HasStorage,
                HasCCTV = cmd.HasCCTV,
                HasGenerator = cmd.HasGenerator,
                FurnishedStatus = cmd.FurnishedStatus
            };

            _villaRepository.Add(villa);
            await _unitOfWork.SaveChangesAsync(ct);

            return ApiResponse<Guid>.Ok(villa.Id, "تم إنشاء الفيلا بنجاح");
        }
    }
}
