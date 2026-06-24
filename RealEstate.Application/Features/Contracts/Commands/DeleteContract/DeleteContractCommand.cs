using MediatR;
using Microsoft.AspNetCore.Authorization;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Contracts.Commands.DeleteContract
{
    [Authorize(Roles = "Owner,Admin")]
    public record DeleteContractCommand(Guid ContractId)
       : IRequest<ApiResponse<bool>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }

        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
