
using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Domain.ReadModels;
using RealEstate.Infrastructure.Persistence;

namespace RealEstate.Infrastructure.Repositories
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<AppointmentListItem>> GetByCompanyAsync(Guid companyId) =>
            await _dbSet
                .AsNoTracking()
                .Where(x => x.Property.CompanyId == companyId)
                .OrderByDescending(x => x.ScheduledAt)
                .Select(x => new AppointmentListItem
                {
                    Id = x.Id,
                    PropertyTitle = x.Property.Title,
                    PropertyCity = x.Property.City,
                    ClientName = x.Client.FullName,
                    ClientPhone = x.Client.Phone,
                    AgentName = x.Agent.FullName,
                    ScheduledAt = x.ScheduledAt,
                    Status = x.Status,
                    Notes = x.Notes,
                    Feedback = x.Feedback
                })
                .ToListAsync();

        public async Task<IEnumerable<AppointmentListItem>> GetByAgentAsync(Guid agentId) =>
            await _dbSet
                .AsNoTracking()
                .Where(x => x.AgentId == agentId)
                .OrderByDescending(x => x.ScheduledAt)
                .Select(x => new AppointmentListItem
                {
                    Id = x.Id,
                    PropertyTitle = x.Property.Title,
                    PropertyCity = x.Property.City,
                    ClientName = x.Client.FullName,
                    ClientPhone = x.Client.Phone,
                    AgentName = x.Agent.FullName,
                    ScheduledAt = x.ScheduledAt,
                    Status = x.Status,
                    Notes = x.Notes,
                    Feedback = x.Feedback
                })
                .ToListAsync();

        public async Task<IEnumerable<AppointmentListItem>> GetTodayAsync(Guid companyId)
        {
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            return await _dbSet
                .AsNoTracking()
                .Where(x =>
                    x.Property.CompanyId == companyId &&
                    x.ScheduledAt >= today &&
                    x.ScheduledAt < tomorrow)
                .OrderBy(x => x.ScheduledAt)
                .Select(x => new AppointmentListItem
                {
                    Id = x.Id,
                    PropertyTitle = x.Property.Title,
                    PropertyCity = x.Property.City,
                    ClientName = x.Client.FullName,
                    ClientPhone = x.Client.Phone,
                    AgentName = x.Agent.FullName,
                    ScheduledAt = x.ScheduledAt,
                    Status = x.Status,
                    Notes = x.Notes,
                    Feedback = x.Feedback
                })
                .ToListAsync();
        }

        public async Task<bool> HasConflictAsync(Guid agentId, DateTime scheduledAt)
        {
            var from = scheduledAt.AddMinutes(-30);
            var to = scheduledAt.AddMinutes(30);

            return await _dbSet
                .AsNoTracking()
                .AnyAsync(x =>
                    x.AgentId == agentId &&
                    x.Status != Domain.Common.Enums.AppointmentStatus.Cancelled &&
                    x.ScheduledAt >= from &&
                    x.ScheduledAt <= to);


        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _dbSet.AnyAsync(x => x.Id == id);
        }
    }
}
