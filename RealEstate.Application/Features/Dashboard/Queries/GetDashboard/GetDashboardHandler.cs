
using MediatR;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Dashboard.Queries.GetDashboard
{
    public class GetDashboardHandler
    : IRequestHandler<GetDashboardQuery, ApiResponse<DashboardDto>>
    {
        private readonly IDashboardRepository _repo;

        public GetDashboardHandler(IDashboardRepository repo) => _repo = repo;

        public async Task<ApiResponse<DashboardDto>> Handle(
            GetDashboardQuery request,
            CancellationToken ct)
        {
            var stats = await _repo.GetStatsAsync(request.CompanyId);

            var result = new DashboardDto(
                stats.TotalProperties,
                stats.AvailableProperties,
                stats.RentedProperties,
                stats.SoldProperties,
                stats.TotalClients,
                stats.HotLeads,
                stats.NewLeadsThisMonth,
                stats.TotalContracts,
                stats.ActiveContracts,
                stats.ExpiringIn30Days,
                stats.TotalRevenue,
                stats.CollectedThisMonth,
                stats.OverdueAmount,
                stats.OverdueCount,
                stats.TodayAppointments,
                stats.PendingAppointments,
                stats.TopAgents.Select(a => new AgentPerformanceDto(
                    a.FullName,
                    a.TotalContracts,
                    a.TotalCommission
                )).ToList(),
                stats.RecentContracts.Select(c => new RecentContractDto(
                    c.Id,
                    c.PropertyTitle,
                    c.ClientName,
                    c.Amount,
                    c.Status,
                    c.CreatedAt
                )).ToList()
            );

            return ApiResponse<DashboardDto>.Ok(result);
        }
    }
}
