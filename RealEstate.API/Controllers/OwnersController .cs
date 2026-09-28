using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Features.Owners.Commands.CreateOwner;
using RealEstate.Application.Features.Owners.Commands.DeleteOwner;
using RealEstate.Application.Features.Owners.Commands.UpdateOwner;
using RealEstate.Application.Features.Owners.Queries.GetOwnerDetail;
using RealEstate.Application.Features.Owners.Queries.GetPagedOwners;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Authorization;

namespace RealEstate.API.Controllers
{
    [ApiController]
    [Route("api/owners")]
    [Authorize(Policy = Policies.AgentAndUp)]
    public class OwnerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OwnerController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
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

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetDetail(
            Guid id, CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new GetOwnerDetailQuery { Id = id }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        [Authorize(Policy = Policies.AdminAndUp)]
        public async Task<IActionResult> Create(
            [FromBody] CreateOwnerCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Policy = Policies.AdminAndUp)]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateOwnerCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = Policies.AdminAndUp)]
        public async Task<IActionResult> Delete(
            Guid id, CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new DeleteOwnerCommand { Id = id }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}