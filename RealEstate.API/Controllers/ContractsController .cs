using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Features.Contracts.Commands.CancelContract;
using RealEstate.Application.Features.Contracts.Commands.CreateContract;
using RealEstate.Application.Features.Contracts.Commands.MarkCommissionPaid;
using RealEstate.Application.Features.Contracts.Commands.RenewContract;
using RealEstate.Application.Features.Contracts.Queries.GetContractDetail;
using RealEstate.Application.Features.Contracts.Queries.GetPagedContracts;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Authorization;

namespace RealEstate.API.Controllers
{
    [ApiController]
    [Route("api/contracts")]
    [Authorize(Policy = Policies.AgentAndUp)]
    public class ContractController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContractController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] ContractStatus? status = null,
            [FromQuery] ContractType? contractType = null,
            [FromQuery] Guid? agentId = null,
            [FromQuery] Guid? clientId = null,
            [FromQuery] Guid? propertyId = null,
            [FromQuery] DateTime? dateFrom = null,
            [FromQuery] DateTime? dateTo = null,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(new GetPagedContractsQuery
            {
                Page = page,
                PageSize = pageSize,
                Status = status,
                ContractType = contractType,
                AgentId = agentId,
                ClientId = clientId,
                PropertyId = propertyId,
                DateFrom = dateFrom,
                DateTo = dateTo
            }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetDetail(
            Guid id, CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new GetContractDetailQuery { Id = id }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        [Authorize(Policy = Policies.AdminAndUp)]
        public async Task<IActionResult> Create(
            [FromBody] CreateContractCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("{id:guid}/cancel")]
        [Authorize(Policy = Policies.AdminAndUp)]
        public async Task<IActionResult> Cancel(
            Guid id,
            [FromBody] CancelContractCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id:guid}/renew")]
        [Authorize(Policy = Policies.AdminAndUp)]
        public async Task<IActionResult> Renew(
            Guid id,
            [FromBody] RenewContractCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("{id:guid}/commission-paid")]
        [Authorize(Policy = Policies.AdminAndUp)]
        public async Task<IActionResult> MarkCommissionPaid(
            Guid id, CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new MarkCommissionPaidCommand { Id = id }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}