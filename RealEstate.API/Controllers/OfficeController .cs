using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Features.Properties.Offices.Commands.CreateOffice;
using RealEstate.Application.Features.Properties.Offices.Commands.UpdateOffice;
using RealEstate.Application.Features.Properties.Offices.Query.GetOfficeDetailById;
using RealEstate.Application.Features.Properties.Offices.Query.GetOfficePages;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Authorization;

namespace RealEstate.API.Controllers
{
    [ApiController]
    [Route("api/offices")]
    [Authorize(Policy = Policies.AgentAndUp)]
    public class OfficeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OfficeController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] PropertyStatus? status = null,
            [FromQuery] PropertyPurpose? purpose = null,
            [FromQuery] FurnishedStatus? furnishedStatus = null,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new GetOfficePagesQuery
                {
                    Page = page,
                    PageSize = pageSize,
                    status = status,
                    purpose = purpose,
                    FurnishedStatus = furnishedStatus
                },
                ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetDetail(
            Guid id,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new GetOfficeDetailQuery
                {
                    Id = id
                },
                ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateOfficeCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateOfficeCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}