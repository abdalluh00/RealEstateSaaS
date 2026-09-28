using MediatR;
using Microsoft.AspNetCore.Authorization;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.Owners.Commands.DeleteOwner
{
    [Authorize(Roles = $"{Roles.Admin}")]
    public sealed class DeleteOwnerCommand
         : IRequest<ApiResponse<bool>>, IAutoTenantRequest
    {
        public Guid Id { get; init; }
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }

}
