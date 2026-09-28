using Microsoft.Extensions.Logging;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Infrastructure.Persistence;

namespace RealEstate.Infrastructure.Persistence.Seeders
{
    public static class MaintenanceSeeder
    {
        public static async Task SeedAsync(
            AppDbContext context,
            List<Company> companies,
            List<Property> properties,
            List<Client> clients,
            List<User> users,
            ILogger logger)
        {
            var requests = new List<MaintenanceRequest>();
            var counter = 1;

            foreach (var company in companies)
            {
                var apartment = properties.OfType<ApartmentProperty>()
                    .First(p => p.CompanyId == company.Id);

                var activeClient = clients.First(c =>
                    c.CompanyId == company.Id &&
                    c.LeadStatus == LeadStatus.Active);

                var agent = users.First(u =>
                    u.CompanyId == company.Id &&
                    u.Role == UserRole.Agent);

                // ── Open ──────────────────────────────────
                requests.Add(new MaintenanceRequest
                {
                    Id = Guid.NewGuid(),
                    RequestNumber = $"MAINT-{DateTime.UtcNow.Year}-{counter++:D4}",
                    Title = "تسريب مياه في الحمام",
                    Description = "يوجد تسريب مياه تحت الحوض في الحمام الرئيسي",
                    Category = MaintenanceCategory.Plumbing,
                    Priority = MaintenancePriority.High,
                    Status = MaintenanceStatus.Open,
                    PropertyId = apartment.Id,
                    ClientId = activeClient.Id,
                    CompanyId = company.Id,
                    CreatedAt = DateTime.UtcNow
                });

                // ── InProgress ────────────────────────────
                requests.Add(new MaintenanceRequest
                {
                    Id = Guid.NewGuid(),
                    RequestNumber = $"MAINT-{DateTime.UtcNow.Year}-{counter++:D4}",
                    Title = "عطل في التكييف المركزي",
                    Description = "التكييف المركزي لا يبرد بشكل كافٍ",
                    Category = MaintenanceCategory.AC,
                    Priority = MaintenancePriority.Medium,
                    Status = MaintenanceStatus.InProgress,
                    AssignedToId = agent.Id,
                    PropertyId = apartment.Id,
                    ClientId = activeClient.Id,
                    CompanyId = company.Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                });

                // ── Done ──────────────────────────────────
                requests.Add(new MaintenanceRequest
                {
                    Id = Guid.NewGuid(),
                    RequestNumber = $"MAINT-{DateTime.UtcNow.Year}-{counter++:D4}",
                    Title = "صيانة الكهرباء",
                    Description = "انقطاع متكرر في التيار الكهربائي",
                    Category = MaintenanceCategory.Electric,
                    Priority = MaintenancePriority.Urgent,
                    Status = MaintenanceStatus.Done,
                    AssignedToId = agent.Id,
                    ResolutionNotes = "تم استبدال القاطع الكهربائي الرئيسي",
                    Cost = 850,
                    ContractorName = "شركة الكهرباء السريعة",
                    ContractorPhone = "0501234567",
                    ResolvedAt = DateTime.UtcNow.AddDays(-1),
                    PropertyId = apartment.Id,
                    ClientId = activeClient.Id,
                    CompanyId = company.Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-5)
                });

                // ── Cancelled ─────────────────────────────
                requests.Add(new MaintenanceRequest
                {
                    Id = Guid.NewGuid(),
                    RequestNumber = $"MAINT-{DateTime.UtcNow.Year}-{counter++:D4}",
                    Title = "دهان الجدران",
                    Description = "طلب دهان الجدران من قبل المستأجر",
                    Category = MaintenanceCategory.Painting,
                    Priority = MaintenancePriority.Low,
                    Status = MaintenanceStatus.Cancelled,
                    PropertyId = apartment.Id,
                    ClientId = activeClient.Id,
                    CompanyId = company.Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-7)
                });
            }

            await context.MaintenanceRequests.AddRangeAsync(requests);
            await context.SaveChangesAsync();

            logger.LogInformation("Seeded {Count} maintenance requests", requests.Count);
        }
    }
}