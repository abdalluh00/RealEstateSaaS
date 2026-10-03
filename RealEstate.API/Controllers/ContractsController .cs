using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RealEstate.Application.Features.Contracts.Commands.CancelContract;
using RealEstate.Application.Features.Contracts.Commands.CreateContract;
using RealEstate.Application.Features.Contracts.Commands.MarkCommissionPaid;
using RealEstate.Application.Features.Contracts.Commands.RenewContract;
using RealEstate.Application.Features.Contracts.Queries.GetContractDetail;
using RealEstate.Application.Features.Contracts.Queries.GetPagedContracts;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Common;

namespace RealEstate.API.Controllers
{/// <summary>
/// إدارة العقود
/// </summary>
    [ApiController]
    [Route("api/contracts")]
    [Authorize(Policy = Policies.AgentAndUp)]
    [EnableRateLimiting(RateLimitPolicies.General)]
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
        /// <summary>
        /// إنشاء عقد جديد
        /// </summary>
        /// <remarks>
        /// عند إنشاء عقد إيجار يتم تلقائياً:
        /// - تغيير حالة العقار من "متاح" إلى "مؤجر"
        /// - إنشاء دفعات تلقائية بناءً على دورة الدفع ومدة العقد
        ///
        /// عند إنشاء عقد بيع:
        /// - تغيير حالة العقار من "متاح" إلى "مباع"
        /// - لا تُنشأ دفعات تلقائية
        ///
        /// **شرط:** لا يمكن إنشاء عقد على عقار لديه عقد نشط بالفعل.
        /// </remarks>
        [HttpPost]
        [Authorize(Policy = Policies.AdminAndUp)]
        [ProducesResponseType(typeof(ApiResponse<Guid>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 409)]
        public async Task<IActionResult> Create(
            [FromBody] CreateContractCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        /// <summary>
        /// إلغاء عقد
        /// </summary>
        /// <remarks>
        /// يُلغي العقد ويُعيد حالة العقار إلى "متاح" تلقائياً.
        ///
        /// **شرط:** يمكن إلغاء العقود النشطة فقط.
        /// </remarks>
        [HttpPatch("{id:guid}/cancel")]
        [Authorize(Policy = Policies.AdminAndUp)]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 409)]
        public async Task<IActionResult> Cancel(
            Guid id,
            [FromBody] CancelContractCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        /// <summary>
        /// تجديد عقد
        /// </summary>
        /// <remarks>
        /// ينشئ عقداً جديداً مرتبطاً بالعقد القديم عبر RenewedFromContractId.
        /// العقد القديم يُغير حالته إلى "مجدد" تلقائياً.
        ///
        /// **شرط:** يمكن تجديد العقود النشطة أو المنتهية فقط.
        /// </remarks>
        [HttpPost("{id:guid}/renew")]
        [Authorize(Policy = Policies.AdminAndUp)]
        [ProducesResponseType(typeof(ApiResponse<Guid>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 409)]
        public async Task<IActionResult> Renew(
            Guid id,
            [FromBody] RenewContractCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        /// <summary>
        /// تحديد أن عمولة العقد قد تم دفعها.
        /// </summary>
        /// <remarks>
        /// يقوم بتحديث حالة عمولة العقد إلى "مدفوعة".
        ///
        /// **شرط:** يجب أن يكون العقد موجوداً ويمكن تحديث حالة العمولة الخاصة به.
        /// </remarks>
        [HttpPatch("{id:guid}/commission-paid")]
        [Authorize(Policy = Policies.AdminAndUp)]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> MarkCommissionPaid(
            Guid id, CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new MarkCommissionPaidCommand { Id = id }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}