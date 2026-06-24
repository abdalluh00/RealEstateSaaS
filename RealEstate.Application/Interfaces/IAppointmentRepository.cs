using RealEstate.Domain.Entities;
using RealEstate.Domain.ReadModels;

namespace RealEstate.Domain.Interfaces
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<IEnumerable<AppointmentListItem>> GetByCompanyAsync(Guid companyId);
        Task<IEnumerable<AppointmentListItem>> GetByAgentAsync(Guid agentId);
        Task<IEnumerable<AppointmentListItem>> GetTodayAsync(Guid companyId);
        Task<bool> HasConflictAsync(Guid agentId, DateTime scheduledAt);
        Task<bool> ExistsAsync(Guid id);
    }
}
