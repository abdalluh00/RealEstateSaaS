using Microsoft.Extensions.Logging;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Infrastructure.Persistence;

namespace RealEstate.Infrastructure.Persistence.Seeders
{
    public static class ContractSeeder
    {
        public static async Task<List<Contract>> SeedAsync(
            AppDbContext context,
            List<Company> companies,
            List<Property> properties,
            List<Client> clients,
            List<User> users,
            ILogger logger)
        {
            var contracts = new List<Contract>();
            var payments = new List<Payment>();
            var counter = 1;

            foreach (var company in companies)
            {
                var apartment = properties.OfType<ApartmentProperty>()
                    .First(p => p.CompanyId == company.Id);

                var villa = properties.OfType<VillaProperty>()
                    .First(p => p.CompanyId == company.Id);

                var activeClient = clients.First(c =>
                    c.CompanyId == company.Id &&
                    c.LeadStatus == LeadStatus.Active);

                var agent = users.First(u =>
                    u.CompanyId == company.Id &&
                    u.Role == UserRole.Agent);

                // ── Rent Contract (Apartment) ──────────────
                var rentContract = new Contract
                {
                    Id = Guid.NewGuid(),
                    ContractNumber = $"CONT-{DateTime.UtcNow.Year}-{counter++:D4}",
                    ContractType = ContractType.Rent,
                    ContractStatus = ContractStatus.Active,
                    Amount = 2500,
                    SecurityDeposit = 5000,
                    PaymentCycle = PaymentCycle.Monthly,
                    PaymentMethod = PaymentMethod.BankTransfer,
                    CommissionType = CommissionType.Percentage,
                    Commission = 5,
                    CommissionStatus = CommissionStatus.Pending,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddYears(1),
                    Notes = "عقد إيجار سنوي مع دفع شهري",
                    PropertyId = apartment.Id,
                    ClientId = activeClient.Id,
                    AgentId = agent.Id,
                    CompanyId = company.Id,
                    CreatedAt = DateTime.UtcNow
                };

                contracts.Add(rentContract);

                // ── Auto-generate monthly payments ────────
                for (int i = 0; i < 12; i++)
                {
                    payments.Add(new Payment
                    {
                        Id = Guid.NewGuid(),
                        PaymentNumber = i + 1,
                        Amount = 2500,
                        DueDate = DateTime.UtcNow.AddMonths(i),
                        PaymentStatus = i == 0
                            ? PaymentStatus.Paid    // first payment paid
                            : i < 3
                                ? PaymentStatus.Overdue  // simulate overdue
                                : PaymentStatus.Pending,
                        PaidDate = i == 0 ? DateTime.UtcNow : null,
                        PaymentMethod = i == 0 ? PaymentMethod.BankTransfer : PaymentMethod.Cash,
                        ContractId = rentContract.Id,
                        CompanyId = company.Id,
                        CreatedAt = DateTime.UtcNow
                    });
                }

                // ── Update apartment status ───────────────
                apartment.PropertyStatus = PropertyStatus.Rented;

                // ── Sale Contract (Villa) ─────────────────
                var saleContract = new Contract
                {
                    Id = Guid.NewGuid(),
                    ContractNumber = $"CONT-{DateTime.UtcNow.Year}-{counter++:D4}",
                    ContractType = ContractType.Sale,
                    ContractStatus = ContractStatus.Active,
                    Amount = 3500000,
                    CommissionType = CommissionType.Fixed,
                    Commission = 50000,
                    CommissionStatus = CommissionStatus.Paid,
                    StartDate = DateTime.UtcNow,
                    Notes = "عقد بيع نهائي",
                    PropertyId = villa.Id,
                    ClientId = activeClient.Id,
                    AgentId = agent.Id,
                    CompanyId = company.Id,
                    CreatedAt = DateTime.UtcNow
                };

                contracts.Add(saleContract);

                // ── Update villa status ───────────────────
                villa.PropertyStatus = PropertyStatus.Sold;
            }

            await context.Contracts.AddRangeAsync(contracts);
            await context.Payments.AddRangeAsync(payments);
            await context.SaveChangesAsync();

            logger.LogInformation(
                "Seeded {Contracts} contracts and {Payments} payments",
                contracts.Count, payments.Count);

            return contracts;
        }
    }
}