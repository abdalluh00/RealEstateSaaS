using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RealEstate.Application.Features.Appointments.Commands.CancelAppointment;
using RealEstate.Application.Features.Appointments.Commands.CompleteAppointment;
using RealEstate.Application.Features.Appointments.Commands.ConfirmAppointment;
using RealEstate.Application.Features.Appointments.Commands.CreateAppointment;
using RealEstate.Application.Features.Appointments.Commands.MarkNoShow;
using RealEstate.Application.Features.Appointments.Queries.GetAppointmentDetail;
using RealEstate.Application.Features.Appointments.Queries.GetPagedAppointments;
using RealEstate.Application.Features.Appointments.Queries.GetTodayAppointments;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Authorization;

namespace RealEstate.API.Controllers
{
    [ApiController]
    [Route("api/appointments")]
    [Authorize(Policy = Policies.AgentAndUp)]
    [EnableRateLimiting(RateLimitPolicies.General)]
    public class AppointmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AppointmentController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] Guid? agentId = null,
            [FromQuery] Guid? clientId = null,
            [FromQuery] Guid? propertyId = null,
            [FromQuery] AppointmentStatus? status = null,
            [FromQuery] DateTime? from = null,
            [FromQuery] DateTime? to = null,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(new GetPagedAppointmentsQuery
            {
                Page = page,
                PageSize = pageSize,
                AgentId = agentId,
                ClientId = clientId,
                PropertyId = propertyId,
                Status = status,
                From = from,
                To = to
            }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("today")]
        public async Task<IActionResult> GetToday(
            [FromQuery] Guid? agentId = null,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new GetTodayAppointmentsQuery { AgentId = agentId }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetDetail(
            Guid id, CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new GetAppointmentDetailQuery { Id = id }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateAppointmentCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("{id:guid}/confirm")]
        public async Task<IActionResult> Confirm(
            Guid id, CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new ConfirmAppointmentCommand { Id = id }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("{id:guid}/cancel")]
        public async Task<IActionResult> Cancel(
            Guid id,
            [FromBody] CancelAppointmentCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("{id:guid}/complete")]
        public async Task<IActionResult> Complete(
            Guid id,
            [FromBody] CompleteAppointmentCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("{id:guid}/no-show")]
        public async Task<IActionResult> NoShow(
            Guid id, CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new MarkNoShowCommand { Id = id }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}