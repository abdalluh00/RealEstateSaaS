using MediatR;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Domain.Interfaces;
using RealEstate.Domain.Interfaces.Properties;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Lands.Commands.CreateLand
{
    public class CreateLandHandler : IRequestHandler<CreateLandCommand, ApiResponse<Guid>>
    {
        private readonly ILandRepository _landRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGenericRepository<Property> _propertyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateLandHandler(
            ILandRepository landRepository,
            IOwnerRepository ownerRepository,
            IUserRepository userRepository,
            IGenericRepository<Property> propertyRepository,
            IUnitOfWork unitOfWork)
        {
            _landRepository = landRepository;
            _ownerRepository = ownerRepository;
            _userRepository = userRepository;
            _propertyRepository = propertyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<Guid>> Handle(CreateLandCommand request, CancellationToken ct)
        {
            var owner = await _ownerRepository.GetByIdAsync(request.OwnerId);
            if (owner is null || owner.CompanyId != request.CompanyId)
                throw new ValidationException("المالك غير موجود أو لا يتبع لنفس الشركة");

            var agent = await _userRepository.GetByIdAsync(request.AgentId);
            if (agent is null || agent.CompanyId != request.CompanyId)
                throw new ValidationException("الوسيط/الموظف المسؤول غير موجود أو لا يتبع لنفس الشركة");

            if (request.ParentPropertyId.HasValue)
            {
                var parent = await _propertyRepository.GetByIdAsync(request.ParentPropertyId.Value);
                if (parent is null || parent.CompanyId != request.CompanyId)
                    throw new ValidationException("العقار الأب غير موجود أو لا يتبع لنفس الشركة");
            }

            var entity = new LandProperty
            {
                PropertyCode = GeneratePropertyCode(),
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
                FacingDirection = request.FacingDirection?.Trim(),
                FurnishedStatus = request.FurnishedStatus,

                RegaLicenseNumber = request.RegaLicenseNumber?.Trim(),
                DeedNumber = request.DeedNumber?.Trim(),
                MunicipalityNumber = request.MunicipalityNumber?.Trim(),

                IsFeatured = request.IsFeatured,

                CompanyId = request.CompanyId,
                OwnerId = request.OwnerId,
                AgentId = request.AgentId,

                StreetWidth = request.StreetWidth,
                ZoningType = request.ZoningType?.Trim(),
                CornerLand = request.CornerLand,
                NumberOfStreets = request.CornerLand ? request.NumberOfStreets : null
            };

            await _landRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync(ct);

            return ApiResponse<Guid>.Ok(entity.Id, "تم إنشاء الأرض بنجاح");
        }

        private static string GeneratePropertyCode()
            => $"LND-{DateTime.UtcNow:yyyyMMddHHmmss}";
    }
}
