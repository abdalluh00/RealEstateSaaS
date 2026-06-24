using MediatR;
using Microsoft.AspNetCore.Authorization;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Lands.Commands.UpdateLand
{
    [Authorize(Roles = "Owner,Admin")]
    public record UpdateLandCommand(
       Guid Id,
       string Title,
       string? Description,
       PropertyPurpose Purpose,
       PropertyStatus PropertyStatus,
       decimal Price,
       decimal Area,
       string City,
       string District,
       string? Address,
       double? Latitude,
       double? Longitude,
       int? ParkingSpots,
       int? AgeInYears,
       string? FacingDirection,
       FurnishedStatus FurnishedStatus,
       string? RegaLicenseNumber,
       string? DeedNumber,
       string? MunicipalityNumber,
       bool IsFeatured,
       Guid OwnerId,
       Guid AgentId,
       Guid? ParentPropertyId,
       decimal? StreetWidth,
       string? ZoningType,
       bool CornerLand,
       int? NumberOfStreets
   ) : IRequest<ApiResponse<Guid>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
