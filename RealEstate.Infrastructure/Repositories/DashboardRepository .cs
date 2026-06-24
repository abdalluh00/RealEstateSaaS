using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Interfaces;
using RealEstate.Domain.ReadModels;
using RealEstate.Infrastructure.Persistence;

namespace RealEstate.Infrastructure.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly AppDbContext _context;

        public DashboardRepository(AppDbContext context) =>
            _context = context;

        public async Task<DashboardStats> GetStatsAsync(Guid companyId)
        {
            var now = DateTime.UtcNow;
            var today = now.Date;
            var tomorrow = today.AddDays(1);
            var thisMonth = new DateTime(now.Year, now.Month, 1);
            var in30Days = now.AddDays(30);

            // كل الاستعلامات تشتغل بالتوازي
            var stats = new DashboardStats();

            // ── العقارات ──────────────────────────────
            //var properties = await _context.Properties
            //    .AsNoTracking()
            //    .Where(x => x.CompanyId == companyId)
            //    .GroupBy(x => x.PropertyStatus)
            //    .Select(g => new { Status = g.Key, Count = g.Count() })
            //    .ToListAsync();

            //stats.TotalProperties = properties.Sum(x => x.Count);
            //stats.AvailableProperties = properties
            //    .FirstOrDefault(x => x.PropertyStatus == "Available")?.Count ?? 0;
            //stats.RentedProperties = properties
            //    .FirstOrDefault(x => x.PropertyStatus == "Rented")?.Count ?? 0;
            //stats.SoldProperties = properties
            //    .FirstOrDefault(x => x.Status == "Sold")?.Count ?? 0;

            // ── العملاء ──────────────────────────────
            var clients = await _context.Clients
                .AsNoTracking()
                .Where(x => x.CompanyId == companyId)
                .GroupBy(x => 1)
                .Select(g => new
                {
                    Total = g.Count(),
                    Hot = g.Count(x => x.LeadStatus == "Hot"),
                    NewThisMonth = g.Count(x => x.CreatedAt >= thisMonth)
                })
                .FirstOrDefaultAsync();

            stats.TotalClients = clients?.Total ?? 0;
            stats.HotLeads = clients?.Hot ?? 0;
            stats.NewLeadsThisMonth = clients?.NewThisMonth ?? 0;

            // ── العقود ──────────────────────────────
            //var contracts = await _context.Contracts
            //    .AsNoTracking()
            //    .Where(x => x.Property.CompanyId == companyId)
            //    .GroupBy(x => 1)
            //    .Select(g => new
            //    {
            //        Total = g.Count(),
            //        Active = g.Count(x => x.Status == "Active"),
            //        Expiring = g.Count(x =>
            //            x.Status == "Active" &&
            //            x.EndDate >= now &&
            //            x.EndDate <= in30Days)
            //    })
            //    .FirstOrDefaultAsync();

            //stats.TotalContracts = contracts?.Total ?? 0;
            //stats.ActiveContracts = contracts?.Active ?? 0;
            //stats.ExpiringIn30Days = contracts?.Expiring ?? 0;

            // ── المدفوعات ──────────────────────────────
            var payments = await _context.Payments
                .AsNoTracking()
                .Where(x => x.Contract.Property.CompanyId == companyId)
                .GroupBy(x => 1)
                .Select(g => new
                {
                    TotalRevenue = g.Where(x => x.Status == "Paid")
                                          .Sum(x => x.Amount),
                    CollectedThisMonth = g.Where(x =>
                                            x.Status == "Paid" &&
                                            x.PaidDate >= thisMonth)
                                          .Sum(x => x.Amount),
                    OverdueAmount = g.Where(x =>
                                            x.Status == "Pending" &&
                                            x.DueDate < now)
                                          .Sum(x => x.Amount),
                    OverdueCount = g.Count(x =>
                                            x.Status == "Pending" &&
                                            x.DueDate < now)
                })
                .FirstOrDefaultAsync();

            stats.TotalRevenue = payments?.TotalRevenue ?? 0;
            stats.CollectedThisMonth = payments?.CollectedThisMonth ?? 0;
            stats.OverdueAmount = payments?.OverdueAmount ?? 0;
            stats.OverdueCount = payments?.OverdueCount ?? 0;

            // ── المواعيد ──────────────────────────────
            var appointments = await _context.Appointments
                .AsNoTracking()
                .Where(x => x.Property.CompanyId == companyId)
                .GroupBy(x => 1)
                .Select(g => new
                {
                    Today = g.Count(x =>
                                x.ScheduledAt >= today &&
                                x.ScheduledAt < tomorrow),
                    Pending = g.Count(x => x.Status == AppointmentStatus.Pending)
                })
                .FirstOrDefaultAsync();

            stats.TodayAppointments = appointments?.Today ?? 0;
            stats.PendingAppointments = appointments?.Pending ?? 0;

            // ── أفضل الوكلاء ──────────────────────────────
            stats.TopAgents = await _context.Contracts
                .AsNoTracking()
                .Where(x => x.Property.CompanyId == companyId)
                .GroupBy(x => new { x.AgentId, x.Agent.FullName })
                .Select(g => new AgentStats
                {
                    FullName = g.Key.FullName,
                    TotalContracts = g.Count(),
                    TotalCommission = g.Sum(x => x.Commission)
                })
                .OrderByDescending(x => x.TotalContracts)
                .Take(5)
                .ToListAsync();

            // ── آخر العقود ──────────────────────────────
            stats.RecentContracts = await _context.Contracts
                .AsNoTracking()
                .Where(x => x.Property.CompanyId == companyId)
                .OrderByDescending(x => x.CreatedAt)
                .Take(5)
                .Select(x => new RecentContractStats
                {
                    Id = x.Id,
                    PropertyTitle = x.Property.Title,
                    ClientName = x.Client.FullName,
                    Amount = x.Amount,
                    Status = x.ContractStatus.ToString(),
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();

            return stats;
        }
    }
}
