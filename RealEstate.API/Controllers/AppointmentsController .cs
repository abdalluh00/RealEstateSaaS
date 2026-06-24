using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Features.Appointments.Commands.CreateAppointment;
using RealEstate.Application.Features.Appointments.Commands.DeleteAppointment;
using RealEstate.Application.Features.Appointments.Commands.UpdateAppointmentStatus;
using RealEstate.Application.Features.Appointments.Queries.GetAppointments;
using RealEstate.Shared.Authorization;

namespace RealEstate.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AppointmentsController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        [Authorize(Policy = Policies.AgentAndUp)]
        public async Task<IActionResult> GetAll(
            
            [FromQuery] bool todayOnly = false)
        {
            var result = await _mediator.Send(
                new GetAppointmentsQuery(todayOnly));
            return Ok(result);
        }
        // Admin و Owner فقط
        [Authorize(Policy = Policies.AdminAndUp)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // Admin و Owner فقط
        [Authorize(Policy = Policies.AdminAndUp)]
        [HttpPut("{id:guid}/status")]
        public async Task<IActionResult> UpdateStatus(
            Guid id,
            [FromBody] UpdateAppointmentStatusCommand command)
        {
            if (id != command.Id) return BadRequest("المعرف غير متطابق");
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }


        // Owner فقط
        [Authorize(Policy = Policies.OwnerOnly)]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteAppointmentCommand(id));
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
