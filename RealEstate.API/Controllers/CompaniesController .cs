using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RealEstate.Application.Features.Companies.Commands.UpdateCompany;
using RealEstate.Application.Features.Companies.Commands.UpdateSubscription;
using RealEstate.Application.Features.Companies.Queries.GetCompany;
using RealEstate.Shared.Authorization;

namespace RealEstate.API.Controllers
{
    [ApiController]
    [Route("api/company")]
    [Authorize(Policy = Policies.AgentAndUp)]
    [EnableRateLimiting(RateLimitPolicies.General)]
    public class CompanyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompanyController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken ct = default)
        {
            var result = await _mediator.Send(new GetCompanyQuery(), ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        [Authorize(Policy = Policies.AdminAndUp)]
        public async Task<IActionResult> Update(
            [FromBody] UpdateCompanyCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("subscription")]
        [Authorize(Policy = Policies.AdminAndUp)]
        public async Task<IActionResult> UpdateSubscription(
            [FromBody] UpdateSubscriptionCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}