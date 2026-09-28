using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.DTOs.Appointments;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Appointments.Queries.GetPagedAppointments
{
    public sealed class GetPagedAppointmentsQuery
        : IRequest<ApiResponse<PagedResult<AppointmentListDto>>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public Guid? AgentId { get; init; }
        public Guid? ClientId { get; init; }
        public Guid? PropertyId { get; init; }
        public AppointmentStatus? Status { get; init; }
        public DateTime? From { get; init; }
        public DateTime? To { get; init; }
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}