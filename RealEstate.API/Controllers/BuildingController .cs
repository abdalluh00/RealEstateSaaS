using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RealEstate.Application.DTOs.Properties.Base;
using RealEstate.Application.DTOs.Properties.Building;
using RealEstate.Application.Features.Buildings.Commands.CreateBuilding;
using RealEstate.Application.Features.Buildings.Commands.DeleteBuilding;
using RealEstate.Application.Features.Buildings.Commands.UpdateBuilding;
using RealEstate.Application.Features.Buildings.Queries.GetBuildingDetail;
using RealEstate.Application.Features.Buildings.Queries.GetBuildingUnits;
using RealEstate.Application.Features.Buildings.Queries.GetPagedBuildings;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Common;

namespace RealEstate.API.Controllers
{/// <summary>
/// إدارة العمارات
/// </summary>
    [ApiController]
    [Route("api/buildings")]
    [Authorize(Policy = Policies.AgentAndUp)]
    [EnableRateLimiting(RateLimitPolicies.General)]
    public class BuildingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BuildingController(IMediator mediator) => _mediator = mediator;
        /// <summary>
        /// الحصول على قائمة العمارات مع ترقيم الصفحات
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<BuildingListDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 401)]
    public async Task<IActionResult> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] PropertyStatus? status = null,
            [FromQuery] PropertyPurpose? purpose = null,
            [FromQuery] bool? hasElevator = null,
            [FromQuery] int? minFloors = null,
            [FromQuery] int? maxFloors = null,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(new GetPagedBuildingsQuery
            {
                Page = page,
                PageSize = pageSize,
                Status = status,
                Purpose = purpose,
                HasElevator = hasElevator,
                MinFloors = minFloors,
                MaxFloors = maxFloors
            }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }
        /// <summary>
        /// الحصول على تفاصيل العمارة محدد
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<BuildingDetailDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<IActionResult> GetDetail(
            Guid id, CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new GetBuildingDetailQuery { Id = id }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }
        /// <summary>
        /// الحصول على وحدات العمارة
        /// </summary>
        /// <param name="id">معرف العمارة</param>
        [HttpGet("{id:guid}/units")]
        [ProducesResponseType(typeof(ApiResponse<List<PropertyListDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> GetUnits(
            Guid id, CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new GetBuildingUnitsQuery { BuildingId = id }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }
        /// <summary>
        /// إنشاء عمارة جديد
        /// </summary>
        [HttpPost]
        [Authorize(Policy = Policies.AdminAndUp)]
        [ProducesResponseType(typeof(ApiResponse<Guid>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> Create(
            [FromBody] CreateBuildingCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        /// <summary>
        /// تحديث العمارة}
        /// </summary>
        [HttpPut("{id:guid}")]
        [Authorize(Policy = Policies.AdminAndUp)]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateBuildingCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        /// <summary>
        /// حذف العمارة
        /// </summary>
        [HttpDelete("{id:guid}")]
        [Authorize(Policy = Policies.AdminAndUp)]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> Delete(
            Guid id, CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new DeleteBuildingCommand { Id = id }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}