using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RealEstate.Application.Features.Clients.Commands.CreateClient;
using RealEstate.Application.Features.Clients.Commands.DeleteClient;
using RealEstate.Application.Features.Clients.Commands.ReassignAgent;
using RealEstate.Application.Features.Clients.Commands.UpdateClient;
using RealEstate.Application.Features.Clients.Commands.UpdateLeadStatus;
using RealEstate.Application.Features.Clients.Queries.GetClientDetail;
using RealEstate.Application.Features.Clients.Queries.GetPagedClients;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Common;

namespace RealEstate.API.Controllers
{
    /// <summary>
    /// إدارة العملاء
    /// </summary>
    [ApiController]
    [Route("api/clients")]
    [Authorize(Policy = Policies.AgentAndUp)]
    [EnableRateLimiting(RateLimitPolicies.General)]
    public class ClientController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClientController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// الحصول على قائمة العملاء مع الترقيم والتصفية.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] LeadStatus? leadStatus = null,
            [FromQuery] LeadSource? source = null,
            [FromQuery] bool? isActive = null,
            [FromQuery] Guid? assignedAgentId = null,
            [FromQuery] string? search = null,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(new GetPagedClientsQuery
            {
                Page = page,
                PageSize = pageSize,
                LeadStatus = leadStatus,
                Source = source,
                IsActive = isActive,
                AssignedAgentId = assignedAgentId,
                Search = search
            }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// الحصول على تفاصيل عميل محدد.
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> GetDetail(
            Guid id,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new GetClientDetailQuery { Id = id }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// إنشاء عميل جديد.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Guid>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> Create(
            [FromBody] CreateClientCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// تحديث بيانات عميل محدد.
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateClientCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// تحديث حالة العميل المحتمل.
        /// </summary>
        [HttpPatch("{id:guid}/lead-status")]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> UpdateLeadStatus(
            Guid id,
            [FromBody] UpdateLeadStatusCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// إعادة تعيين العميل إلى وكيل آخر.
        /// </summary>
        [HttpPatch("{id:guid}/reassign")]
        [Authorize(Policy = Policies.AdminAndUp)]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> ReassignAgent(
            Guid id,
            [FromBody] ReassignAgentCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// حذف عميل محدد.
        /// </summary>
        [HttpDelete("{id:guid}")]
        [Authorize(Policy = Policies.AdminAndUp)]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new DeleteClientCommand { Id = id }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}