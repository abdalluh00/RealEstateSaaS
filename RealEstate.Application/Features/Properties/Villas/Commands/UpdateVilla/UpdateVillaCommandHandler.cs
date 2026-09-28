using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Villas.Commands.UpdateVilla
{
    public class UpdateVillaCommandHandler
        : IRequestHandler<UpdateVillaCommand, ApiResponse<bool>>
    {
        private readonly IVillaRepository _villaRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateVillaCommandHandler(
            IVillaRepository villaRepository,
            
            IUnitOfWork unitOfWork)
        {
            _villaRepository = villaRepository;
           
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<bool>> Handle(UpdateVillaCommand request, CancellationToken ct)
        {
           var entity = await _villaRepository.Query().FirstOrDefaultAsync(x => x.Id == request.Id 
           && x.CompanyId == request.CompanyId);

           if(entity == null)
                throw new NotFoundException($"الفيلا غير موجودة {request.Id} not found.");

            entity.Title = request.Title;
            entity.Description = request.Description;
            entity.Purpose = request.Purpose;
            entity.PropertyStatus = request.PropertyStatus;
            entity.Price = request.Price;
            entity.Area = request.Area;
            entity.City = request.City;
            entity.District = request.District;
            entity.Address = request.Address;
            entity.Latitude = request.Latitude;
            entity.Longitude = request.Longitude;
            entity.ParkingSpots = request.ParkingSpots;
            entity.AgeInYears = request.AgeInYears;
            entity.FacingDirection = request.FacingDirection;
            entity.FurnishedStatus = request.FurnishedStatus;
            entity.RegaLicenseNumber = request.RegaLicenseNumber;
            entity.DeedNumber = request.DeedNumber;
            entity.MunicipalityNumber = request.MunicipalityNumber;
            entity.IsFeatured = request.IsFeatured;
            entity.ParentPropertyId = request.ParentPropertyId;
            entity.OwnerId = request.OwnerId;
            entity.AgentId = request.AgentId;

            entity.Bedrooms = request.Bedrooms;
            entity.Bathrooms = request.Bathrooms;
            entity.Floors = request.Floors;
            entity.HasMaidRoom = request.MaidRoom;
            entity.HasDriverRoom = request.DriverRoom;
            entity.HasPool = request.Pool;
            entity.GardenArea = request.GardenArea;

            _villaRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم تعديل الفيلا بنجاح");
        }
    }
}
