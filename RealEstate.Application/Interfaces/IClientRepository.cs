using RealEstate.Application.DTOs.Clients;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Interfaces
{
    public interface IClientRepository : IGenericRepository<Client>
    {
        // ── Queries ───────────────────────────────────────
        Task<PagedResult<ClientListDto>> GetPagedAsync(
            Guid companyId,
            int page,
            int pageSize,
            LeadStatus? leadStatus = null,
            LeadSource? source = null,
            bool? isActive = null,
            Guid? assignedAgentId = null,
            string? search = null,
            CancellationToken ct = default);

        Task<ClientDetailDto?> GetDetailByIdAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default);

        // ── Validation ────────────────────────────────────
        Task<bool> PhoneExistsAsync(
            string phone,
            Guid companyId,
            CancellationToken ct = default);

        Task<bool> PhoneExistsForAnotherClientAsync(
            string phone,
            Guid clientId,
            Guid companyId,
            CancellationToken ct = default);

        Task<bool> ExistsInCompanyAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default);
    }
}