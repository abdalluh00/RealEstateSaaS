using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RealEstate.Application.Features.Maintenance.Commands.AssignMaintenanceRequest;
using RealEstate.Application.Features.Maintenance.Commands.CancelMaintenanceRequest;
using RealEstate.Application.Features.Maintenance.Commands.CreateMaintenanceRequest;
using RealEstate.Application.Features.Maintenance.Commands.DeleteMaintenanceMedia;
using RealEstate.Application.Features.Maintenance.Commands.ResolveMaintenanceRequest;
using RealEstate.Application.Features.Maintenance.Commands.UploadMaintenanceMedia;
using RealEstate.Application.Features.Maintenance.Queries.GetMaintenanceDetail;
using RealEstate.Application.Features.Maintenance.Queries.GetPagedMaintenanceRequests;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Authorization;

namespace RealEstate.API.Controllers
{
    [ApiController]
    [Route("api/maintenance")]
    [Authorize(Policy = Policies.AgentAndUp)]
    [EnableRateLimiting(RateLimitPolicies.General)]
    public class MaintenanceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MaintenanceController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] MaintenanceStatus? status = null,
            [FromQuery] MaintenanceCategory? category = null,
            [FromQuery] MaintenancePriority? priority = null,
            [FromQuery] Guid? propertyId = null,
            [FromQuery] Guid? assignedToId = null,
            [FromQuery] DateTime? dateFrom = null,
            [FromQuery] DateTime? dateTo = null,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new GetPagedMaintenanceRequestsQuery
                {
                    Page = page,
                    PageSize = pageSize,
                    Status = status,
                    Category = category,
                    Priority = priority,
                    PropertyId = propertyId,
                    AssignedToId = assignedToId,
                    DateFrom = dateFrom,
                    DateTo = dateTo
                }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetDetail(
            Guid id, CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new GetMaintenanceDetailQuery { Id = id }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateMaintenanceRequestCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("{id:guid}/assign")]
        [Authorize(Policy = Policies.AdminAndUp)]
        public async Task<IActionResult> Assign(
            Guid id,
            [FromBody] AssignMaintenanceRequestCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("{id:guid}/resolve")]
        [Authorize(Policy = Policies.AdminAndUp)]
        public async Task<IActionResult> Resolve(
            Guid id,
            [FromBody] ResolveMaintenanceRequestCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("{id:guid}/cancel")]
        [Authorize(Policy = Policies.AdminAndUp)]
        public async Task<IActionResult> Cancel(
            Guid id, CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new CancelMaintenanceRequestCommand { Id = id }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id:guid}/media")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadMedia(
            Guid id,
            [FromForm] UploadMaintenanceMediaCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("media/{mediaId:guid}")]
        [Authorize(Policy = Policies.AdminAndUp)]
        public async Task<IActionResult> DeleteMedia(
            Guid mediaId, CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new DeleteMaintenanceMediaCommand { MediaId = mediaId }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}