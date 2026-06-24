
using RealEstate.Domain.ReadModels;

namespace RealEstate.Domain.Interfaces
{
    public interface IDashboardRepository
    {
        Task<DashboardStats> GetStatsAsync(Guid companyId);
    }
}
