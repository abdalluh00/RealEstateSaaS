using MediatR;
using RealEstate.Application.DTOs.Clients;
using RealEstate.Application.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Clients.Queries.GetPagedClients
{
    public sealed class GetPagedClientsQueryHandler
        : IRequestHandler<GetPagedClientsQuery, ApiResponse<PagedResult<ClientListDto>>>
    {
        private readonly IClientRepository _clients;

        public GetPagedClientsQueryHandler(IClientRepository clients)
            => _clients = clients;

        public async Task<ApiResponse<PagedResult<ClientListDto>>> Handle(
            GetPagedClientsQuery query,
            CancellationToken ct)
        {
            var result = await _clients.GetPagedAsync(
                companyId: query.CompanyId,
                page: query.Page,
                pageSize: query.PageSize,
                leadStatus: query.LeadStatus,
                source: query.Source,
                isActive: query.IsActive,
                assignedAgentId: query.AssignedAgentId,
                search: query.Search,
                ct: ct);

            return ApiResponse<PagedResult<ClientListDto>>.Ok(result);
        }
    }
}