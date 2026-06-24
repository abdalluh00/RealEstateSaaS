using MediatR;
using Microsoft.AspNetCore.Authorization;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.Features.Properties.Warehouses.DTOs;
using RealEstate.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Warehouses.Commands.CreateWarehouse
{
    [Authorize(Roles = "Owner,Admin")]
    public record CreateWarehouseCommand(CreateWarehouseRequestDto Warehouse)
        : IRequest<ApiResponse<Guid>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
