using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealEstate.Infrastructure.Persistence;
using RealEstate.Infrastructure.Persistence.Seeders;

namespace RealEstate.Infrastructure.Persistence.Seeders
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(
            AppDbContext context,
            ILogger logger)
        {
            try
            {
                // ── Only seed if DB is empty ───────────────
                if (await context.Companies.AnyAsync())
                {
                    logger.LogInformation("Database already seeded — skipping");
                    return;
                }

                logger.LogInformation("Starting database seeding...");

                // ── Order matters — FK dependencies ───────
                var companies = await CompanySeeder.SeedAsync(context, logger);
                var users = await UserSeeder.SeedAsync(context, companies, logger);
                var owners = await OwnerSeeder.SeedAsync(context, companies, logger);
                var clients = await ClientSeeder.SeedAsync(context, companies, users, logger);
                var properties = await PropertySeeder.SeedAsync(context, companies, owners, users, logger);
                var contracts = await ContractSeeder.SeedAsync(context, companies, properties, clients, users, logger);
                await AppointmentSeeder.SeedAsync(context, companies, properties, clients, users, logger);
                await MaintenanceSeeder.SeedAsync(context, companies, properties, clients, users, logger);

                logger.LogInformation("Database seeding completed successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error seeding database");
                throw;
            }
        }
    }
}