using RealEstate.Application.DTOs.Users;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        // ── Queries ───────────────────────────────────────
        Task<PagedResult<UserListDto>> GetPagedAsync(
            Guid companyId,
            int page,
            int pageSize,
            UserRole? role = null,
            bool? isActive = null,
            CancellationToken ct = default);

        Task<UserDetailDto?> GetDetailByIdAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default);

        // ── Auth ──────────────────────────────────────────
        Task<User?> GetByEmailAsync(
            string email,
            CancellationToken ct = default);

        Task<User?> GetByInvitationTokenAsync(
            string token,
            CancellationToken ct = default);

        Task<User?> GetByPasswordResetTokenAsync(
            string token,
            CancellationToken ct = default);

        // ── Validation ────────────────────────────────────
        Task<bool> EmailExistsAsync(
            string email,
            CancellationToken ct = default);

        Task<bool> EmailExistsForAnotherUserAsync(
            string email,
            Guid userId,
            CancellationToken ct = default);
    }
}