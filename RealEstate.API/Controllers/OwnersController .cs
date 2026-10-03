using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RealEstate.Application.Features.Owners.Commands.CreateOwner;
using RealEstate.Application.Features.Owners.Commands.DeleteOwner;
using RealEstate.Application.Features.Owners.Commands.UpdateOwner;
using RealEstate.Application.Features.Owners.Queries.GetOwnerDetail;
using RealEstate.Application.Features.Owners.Queries.GetPagedOwners;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Common;

namespace RealEstate.API.Controllers
{
    /// <summary>
    /// إدارة الملاك
    /// </summary>
    [ApiController]
    [Route("api/owners")]
    [Authorize(Policy = Policies.AgentAndUp)]
    [EnableRateLimiting(RateLimitPolicies.General)]
    public class OwnerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OwnerController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// الحصول على قائمة الملاك مع الترقيم والتصفية.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool? isActive = null,
            [FromQuery] OwnerType? ownerType = null,
            [FromQuery] string? search = null,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(new GetPagedOwnersQuery
            {
                Page = page,
                PageSize = pageSize,
                IsActive = isActive,
                OwnerType = ownerType,
                Search = search
            }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// الحصول على تفاصيل مالك محدد.
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> GetDetail(
            Guid id, CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new GetOwnerDetailQuery { Id = id }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// إنشاء مالك جديد.
        /// </summary>
        [HttpPost]
        [Authorize(Policy = Policies.AdminAndUp)]
        [ProducesResponseType(typeof(ApiResponse<Guid>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> Create(
            [FromBody] CreateOwnerCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// تحديث بيانات مالك محدد.
        /// </summary>
        [HttpPut("{id:guid}")]
        [Authorize(Policy = Policies.AdminAndUp)]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateOwnerCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// حذف مالك محدد.
        /// </summary>
        [HttpDelete("{id:guid}")]
        [Authorize(Policy = Policies.AdminAndUp)]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> Delete(
            Guid id, CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new DeleteOwnerCommand { Id = id }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}