using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Features.Payments.Commands.CancelPayment;
using RealEstate.Application.Features.Payments.Commands.MarkPaymentPaid;
using RealEstate.Application.Features.Payments.Queries.GetContractPaymentSummary;
using RealEstate.Application.Features.Payments.Queries.GetContractPayments;
using RealEstate.Shared.Authorization;

namespace RealEstate.API.Controllers
{
    [ApiController]
    [Route("api/payments")]
    [Authorize(Policy = Policies.AgentAndUp)]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentController(IMediator mediator) => _mediator = mediator;

        [HttpGet("contract/{contractId:guid}")]
        public async Task<IActionResult> GetByContract(
            Guid contractId,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new GetContractPaymentsQuery { ContractId = contractId }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("contract/{contractId:guid}/summary")]
        public async Task<IActionResult> GetSummary(
            Guid contractId,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new GetContractPaymentSummaryQuery { ContractId = contractId }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("{id:guid}/paid")]
        [Authorize(Policy = Policies.AdminAndUp)]
        public async Task<IActionResult> MarkPaid(
            Guid id,
            [FromBody] MarkPaymentPaidCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("{id:guid}/cancel")]
        [Authorize(Policy = Policies.AdminAndUp)]
        public async Task<IActionResult> Cancel(
            Guid id,
            [FromBody] CancelPaymentCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}