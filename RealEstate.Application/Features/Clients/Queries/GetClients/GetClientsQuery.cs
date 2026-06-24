
using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.Features.Clients.DTO;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Clients.Queries.GetClients
{
    public record GetClientsQuery(string? LeadStatus = null)
    : IRequest<ApiResponse<List<ClientDto>>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }



}
