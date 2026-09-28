using Microsoft.Extensions.Logging;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Persistence;

namespace RealEstate.Infrastructure.Persistence.Seeders
{
    public static class ClientSeeder
    {
        public static async Task<List<Client>> SeedAsync(
            AppDbContext context,
            List<Company> companies,
            List<User> users,
            ILogger logger)
        {
            var clients = new List<Client>();

            foreach (var company in companies)
            {
                var agent = users.First(u =>
                    u.CompanyId == company.Id &&
                    u.Role == UserRole.Agent);

                // ── Lead ──────────────────────────────────
                clients.Add(new Client
                {
                    Id = Guid.NewGuid(),
                    FullName = "عبدالرحمن خالد السبيعي",
                    Phone = $"05{new Random().Next(10000000, 99999999)}",
                    Email = $"client1.{company.Id}@test.com",
                    LeadStatus = LeadStatus.Lead,
                    Source = LeadSource.WhatsApp,
                    AssignedAgentId = agent.Id,
                    IsActive = true,
                    CompanyId = company.Id,
                    CreatedAt = DateTime.UtcNow
                });

                // ── Prospect ──────────────────────────────
                clients.Add(new Client
                {
                    Id = Guid.NewGuid(),
                    FullName = "فيصل عمر الدوسري",
                    Phone = $"05{new Random().Next(10000000, 99999999)}",
                    Email = $"client2.{company.Id}@test.com",
                    LeadStatus = LeadStatus.Prospect,
                    Source = LeadSource.Referral,
                    AssignedAgentId = agent.Id,
                    IsActive = true,
                    CompanyId = company.Id,
                    CreatedAt = DateTime.UtcNow
                });

                // ── Active ────────────────────────────────
                clients.Add(new Client
                {
                    Id = Guid.NewGuid(),
                    FullName = "نورة سعد العتيبي",
                    Phone = $"05{new Random().Next(10000000, 99999999)}",
                    Email = $"client3.{company.Id}@test.com",
                    LeadStatus = LeadStatus.Active,
                    Source = LeadSource.Website,
                    AssignedAgentId = agent.Id,
                    IsActive = true,
                    CompanyId = company.Id,
                    CreatedAt = DateTime.UtcNow
                });

                // ── Inactive ──────────────────────────────
                clients.Add(new Client
                {
                    Id = Guid.NewGuid(),
                    FullName = "تركي ناصر الحربي",
                    Phone = $"05{new Random().Next(10000000, 99999999)}",
                    Email = $"client4.{company.Id}@test.com",
                    LeadStatus = LeadStatus.Inactive,
                    Source = LeadSource.Call,
                    AssignedAgentId = agent.Id,
                    IsActive = false,
                    CompanyId = company.Id,
                    CreatedAt = DateTime.UtcNow
                });
            }

            await context.Clients.AddRangeAsync(clients);
            await context.SaveChangesAsync();

            logger.LogInformation("Seeded {Count} clients", clients.Count);
            return clients;
        }
    }
}