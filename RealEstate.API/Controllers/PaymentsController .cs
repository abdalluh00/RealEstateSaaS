using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RealEstate.Application.Features.Payments.Commands.CancelPayment;
using RealEstate.Application.Features.Payments.Commands.MarkPaymentPaid;
using RealEstate.Application.Features.Payments.Queries.GetContractPayments;
using RealEstate.Application.Features.Payments.Queries.GetContractPaymentSummary;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Common;

namespace RealEstate.API.Controllers
{
    /// <summary>
    /// إدارة الدفعات
    /// </summary>
    [ApiController]
    [Route("api/payments")]
    [Authorize(Policy = Policies.AgentAndUp)]
    [EnableRateLimiting(RateLimitPolicies.General)]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// الحصول على دفعات عقد محدد.
        /// </summary>
        [HttpGet("contract/{contractId:guid}")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> GetByContract(
            Guid contractId,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new GetContractPaymentsQuery { ContractId = contractId }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// الحصول على ملخص دفعات عقد محدد.
        /// </summary>
        [HttpGet("contract/{contractId:guid}/summary")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> GetSummary(
            Guid contractId,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new GetContractPaymentSummaryQuery { ContractId = contractId }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// تحديد الدفعة كمدفوعة.
        /// </summary>
        [HttpPatch("{id:guid}/paid")]
        [Authorize(Policy = Policies.AdminAndUp)]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> MarkPaid(
            Guid id,
            [FromBody] MarkPaymentPaidCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// إلغاء دفعة محددة.
        /// </summary>
        [HttpPatch("{id:guid}/cancel")]
        [Authorize(Policy = Policies.AdminAndUp)]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
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