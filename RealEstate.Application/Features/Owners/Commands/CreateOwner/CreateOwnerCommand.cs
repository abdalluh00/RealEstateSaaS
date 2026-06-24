using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Owners.Commands.CreateOwner
{
    public record CreateOwnerCommand(
    string FullName,
    string Phone,
    string? Email,
    string? IdNumber,
    string? Notes
) : IRequest<ApiResponse<Guid>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
