using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.DTOs.Clients;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Clients.Queries.GetPagedClients
{
    public sealed class GetPagedClientsQuery
        : IRequest<ApiResponse<PagedResult<ClientListDto>>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public LeadStatus? LeadStatus { get; init; }
        public LeadSource? Source { get; init; }
        public bool? IsActive { get; init; }
        public Guid? AssignedAgentId { get; init; }
        public string? Search { get; init; }
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}