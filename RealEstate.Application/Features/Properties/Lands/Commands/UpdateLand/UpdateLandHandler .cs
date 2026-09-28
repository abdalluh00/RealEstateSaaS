using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Lands.Commands.UpdateLand
{
    public sealed class UpdateLandCommandHandler
        : IRequestHandler<UpdateLandCommand, ApiResponse<bool>>
    {
        private readonly ILandRepository _lands;
        private readonly IUnitOfWork _uow;

        public UpdateLandCommandHandler(ILandRepository lands, IUnitOfWork uow)
        {
            _lands = lands;
            _uow = uow;
        }

        public async Task<ApiResponse<bool>> Handle(
            UpdateLandCommand cmd,
            CancellationToken ct)
        {
            var land = await _lands.Query()
                .FirstOrDefaultAsync(x => x.Id == cmd.Id
                                       && x.CompanyId == cmd.CompanyId, ct);

            if (land is null)
                throw new NotFoundException("الأرض", cmd.Id);

            land.Title = cmd.Title;
            land.Description = cmd.Description;
            land.Purpose = cmd.Purpose;
            land.Price = cmd.Price;
            land.Area = cmd.Area;
            land.City = cmd.City;
            land.District = cmd.District;
            land.Address = cmd.Address;
            land.Latitude = cmd.Latitude;
            land.Longitude = cmd.Longitude;
            land.AgeInYears = cmd.AgeInYears;
            land.FacingDirection = cmd.FacingDirection;
            land.RegaLicenseNumber = cmd.RegaLicenseNumber;
            land.DeedNumber = cmd.DeedNumber;
            land.MunicipalityNumber = cmd.MunicipalityNumber;
            land.OwnerId = cmd.OwnerId;
            land.AgentId = cmd.AgentId;
            land.IsFeatured = cmd.IsFeatured;
            land.IsPublished = cmd.IsPublished;
            land.StreetWidth = cmd.StreetWidth;
            land.NumberOfStreets = cmd.NumberOfStreets;
            land.ZoningType = cmd.ZoningType;
            land.LandShape = cmd.LandShape;
            land.IsCornerLand = cmd.IsCornerLand;
            land.IsWalled = cmd.IsWalled;
            land.HasElectricity = cmd.HasElectricity;
            land.HasWater = cmd.HasWater;
            land.HasSewer = cmd.HasSewer;

            await _uow.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم تحديث الأرض بنجاح");
        }
    }
}