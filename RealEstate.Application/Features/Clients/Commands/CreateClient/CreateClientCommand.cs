using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.Clients.Commands.CreateClient
{
    public record CreateClientCommand(
    string FullName,
    string Phone,
    string? Email,
    string? Source,
    string? Notes
) : IRequest<ApiResponse<Guid>>, IAutoTenantRequest  
    {
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    };
}
