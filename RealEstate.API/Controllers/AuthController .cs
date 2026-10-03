using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RealEstate.API.Swagger.Examples;
using RealEstate.API.Swagger.Examples.Auth;
using RealEstate.Application.DTOs.Users;
using RealEstate.Application.Features.Auth.Commands.AcceptInvitation;
using RealEstate.Application.Features.Auth.Commands.ChangePassword;
using RealEstate.Application.Features.Auth.Commands.Login;
using RealEstate.Application.Features.Auth.Commands.ResetPassword;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Common;
using Swashbuckle.AspNetCore.Filters;

namespace RealEstate.API.Controllers
{ /// <summary>
  /// إدارة المصادقة وتسجيل الدخول
  /// </summary>
    [ApiController]
    [Route("api/auth")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator) => _mediator = mediator;
        /// <summary>
        /// تسجيل الدخول للنظام
        /// </summary>
        /// <remarks>
        /// يتحقق من البريد الإلكتروني وكلمة المرور ويعيد JWT token صالح لمدة 7 أيام.
        ///
        /// **ملاحظة:** محدود بـ 5 محاولات في الدقيقة لكل IP لحماية من هجمات القوة الغاشمة.
        ///
        /// بيانات تسجيل الدخول للاختبار:
        /// - Owner:  owner.basic@realestate.test / Test@1234
        /// - Admin:  admin.basic@realestate.test / Test@1234
        /// - Agent:  agent.basic@realestate.test / Test@1234
        /// </remarks>
        /// <param name="cmd">بيانات تسجيل الدخول</param>
        /// <param name="ct">رمز الإلغاء</param>
        [HttpPost("login")]
        [EnableRateLimiting(RateLimitPolicies.Login)]
        [SwaggerRequestExample(typeof(LoginCommand), typeof(LoginRequestExample))]
        [SwaggerResponseExample(200, typeof(LoginResponseExample))]
        [SwaggerResponseExample(400, typeof(ValidationErrorExample))]
        [SwaggerResponseExample(401, typeof(UnauthorizedErrorExample))]
        [ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 401)]
        [ProducesResponseType(typeof(ApiResponse<object>), 429)]
        public async Task<IActionResult> Login(
            [FromBody] LoginCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        /// <summary>
        /// قبول دعوة الانضمام وتفعيل الحساب
        /// </summary>
        /// <remarks>
        /// يستخدم رمز الدعوة المرسل عبر واتساب لتفعيل الحساب وتعيين كلمة المرور.
        /// الرمز صالح لمدة 48 ساعة من وقت الإرسال.
        /// </remarks>
        /// <param name="cmd">رمز الدعوة وكلمة المرور الجديدة</param>
        /// <param name="ct">رمز الإلغاء</param>
        [HttpPost("accept-invitation")]
        [EnableRateLimiting(RateLimitPolicies.AcceptInvitation)]
        [ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 409)]
        public async Task<IActionResult> AcceptInvitation(
            [FromBody] AcceptInvitationCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// طلب إعادة تعيين كلمة المرور
        /// </summary>
        /// <remarks>
        /// يرسل رمز إعادة التعيين عبر واتساب إلى رقم الجوال المرتبط بالبريد الإلكتروني.
        /// الرمز صالح لمدة ساعة واحدة.
        ///
        /// **ملاحظة أمنية:** يعيد نفس الرسالة سواء كان البريد موجوداً أم لا لمنع كشف البيانات.
        /// </remarks>
        /// <param name="cmd">البريد الإلكتروني</param>
        /// <param name="ct">رمز الإلغاء</param>
        [HttpPost("reset-password")]
        [EnableRateLimiting(RateLimitPolicies.ResetPassword)]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 429)]
        public async Task<IActionResult> ResetPassword(
            [FromBody] ResetPasswordCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// تغيير كلمة المرور باستخدام رمز إعادة التعيين
        /// </summary>
        /// <remarks>
        /// يستخدم الرمز المرسل عبر واتساب لتعيين كلمة مرور جديدة.
        /// الرمز يُحذف فور الاستخدام — لا يمكن استخدامه مرتين.
        /// </remarks>
        /// <param name="cmd">رمز إعادة التعيين وكلمة المرور الجديدة</param>
        /// <param name="ct">رمز الإلغاء</param>
        [HttpPost("change-password")]
        [EnableRateLimiting(RateLimitPolicies.ChangePassword)]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 409)]
        public async Task<IActionResult> ChangePassword(
            [FromBody] ChangePasswordCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}