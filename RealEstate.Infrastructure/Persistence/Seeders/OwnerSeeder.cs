using Microsoft.Extensions.Logging;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Persistence;

namespace RealEstate.Infrastructure.Persistence.Seeders
{
    public static class OwnerSeeder
    {
        public static async Task<List<Owner>> SeedAsync(
            AppDbContext context,
            List<Company> companies,
            ILogger logger)
        {
            var owners = new List<Owner>();

            foreach (var company in companies)
            {
                // ── Individual Owner ──────────────────────
                owners.Add(new Owner
                {
                    Id = Guid.NewGuid(),
                    FullName = "محمد عبدالله الغامدي",
                    Phone = $"05{new Random().Next(10000000, 99999999)}",
                    Email = $"owner1.{company.Id}@test.com",
                    NationalId = $"10{new Random().Next(10000000, 99999999)}",
                    Nationality = "سعودي",
                    OwnerType = OwnerType.Individual,
                    IBAN = "SA0380000000608010167519",
                    CommissionRate = 5,
                    IsActive = true,
                    CompanyId = company.Id,
                    CreatedAt = DateTime.UtcNow
                });

                // ── Company Owner ─────────────────────────
                owners.Add(new Owner
                {
                    Id = Guid.NewGuid(),
                    FullName = "سعد محمد الشهري",
                    Phone = $"05{new Random().Next(10000000, 99999999)}",
                    Email = $"owner2.{company.Id}@test.com",
                    NationalId = $"10{new Random().Next(10000000, 99999999)}",
                    Nationality = "سعودي",
                    OwnerType = OwnerType.Company,
                    CompanyName = "مجموعة الشهري للاستثمار",
                    IBAN = "SA4420000001234567891234",
                    CommissionRate = 3,
                    IsActive = true,
                    CompanyId = company.Id,
                    CreatedAt = DateTime.UtcNow
                });
            }

            await context.Owners.AddRangeAsync(owners);
            await context.SaveChangesAsync();

            logger.LogInformation("Seeded {Count} owners", owners.Count);
            return owners;
        }
    }
}