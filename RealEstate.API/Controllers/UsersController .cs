using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RealEstate.Application.Features.Users.Commands.DeactivateUser;
using RealEstate.Application.Features.Users.Commands.InviteUser;
using RealEstate.Application.Features.Users.Commands.UpdateUser;
using RealEstate.Application.Features.Users.Queries.GetPagedUsers;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Common;

namespace RealEstate.API.Controllers
{
    /// <summary>
    /// إدارة المستخدمين
    /// </summary>
    [ApiController]
    [Route("api/users")]
    [Authorize(Policy = Policies.AdminAndUp)]
    [EnableRateLimiting(RateLimitPolicies.General)]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// الحصول على قائمة المستخدمين مع الترقيم والتصفية.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] UserRole? role = null,
            [FromQuery] bool? isActive = null,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(new GetPagedUsersQuery
            {
                Page = page,
                PageSize = pageSize,
                Role = role,
                IsActive = isActive
            }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// إرسال دعوة لإنشاء مستخدم جديد.
        /// </summary>
        [HttpPost("invite")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> Invite(
            [FromBody] InviteUserCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// تحديث بيانات مستخدم محدد.
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateUserCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// إلغاء تفعيل مستخدم محدد.
        /// </summary>
        [HttpPatch("{id:guid}/deactivate")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> Deactivate(
            Guid id, CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new DeactivateUserCommand { Id = id }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}