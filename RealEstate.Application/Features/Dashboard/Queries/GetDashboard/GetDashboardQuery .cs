
using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Dashboard.Queries.GetDashboard
{
    public record GetDashboardQuery : IRequest<ApiResponse<DashboardDto>>,
    IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }

    public record DashboardDto(
        // العقارات
        int TotalProperties,
        int AvailableProperties,
        int RentedProperties,
        int SoldProperties,

        // العملاء
        int TotalClients,
        int HotLeads,
        int NewLeadsThisMonth,

        // العقود
        int TotalContracts,
        int ActiveContracts,
        int ExpiringIn30Days,

        // المدفوعات
        decimal TotalRevenue,
        decimal CollectedThisMonth,
        decimal OverdueAmount,
        int OverdueCount,

        // المواعيد
        int TodayAppointments,
        int PendingAppointments,

        // أفضل الوكلاء
        List<AgentPerformanceDto> TopAgents,

        // آخر العقود
        List<RecentContractDto> RecentContracts
    );

    public record AgentPerformanceDto(
        string FullName,
        int TotalContracts,
        decimal TotalCommission
    );

    public record RecentContractDto(
        Guid Id,
        string PropertyTitle,
        string ClientName,
        decimal Amount,
        string Status,
        DateTime CreatedAt
    );
}
