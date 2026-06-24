using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Appointments.Queries.GetAppointmentById
{
    public record GetAppointmentByIdQuery(Guid Id)
    : IRequest<ApiResponse<AppointmentDetailsDto>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }

        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }

}

public record AppointmentDetailsDto(
      Guid Id,
      Guid PropertyId,
      string PropertyTitle,
      string PropertyCity,
      Guid ClientId,
      string ClientName,
      string ClientPhone,
      Guid AgentId,
      string AgentName,
      DateTime ScheduledAt,
      AppointmentStatus Status,
      string? Notes,
      string? Feedback
  );
