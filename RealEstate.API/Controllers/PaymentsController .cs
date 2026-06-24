using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Features.Payments.Commands.CancelPayment;
using RealEstate.Application.Features.Payments.Commands.MarkPaymentPaid;
using RealEstate.Application.Features.Payments.Queries.GetOverduePayments;
using RealEstate.Application.Features.Payments.Queries.GetPayments;
using RealEstate.Application.Features.Payments.Queries.GetPaymentSummary;

namespace RealEstate.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator) => _mediator = mediator;

        [HttpGet("contract/{contractId:guid}")]
        public async Task<IActionResult> GetByContract(Guid contractId)
        {
            var result = await _mediator.Send(new GetPaymentsQuery(contractId));
            return Ok(result);
        }

        [HttpGet("overdue/{companyId:guid}")]
        public async Task<IActionResult> GetOverdue(Guid companyId)
        {
            var result = await _mediator.Send(new GetOverduePaymentsQuery(companyId));
            return Ok(result);
        }

        [HttpGet("summary/{companyId:guid}")]
        public async Task<IActionResult> GetSummary(Guid companyId)
        {
            var result = await _mediator.Send(new GetPaymentSummaryQuery(companyId));
            return Ok(result);
        }

        [HttpPut("pay/{paymentId:guid}")]
        public async Task<IActionResult> MarkPaid(
            Guid paymentId,
            [FromBody] MarkPaymentPaidCommand command)
        {
            if (paymentId != command.PaymentId) return BadRequest("المعرف غير متطابق");
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("cancel/{paymentId:guid}")]
        public async Task<IActionResult> Cancel(Guid paymentId)
        {
            var result = await _mediator.Send(new CancelPaymentCommand(paymentId));
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // PaymentsController.cs
        //[HttpGet("upcoming/{companyId:guid}")]
        //public async Task<IActionResult> GetUpcoming(
        //    Guid companyId,
        //    [FromQuery] int daysAhead = 30)
        //{
        //    var result = await _mediator.Send(
        //        new GetUpcomingPaymentsQuery(companyId, daysAhead));
        //    return Ok(result);
        //}
    }
}
