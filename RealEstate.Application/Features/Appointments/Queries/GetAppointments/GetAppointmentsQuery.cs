using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.Appointments.Queries.GetAppointments
{
    public record GetAppointmentsQuery
        : IRequest<ApiResponse<List<AppointmentListItemDto>>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }

        public bool TodayOnly { get; init; } = false;
        public AppointmentStatus? Status { get; init; }
        public Guid? PropertyId { get; init; }
        public Guid? AgentId { get; init; }

        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}

//public record AppointmentDto(
//    Guid Id,
//    string PropertyTitle,
//    string PropertyCity,
//    string ClientName,
//    string ClientPhone,
//    string AgentName,
//    DateTime ScheduledAt,
//    AppointmentStatus Status,
//    string? Notes,
//    string? Feedback
//);
public record AppointmentListItemDto(
    Guid Id,
    string PropertyTitle,
    string PropertyCity,
    string ClientName,
    string ClientPhone,
    string AgentName,
    DateTime ScheduledAt,
    AppointmentStatus Status,
    string? Notes,
    string? Feedback
);
