using MediatR;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Properties.Warehouses.Commands.CreateWarehouse
{
    public class CreateWarehouseCommandHandler
       : IRequestHandler<CreateWarehouseCommand, ApiResponse<Guid>>
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPropertyCodeGenerator _codeGenerator;


        public CreateWarehouseCommandHandler(
            IWarehouseRepository warehouseRepository,
            IPropertyCodeGenerator codeGenerator,
            IUnitOfWork unitOfWork)
        {
            _warehouseRepository = warehouseRepository;
           _codeGenerator = codeGenerator;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<Guid>> Handle(CreateWarehouseCommand cmd, CancellationToken ct)
        {
            var code = await _codeGenerator.GenerateAsync(cmd.CompanyId, "", ct);

            var warehouse = new WarehouseProperty
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
                CeilingHeight = cmd.CeilingHeight,
                LoadingDocks = cmd.LoadingDocks,
                GateCount = cmd.GateCount,
                ElectricityCapacity = (ElectricityCapacity)cmd.ElectricityCapacity!,
                HasOfficeSpace = cmd.HasOfficeSpace,
                HasSecurityRoom = cmd.HasSecurityRoom,
                HasCCTV = cmd.HasCCTV,
                HasFireSystem = cmd.HasFireSystem,
                HasColdStorage = cmd.HasColdStorage,
                HasMosanada = cmd.HasMosanada,
                IsFenced = cmd.IsFenced,
                HasTruckAccess = cmd.HasTruckAccess
            };

             _warehouseRepository.Add(warehouse);
            await _unitOfWork.SaveChangesAsync(ct);

            return ApiResponse<Guid>.Ok(warehouse.Id, "تم إنشاء المستودع بنجاح");
        }
    }
}
