using MediatR;
using Microsoft.AspNetCore.Authorization;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Warehouses.Commands.DeleteWarehouse
{
    [Authorize(Roles = "Owner,Admin")]
    public record DeleteWarehouseCommand(Guid Id)
         : IRequest<ApiResponse<bool>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
