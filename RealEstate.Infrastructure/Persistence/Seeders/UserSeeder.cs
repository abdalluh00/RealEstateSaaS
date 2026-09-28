using Microsoft.Extensions.Logging;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Persistence;
using RealEstate.Shared.Helpers;

namespace RealEstate.Infrastructure.Persistence.Seeders
{
    public static class UserSeeder
    {
        public static async Task<List<User>> SeedAsync(
            AppDbContext context,
            List<Company> companies,
            ILogger logger)
        {
            var users = new List<User>();

            foreach (var company in companies)
            {
                var suffix = company.SubscriptionPlan.ToString().ToLower();

                // ── Owner ─────────────────────────────────
                users.Add(new User
                {
                    Id = Guid.NewGuid(),
                    FullName = $"مالك {company.Name}",
                    Email = $"owner.{suffix}@realestate.test",
                    Phone = $"05{new Random().Next(10000000, 99999999)}",
                    PasswordHash = PasswordHelper.Hash("Test@1234"),
                    Role = UserRole.Owner,
                    CompanyId = company.Id,
                    IsActive = true,
                    IsInvitationAccepted = true,
                    InvitationAcceptedAt = DateTime.UtcNow,
                    LastLoginAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                });

                // ── Admin ─────────────────────────────────
                users.Add(new User
                {
                    Id = Guid.NewGuid(),
                    FullName = $"مدير {company.Name}",
                    Email = $"admin.{suffix}@realestate.test",
                    Phone = $"05{new Random().Next(10000000, 99999999)}",
                    PasswordHash = PasswordHelper.Hash("Test@1234"),
                    Role = UserRole.Admin,
                    CompanyId = company.Id,
                    IsActive = true,
                    IsInvitationAccepted = true,
                    InvitationAcceptedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                });

                // ── Agent ─────────────────────────────────
                users.Add(new User
                {
                    Id = Guid.NewGuid(),
                    FullName = $"وكيل {company.Name}",
                    Email = $"agent.{suffix}@realestate.test",
                    Phone = $"05{new Random().Next(10000000, 99999999)}",
                    PasswordHash = PasswordHelper.Hash("Test@1234"),
                    Role = UserRole.Agent,
                    CompanyId = company.Id,
                    IsActive = true,
                    IsInvitationAccepted = true,
                    InvitationAcceptedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                });

                // ── Viewer ────────────────────────────────
                users.Add(new User
                {
                    Id = Guid.NewGuid(),
                    FullName = $"مشاهد {company.Name}",
                    Email = $"viewer.{suffix}@realestate.test",
                    Phone = $"05{new Random().Next(10000000, 99999999)}",
                    PasswordHash = PasswordHelper.Hash("Test@1234"),
                    Role = UserRole.Viewer,
                    CompanyId = company.Id,
                    IsActive = true,
                    IsInvitationAccepted = true,
                    InvitationAcceptedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                });
            }

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();

            logger.LogInformation("Seeded {Count} users", users.Count);
            return users;
        }
    }
}