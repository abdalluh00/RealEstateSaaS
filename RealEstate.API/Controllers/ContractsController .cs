using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Features.Contracts.Commands.CreateContract;
using RealEstate.Application.Features.Contracts.Queries.GetContractById;
using RealEstate.Application.Features.Contracts.Queries.GetContracts;
using RealEstate.Application.Features.Payments.Commands.MarkPaymentPaid;

namespace RealEstate.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContractsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContractsController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetContractsQuery());
            return Ok(result);
        }

        [HttpGet("detail/{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetContractByIdQuery(id));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateContractCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
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
    }
}
