using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Features.Lands.Commands.CreateLand;
using RealEstate.Application.Features.Lands.Commands.DeleteLand;
using RealEstate.Application.Features.Lands.Commands.UpdateLand;
using RealEstate.Application.Features.Lands.Queries.GetLandDetail;
using RealEstate.Application.Features.Lands.Queries.GetPagedLands;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Authorization;

namespace RealEstate.API.Controllers
{
    [ApiController]
    [Route("api/lands")]
    [Authorize(Policy = Policies.AgentAndUp)]
    public class LandController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LandController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] PropertyStatus? status = null,
            [FromQuery] PropertyPurpose? purpose = null,
            [FromQuery] ZoningType? zoningType = null,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(new GetPagedLandsQuery
            {
                Page = page,
                PageSize = pageSize,
                Status = status,
                Purpose = purpose,
                ZoningType = zoningType
            }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetDetail(
            Guid id, CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new GetLandDetailQuery { Id = id }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        [Authorize(Policy = Policies.AdminAndUp)]
        public async Task<IActionResult> Create(
            [FromBody] CreateLandCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Policy = Policies.AdminAndUp)]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateLandCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd , ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = Policies.AdminAndUp)]
        public async Task<IActionResult> Delete(
            Guid id, CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new DeleteLandCommand { Id = id }, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}