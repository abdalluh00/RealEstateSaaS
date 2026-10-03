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
using RealEstate.Shared.Common;

namespace RealEstate.Api.Controllers
{/// <summary>
/// إدارة الشيكات المرتبطة بالعقود
/// </summary>
/// <remarks>
/// دورة حياة الشيك: معلق ← تم الإيداع ← تم التحصيل
///                              ↓
///                           مرتجع ← تم استبداله بشيك جديد
/// </remarks>
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
        /// <summary>
        /// الحصول على قائمة الشيكات مع الترقيم والتصفية.
        /// </summary>
        /// <remarks>
        /// يمكن تصفية النتائج حسب العقد أو الحالة أو بيانات الشيك
        /// وفقاً للخصائص المتاحة في GetPagedChequesQuery.
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> GetPaged(
            [FromQuery] GetPagedChequesQuery query,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(query, ct);

            return result.Success
                ? Ok(result)
                : BadRequest(result);
        }

        /// <summary>
        /// الحصول على تفاصيل شيك محدد.
        /// </summary>
        /// <remarks>
        /// يعرض تفاصيل الشيك وحالته وبيانات العقد المرتبط به.
        /// </remarks>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
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

        /// <summary>
        /// الحصول على جميع الشيكات المرتبطة بعقد محدد.
        /// </summary>
        /// <remarks>
        /// يعرض جميع الشيكات المسجلة على العقد مع حالاتها الحالية.
        /// </remarks>
        [HttpGet("contract/{contractId:guid}")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
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
        /// <summary>
        /// إنشاء شيك جديد.
        /// </summary>
        /// <remarks>
        /// ينشئ شيكاً جديداً ويربطه بالعقد المحدد.
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Guid>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 409)]
        public async Task<IActionResult> Create(
            [FromBody] CreateChequeCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);

            return result.Success
                ? Ok(result)
                : BadRequest(result);
        }

        /// <summary>
        /// تحديث بيانات شيك محدد.
        /// </summary>
        /// <remarks>
        /// يقوم بتحديث بيانات الشيك باستخدام المعرف الموجود في الرابط.
        ///
        /// **ملاحظة:** يتم اعتماد معرف الشيك الموجود في الرابط بدلاً من أي معرف
        /// يتم إرساله داخل الطلب.
        /// </remarks>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
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

        /// <summary>
        /// تسجيل إيداع الشيك.
        /// </summary>
        /// <remarks>
        /// يغير حالة الشيك إلى "تم الإيداع".
        ///
        /// **شرط:** يجب أن يكون الشيك في حالة تسمح بإيداعه.
        /// </remarks>
        [HttpPost("{id:guid}/deposit")]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 409)]
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

        /// <summary>
        /// تسجيل تحصيل الشيك.
        /// </summary>
        /// <remarks>
        /// يغير حالة الشيك من "تم الإيداع" إلى "تم التحصيل".
        ///
        /// **شرط:** يجب أن يكون الشيك مودعاً ولم يتم تحصيله أو إلغاؤه مسبقاً.
        /// </remarks>
        [HttpPost("{id:guid}/clear")]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 409)]
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
        /// <summary>
        /// تسجيل ارتداد شيك
        /// </summary>
        /// <remarks>
        /// يُغير حالة الشيك إلى "مرتجع" ويحفظ سبب الارتداد وتاريخه.
        /// في النظام القانوني السعودي، الشيك المرتجع جريمة جنائية —
        /// يُنصح بتوثيق السبب بدقة.
        ///
        /// **شرط:** يمكن تسجيل ارتداد الشيكات المودعة فقط.
        /// </remarks>
        [HttpPatch("{id:guid}/bounce")]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 409)]
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

        /// <summary>
        /// إلغاء شيك محدد.
        /// </summary>
        /// <remarks>
        /// يقوم بإلغاء الشيك وتحديث حالته وفقاً لقواعد دورة حياة الشيك.
        ///
        /// **شرط:** يمكن إلغاء الشيك فقط إذا كانت حالته تسمح بذلك.
        /// </remarks>
        [HttpPost("{id:guid}/cancel")]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 409)]
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

        ///// <summary>
        ///// استبدال شيك مرتجع بشيك جديد
        ///// </summary>
        ///// <remarks>
        ///// ينشئ شيكاً بديلاً بنفس المبلغ وترتيب الشيك الأصلي.
        ///// الشيك القديم يُحتفظ به للسجل مع ربطه بالشيك الجديد عبر ReplacedByChequeId.
        /////
        ///// **شرط:** يمكن استبدال الشيكات المرتجعة فقط، ولا يمكن استبدال شيك مرة ثانية.
        ///// </remarks>
        //[HttpPost("{id:guid}/replace")]
        //[ProducesResponseType(typeof(ApiResponse<Guid>), 200)]
        //[ProducesResponseType(typeof(ApiResponse<object>), 409)]
        //public async Task<IActionResult> Replace(
        //    Guid id,
        //    [FromBody] ReplaceChequeCommand cmd,
        //    CancellationToken ct = default)
        //{ }
    }
}