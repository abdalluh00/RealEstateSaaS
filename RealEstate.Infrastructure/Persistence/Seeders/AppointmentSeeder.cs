using Microsoft.Extensions.Logging;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Infrastructure.Persistence;

namespace RealEstate.Infrastructure.Persistence.Seeders
{
    public static class AppointmentSeeder
    {
        public static async Task SeedAsync(
            AppDbContext context,
            List<Company> companies,
            List<Property> properties,
            List<Client> clients,
            List<User> users,
            ILogger logger)
        {
            var appointments = new List<Appointment>();

            foreach (var company in companies)
            {
                var office = properties.OfType<OfficeProperty>()
                    .First(p => p.CompanyId == company.Id);

                var leadClient = clients.First(c =>
                    c.CompanyId == company.Id &&
                    c.LeadStatus == LeadStatus.Lead);

                var prospectClient = clients.First(c =>
                    c.CompanyId == company.Id &&
                    c.LeadStatus == LeadStatus.Prospect);

                var agent = users.First(u =>
                    u.CompanyId == company.Id &&
                    u.Role == UserRole.Agent);

                // ── Pending ───────────────────────────────
                appointments.Add(new Appointment
                {
                    Id = Guid.NewGuid(),
                    ScheduledAt = DateTime.UtcNow.AddDays(2),
                    DurationMinutes = 60,
                    Status = AppointmentStatus.Pending,
                    Notes = "زيارة أولى لمشاهدة المكتب",
                    PropertyId = office.Id,
                    ClientId = leadClient.Id,
                    AgentId = agent.Id,
                    CompanyId = company.Id,
                    CreatedAt = DateTime.UtcNow
                });

                // ── Confirmed ─────────────────────────────
                appointments.Add(new Appointment
                {
                    Id = Guid.NewGuid(),
                    ScheduledAt = DateTime.UtcNow.AddDays(1),
                    DurationMinutes = 45,
                    Status = AppointmentStatus.Confirmed,
                    Notes = "موعد مؤكد لمشاهدة المكتب",
                    PropertyId = office.Id,
                    ClientId = prospectClient.Id,
                    AgentId = agent.Id,
                    CompanyId = company.Id,
                    CreatedAt = DateTime.UtcNow
                });

                // ── Done ──────────────────────────────────
                appointments.Add(new Appointment
                {
                    Id = Guid.NewGuid(),
                    ScheduledAt = DateTime.UtcNow.AddDays(-3),
                    ActualVisitAt = DateTime.UtcNow.AddDays(-3),
                    DurationMinutes = 60,
                    Status = AppointmentStatus.Done,
                    Result = AppointmentResult.Interested,
                    Feedback = "العميل معجب بالمكتب وسيعود للتفاوض",
                    PropertyId = office.Id,
                    ClientId = leadClient.Id,
                    AgentId = agent.Id,
                    CompanyId = company.Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-4)
                });

                // ── Cancelled ─────────────────────────────
                appointments.Add(new Appointment
                {
                    Id = Guid.NewGuid(),
                    ScheduledAt = DateTime.UtcNow.AddDays(-1),
                    DurationMinutes = 60,
                    Status = AppointmentStatus.Cancelled,
                    CancellationReason = "العميل اعتذر بسبب ارتباط مفاجئ",
                    PropertyId = office.Id,
                    ClientId = prospectClient.Id,
                    AgentId = agent.Id,
                    CompanyId = company.Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                });

                // ── NoShow ────────────────────────────────
                appointments.Add(new Appointment
                {
                    Id = Guid.NewGuid(),
                    ScheduledAt = DateTime.UtcNow.AddDays(-5),
                    DurationMinutes = 30,
                    Status = AppointmentStatus.NoShow,
                    Notes = "العميل لم يحضر ولم يتواصل",
                    PropertyId = office.Id,
                    ClientId = leadClient.Id,
                    AgentId = agent.Id,
                    CompanyId = company.Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-6)
                });
            }

            await context.Appointments.AddRangeAsync(appointments);
            await context.SaveChangesAsync();

            logger.LogInformation("Seeded {Count} appointments", appointments.Count);
        }
    }
}