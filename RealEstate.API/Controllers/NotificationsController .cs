using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Authorization;

namespace RealEstate.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableRateLimiting(RateLimitPolicies.General)]
    public class NotificationsController : ControllerBase
    {
        private readonly IWhatsAppService _whatsApp;

        public NotificationsController(IWhatsAppService whatsApp) =>
            _whatsApp = whatsApp;

        // إرسال رسالة يدوية
        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] SendMessageRequest request)
        {
            await _whatsApp.SendAsync(request.Phone, request.Message);
            return Ok(new { success = true, message = "تم إرسال الرسالة" });
        }
    }

    public record SendMessageRequest(string Phone, string Message);
}
