using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Features.Properties.Queries.GetDashboardStats;
using RealEstate.Application.Features.Properties.Queries.GetFeaturedProperties;
using RealEstate.Application.Features.Properties.Queries.GetPagedProperties;
using RealEstate.Application.Features.Properties.Queries.GetPropertiesByAgent;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Authorization;

[ApiController]
[Route("api/properties")]
[Authorize(Policy = Policies.AgentAndUp)]
public class PropertyController : ControllerBase
{
    private readonly IMediator _mediator;

    public PropertyController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] PropertyStatus? status = null,
        [FromQuery] PropertyPurpose? purpose = null,
        [FromQuery] string? city = null,
        CancellationToken ct = default)
    {
        var query = new GetPagedPropertiesQuery
        {
            Page = page,
            PageSize = pageSize,
            Status = status,
            Purpose = purpose,
            City = city
        };

        var result = await _mediator.Send(query, ct);

        return result.Success
            ? Ok(result)
            : BadRequest(result);
    }
    [HttpGet("featured")]
    public async Task<IActionResult> GetFeatured(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10,
    CancellationToken ct = default)
    {
        var query = new GetFeaturedPropertiesQuery { Page = page, PageSize = pageSize };
        var result = await _mediator.Send(query, ct);
        return result.Success ? Ok(result) : BadRequest(result);
    }
    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboardStats(
    [FromQuery] int featuredLimit = 6,
    CancellationToken ct = default)
    {
        var query = new GetDashboardStatsQuery { FeaturedLimit = featuredLimit };
        var result = await _mediator.Send(query, ct);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("agent/{agentId:guid}")]
    public async Task<IActionResult> GetByAgent(
    Guid agentId,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10,
    CancellationToken ct = default)
    {
        var query = new GetPropertiesByAgentQuery
        {
            AgentId = agentId,
            Page = page,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query, ct);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}