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
using RealEstate.Shared.Common;

namespace RealEstate.API.Controllers
{
    /// <summary>
    /// إدارة طلبات الصيانة
    /// </summary>
    [ApiController]
    [Route("api/maintenance")]
    [Authorize(Policy = Policies.AgentAndUp)]
    [EnableRateLimiting(RateLimitPolicies.General)]
    public class MaintenanceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MaintenanceController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// الحصول على قائمة طلبات الصيانة مع الترقيم والتصفية.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
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

        /// <summary>
        /// الحصول على تفاصيل طلب صيانة محدد.
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> GetDetail(
            Guid id, CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new GetMaintenanceDetailQuery { Id = id }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// إنشاء طلب صيانة جديد.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Guid>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> Create(
            [FromBody] CreateMaintenanceRequestCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// تعيين طلب الصيانة إلى مستخدم.
        /// </summary>
        [HttpPatch("{id:guid}/assign")]
        [Authorize(Policy = Policies.AdminAndUp)]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> Assign(
            Guid id,
            [FromBody] AssignMaintenanceRequestCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// حل طلب الصيانة.
        /// </summary>
        [HttpPatch("{id:guid}/resolve")]
        [Authorize(Policy = Policies.AdminAndUp)]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> Resolve(
            Guid id,
            [FromBody] ResolveMaintenanceRequestCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// إلغاء طلب الصيانة.
        /// </summary>
        [HttpPatch("{id:guid}/cancel")]
        [Authorize(Policy = Policies.AdminAndUp)]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> Cancel(
            Guid id, CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new CancelMaintenanceRequestCommand { Id = id }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// رفع ملف مرفق لطلب الصيانة.
        /// </summary>
        [HttpPost("{id:guid}/media")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> UploadMedia(
            Guid id,
            [FromForm] UploadMaintenanceMediaCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// حذف ملف مرفق من طلب الصيانة.
        /// </summary>
        [HttpDelete("media/{mediaId:guid}")]
        [Authorize(Policy = Policies.AdminAndUp)]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> DeleteMedia(
            Guid mediaId, CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new DeleteMaintenanceMediaCommand { MediaId = mediaId }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}