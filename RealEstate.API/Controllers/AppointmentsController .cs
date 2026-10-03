using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RealEstate.Application.DTOs.Appointments;
using RealEstate.Application.Features.Appointments.Commands.CancelAppointment;
using RealEstate.Application.Features.Appointments.Commands.CompleteAppointment;
using RealEstate.Application.Features.Appointments.Commands.ConfirmAppointment;
using RealEstate.Application.Features.Appointments.Commands.CreateAppointment;
using RealEstate.Application.Features.Appointments.Commands.MarkNoShow;
using RealEstate.Application.Features.Appointments.Queries.GetAppointmentDetail;
using RealEstate.Application.Features.Appointments.Queries.GetPagedAppointments;
using RealEstate.Application.Features.Appointments.Queries.GetTodayAppointments;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Common;

namespace RealEstate.API.Controllers
{
    /// <summary>
    /// إدارة مواعيد المعاينة
    /// </summary>
    [ApiController]
    [Route("api/appointments")]
    [Authorize(Policy = Policies.AgentAndUp)]
    [EnableRateLimiting(RateLimitPolicies.General)]
    public class AppointmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AppointmentController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// الحصول على قائمة مواعيد المعاينة مع الترقيم والتصفية.
        /// </summary>
        /// <remarks>
        /// يمكن تصفية المواعيد حسب الوكيل أو العميل أو العقار أو حالة الموعد
        /// أو نطاق التاريخ والوقت.
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] Guid? agentId = null,
            [FromQuery] Guid? clientId = null,
            [FromQuery] Guid? propertyId = null,
            [FromQuery] AppointmentStatus? status = null,
            [FromQuery] DateTime? from = null,
            [FromQuery] DateTime? to = null,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(new GetPagedAppointmentsQuery
            {
                Page = page,
                PageSize = pageSize,
                AgentId = agentId,
                ClientId = clientId,
                PropertyId = propertyId,
                Status = status,
                From = from,
                To = to
            }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// مواعيد اليوم
        /// </summary>
        /// <remarks>
        /// يعيد جميع المواعيد المجدولة لليوم الحالي (باستثناء الملغاة).
        /// Admin يرى مواعيد جميع الوكلاء، Agent يرى مواعيده فقط عبر فلتر agentId.
        /// </remarks>
        [HttpGet("today")]
        [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<AppointmentListDto>>), 200)]
        public async Task<IActionResult> GetToday(
            [FromQuery] Guid? agentId = null,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new GetTodayAppointmentsQuery { AgentId = agentId }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// الحصول على تفاصيل موعد معاينة محدد.
        /// </summary>
        /// <remarks>
        /// يعرض تفاصيل الموعد والعقار والعميل والوكيل المرتبطين به.
        /// </remarks>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> GetDetail(
            Guid id, CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new GetAppointmentDetailQuery { Id = id }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// إنشاء موعد معاينة
        /// </summary>
        /// <remarks>
        /// ينشئ موعداً جديداً بحالة "قيد الانتظار".
        ///
        /// **فحص التعارض:** يتحقق تلقائياً من عدم تعارض الموعد مع مواعيد الوكيل الأخرى
        /// بناءً على وقت البداية ومدة الموعد.
        ///
        /// **شرط:** العقار يجب أن يكون بحالة "متاح".
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Guid>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 409)]
        public async Task<IActionResult> Create(
            [FromBody] CreateAppointmentCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// تأكيد موعد معاينة.
        /// </summary>
        /// <remarks>
        /// يغير حالة الموعد من "قيد الانتظار" إلى "مؤكد".
        ///
        /// **شرط:** يجب أن يكون الموعد في حالة تسمح بالتأكيد.
        /// </remarks>
        [HttpPatch("{id:guid}/confirm")]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 409)]
        public async Task<IActionResult> Confirm(
            Guid id, CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new ConfirmAppointmentCommand { Id = id }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// إلغاء موعد معاينة.
        /// </summary>
        /// <remarks>
        /// يغير حالة الموعد إلى "ملغى" ويحفظ بيانات الإلغاء
        /// وفقاً لقواعد النظام.
        ///
        /// **شرط:** يمكن إلغاء الموعد فقط إذا كانت حالته تسمح بذلك.
        /// </remarks>
        [HttpPatch("{id:guid}/cancel")]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 409)]
        public async Task<IActionResult> Cancel(
            Guid id,
            [FromBody] CancelAppointmentCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// إكمال موعد المعاينة وتسجيل نتيجته.
        /// </summary>
        /// <remarks>
        /// يغير حالة الموعد إلى "مكتمل" ويسجل بيانات الزيارة
        /// وملاحظاتها ونتيجتها حسب البيانات المرسلة.
        ///
        /// يمكن أن تتضمن النتيجة اهتمام العميل أو عدم اهتمامه
        /// أو حاجته إلى متابعة.
        /// </remarks>
        [HttpPatch("{id:guid}/complete")]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 409)]
        public async Task<IActionResult> Complete(
            Guid id,
            [FromBody] CompleteAppointmentCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// تسجيل عدم حضور العميل للموعد.
        /// </summary>
        /// <remarks>
        /// يغير حالة الموعد إلى "لم يحضر".
        ///
        /// يستخدم هذا الإجراء عندما لا يحضر العميل في الموعد المحدد
        /// دون إلغاء الموعد مسبقاً.
        /// </remarks>
        [HttpPatch("{id:guid}/no-show")]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 409)]
        public async Task<IActionResult> NoShow(
            Guid id, CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new MarkNoShowCommand { Id = id }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}