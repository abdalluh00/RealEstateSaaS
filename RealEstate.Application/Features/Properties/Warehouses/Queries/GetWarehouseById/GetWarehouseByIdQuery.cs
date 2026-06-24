using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Domain.ReadModels;
using RealEstate.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Warehouses.Queries.GetWarehouseById
{
    public record GetWarehouseByIdQuery(Guid Id)
         : IRequest<ApiResponse<WarehouseDetailsDto>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
