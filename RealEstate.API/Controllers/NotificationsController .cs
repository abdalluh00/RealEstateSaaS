using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Authorization;

namespace RealEstate.API.Controllers
{
    /// <summary>
    /// إدارة الإشعارات والرسائل
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [EnableRateLimiting(RateLimitPolicies.General)]
    public class NotificationsController : ControllerBase
    {
        private readonly IWhatsAppService _whatsApp;

        public NotificationsController(IWhatsAppService whatsApp) =>
            _whatsApp = whatsApp;

        /// <summary>
        /// إرسال رسالة عبر واتساب.
        /// </summary>
        [HttpPost("send")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Send(
            [FromBody] SendMessageRequest request)
        {
            await _whatsApp.SendAsync(request.Phone, request.Message);
            return Ok(new { success = true, message = "تم إرسال الرسالة" });
        }
    }

    /// <summary>
    /// بيانات إرسال الرسالة.
    /// </summary>
    public record SendMessageRequest(string Phone, string Message);
}