using Microsoft.AspNetCore.Mvc;
using RealEstate.Domain.Interfaces;

namespace RealEstate.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
