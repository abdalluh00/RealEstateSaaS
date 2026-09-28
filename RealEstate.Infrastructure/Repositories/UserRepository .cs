using Microsoft.EntityFrameworkCore;
using RealEstate.Application.DTOs.Users;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Persistence;
using RealEstate.Shared.Common;
using RealEstate.Application.Common.Extensions;
using System.Linq.Expressions;

namespace RealEstate.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        private static readonly Expression<Func<User, UserListDto>> ToListDto =
            u => new UserListDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Phone = u.Phone,
                Role = u.Role.ToArabicString(),
                IsActive = u.IsActive,
                IsInvitationAccepted = u.IsInvitationAccepted,
                LastLoginAt = u.LastLoginAt,
                CreatedAt = u.CreatedAt
            };

        // ── Paged ─────────────────────────────────────────
        public async Task<PagedResult<UserListDto>> GetPagedAsync(
            Guid companyId,
            int page,
            int pageSize,
            UserRole? role = null,
            bool? isActive = null,
            CancellationToken ct = default)
        {
            var query = _dbSet
                .AsNoTracking()
                .Where(u => u.CompanyId == companyId);

            if (role.HasValue)
                query = query.Where(u => u.Role == role.Value);

            if (isActive.HasValue)
                query = query.Where(u => u.IsActive == isActive.Value);

            var totalCount = await query.CountAsync(ct);

            if (totalCount == 0)
                return PagedResult<UserListDto>.Empty(page, pageSize);

            var items = await query
                .OrderBy(u => u.FullName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(ToListDto)
                .ToListAsync(ct);

            return PagedResult<UserListDto>.Create(items, totalCount, page, pageSize);
        }

        // ── Detail ────────────────────────────────────────
        public async Task<UserDetailDto?> GetDetailByIdAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default) =>
            await _dbSet
                .AsNoTracking()
                .Where(u => u.Id == id && u.CompanyId == companyId)
                .Select(u => new UserDetailDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    Phone = u.Phone,
                    Role = u.Role.ToArabicString(),
                    IsActive = u.IsActive,
                    IsInvitationAccepted = u.IsInvitationAccepted,
                    InvitationExpiry = u.InvitationExpiry,
                    InvitationAcceptedAt = u.InvitationAcceptedAt,
                    LastLoginAt = u.LastLoginAt,
                    CreatedAt = u.CreatedAt,
                    UpdatedAt = u.UpdatedAt
                })
                .FirstOrDefaultAsync(ct);

        // ── Auth ──────────────────────────────────────────
        public async Task<User?> GetByEmailAsync(
            string email,
            CancellationToken ct = default) =>
            await _dbSet
                .FirstOrDefaultAsync(u => u.Email == email, ct);

        public async Task<User?> GetByInvitationTokenAsync(
            string token,
            CancellationToken ct = default) =>
            await _dbSet
                .FirstOrDefaultAsync(u => u.InvitationToken == token, ct);

        public async Task<User?> GetByPasswordResetTokenAsync(
            string token,
            CancellationToken ct = default) =>
            await _dbSet
                .FirstOrDefaultAsync(u => u.PasswordResetToken == token, ct);

        // ── Validation ────────────────────────────────────
        public async Task<bool> EmailExistsAsync(
            string email,
            CancellationToken ct = default) =>
            await _dbSet.AnyAsync(u => u.Email == email, ct);

        public async Task<bool> EmailExistsForAnotherUserAsync(
            string email,
            Guid userId,
            CancellationToken ct = default) =>
            await _dbSet
                .AnyAsync(u => u.Email == email && u.Id != userId, ct);
    }
}