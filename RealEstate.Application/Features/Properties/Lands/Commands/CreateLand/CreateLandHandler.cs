using MediatR;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Lands.Commands.CreateLand
{
    public sealed class CreateLandCommandHandler
        : IRequestHandler<CreateLandCommand, ApiResponse<Guid>>
    {
        private readonly ILandRepository _lands;
        private readonly IUnitOfWork _uow;
        private readonly IPropertyCodeGenerator _codeGenerator;

        public CreateLandCommandHandler(
            ILandRepository lands,
            IUnitOfWork uow,
            IPropertyCodeGenerator codeGenerator)
        {
            _lands = lands;
            _uow = uow;
            _codeGenerator = codeGenerator;
        }

        public async Task<ApiResponse<Guid>> Handle(
            CreateLandCommand cmd,
            CancellationToken ct)
        {
            var code = await _codeGenerator.GenerateAsync(cmd.CompanyId, "", ct);

            var land = new LandProperty
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
                AgeInYears = cmd.AgeInYears,
                FacingDirection = cmd.FacingDirection,
                RegaLicenseNumber = cmd.RegaLicenseNumber,
                DeedNumber = cmd.DeedNumber,
                MunicipalityNumber = cmd.MunicipalityNumber,
                CompanyId = cmd.CompanyId,
                OwnerId = cmd.OwnerId,
                AgentId = cmd.AgentId,
                StreetWidth = cmd.StreetWidth,
                NumberOfStreets = cmd.NumberOfStreets,
                ZoningType = cmd.ZoningType,
                LandShape = cmd.LandShape,
                IsCornerLand = cmd.IsCornerLand,
                IsWalled = cmd.IsWalled,
                HasElectricity = cmd.HasElectricity,
                HasWater = cmd.HasWater,
                HasSewer = cmd.HasSewer
            };

            _lands.Add(land);
            await _uow.SaveChangesAsync(ct);

            return ApiResponse<Guid>.Ok(land.Id, "تم إنشاء الأرض بنجاح");
        }
    }
}