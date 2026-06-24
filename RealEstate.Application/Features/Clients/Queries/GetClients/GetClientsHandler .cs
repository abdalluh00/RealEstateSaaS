using MediatR;
using RealEstate.Application.Features.Clients.DTO;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Clients.Queries.GetClients
{
    public class GetClientsHandler : IRequestHandler<GetClientsQuery, ApiResponse<List<ClientDto>>>
    {
        private readonly IClientRepository _repo;

        public GetClientsHandler(IClientRepository repo) => _repo = repo;

        public async Task<ApiResponse<List<ClientDto>>> Handle(
            GetClientsQuery request,
            CancellationToken ct)
        {
            var clients = string.IsNullOrEmpty(request.LeadStatus)
                ? await _repo.GetByCompanyAsync(request.CompanyId)
                : await _repo.GetByLeadStatusAsync(request.CompanyId, request.LeadStatus);

            var result = clients.Select(c => new ClientDto(
                c.Id, c.FullName, c.Phone,
                c.Email, c.LeadStatus, c.Source, c.CreatedAt
            )).ToList();

            return ApiResponse<List<ClientDto>>.Ok(result);
        }
    }
}
