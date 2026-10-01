using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RealEstate.Application.Features.Cheques.Commands.BounceCheque;
using RealEstate.Application.Features.Cheques.Commands.CancelCheque;
using RealEstate.Application.Features.Cheques.Commands.ClearCheque;
using RealEstate.Application.Features.Cheques.Commands.CreateCheque;
using RealEstate.Application.Features.Cheques.Commands.DepositCheque;
using RealEstate.Application.Features.Cheques.Commands.UpdateCheque;
using RealEstate.Application.Features.Cheques.Queries.GetChequeDetail;
using RealEstate.Application.Features.Cheques.Queries.GetContractCheques;
using RealEstate.Application.Features.Cheques.Queries.GetPagedCheques;
using RealEstate.Shared.Authorization;

namespace RealEstate.Api.Controllers
{
    [ApiController]
    [Route("api/cheques")]
    [Authorize(Policy = Policies.AgentAndUp)]
    [EnableRateLimiting(RateLimitPolicies.General)]
    public class ChequeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChequeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ── Queries ───────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> GetPaged(
            [FromQuery] GetPagedChequesQuery query,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(query, ct);

            return result.Success
                ? Ok(result)
                : BadRequest(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetDetail(
            Guid id,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new GetChequeDetailQuery
                {
                    Id = id
                },
                ct);

            return result.Success
                ? Ok(result)
                : BadRequest(result);
        }

        [HttpGet("contract/{contractId:guid}")]
        public async Task<IActionResult> GetByContract(
            Guid contractId,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new GetContractChequesQuery
                {
                    ContractId = contractId
                },
                ct);

            return result.Success
                ? Ok(result)
                : BadRequest(result);
        }

        // ── Create / Update ───────────────────────────────

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateChequeCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);

            return result.Success
                ? Ok(result)
                : BadRequest(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateChequeCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                cmd with { Id = id },
                ct);

            return result.Success
                ? Ok(result)
                : BadRequest(result);
        }

        // ── Status transitions ────────────────────────────

        [HttpPost("{id:guid}/deposit")]
        public async Task<IActionResult> Deposit(
            Guid id,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new DepositChequeCommand
                {
                    Id = id
                },
                ct);

            return result.Success
                ? Ok(result)
                : BadRequest(result);
        }

        [HttpPost("{id:guid}/clear")]
        public async Task<IActionResult> Clear(
            Guid id,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new ClearChequeCommand
                {
                    Id = id
                },
                ct);

            return result.Success
                ? Ok(result)
                : BadRequest(result);
        }

        [HttpPost("{id:guid}/bounce")]
        public async Task<IActionResult> Bounce(
            Guid id,
            [FromBody] BounceChequeCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                cmd with { Id = id },
                ct);

            return result.Success
                ? Ok(result)
                : BadRequest(result);
        }

        [HttpPost("{id:guid}/cancel")]
        public async Task<IActionResult> Cancel(
            Guid id,
            [FromBody] CancelChequeCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                cmd with { Id = id },
                ct);

            return result.Success
                ? Ok(result)
                : BadRequest(result);
        }
    }
}