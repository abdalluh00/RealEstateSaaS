using MediatR;
using Microsoft.AspNetCore.Authorization;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Villas.Commands.DeleteVilla
{
    [Authorize(Roles = "Owner,Admin")]
    public sealed record DeleteVillaCommand : IRequest<ApiResponse<bool>>, IAutoTenantRequest
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
