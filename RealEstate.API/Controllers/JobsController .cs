using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Authorization;

namespace RealEstate.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableRateLimiting(RateLimitPolicies.General)]
    public class JobsController : ControllerBase
    {
        private readonly INotificationJobService _jobService;

        public JobsController(INotificationJobService jobService) =>
            _jobService = jobService;

        // تشغيل يدوي للتجربة
        [HttpPost("payment-reminders")]
        public IActionResult TriggerPaymentReminders()
        {
            BackgroundJob.Enqueue(() => _jobService.SendPaymentRemindersAsync());
            return Ok(new { message = "تم جدولة إرسال التذكيرات" });
        }

        [HttpPost("contract-expiry")]
        public IActionResult TriggerContractExpiry()
        {
            BackgroundJob.Enqueue(() => _jobService.SendContractExpiryAlertsAsync());
            return Ok(new { message = "تم جدولة إرسال تنبيهات انتهاء العقود" });
        }

        [HttpPost("appointment-reminders")]
        public IActionResult TriggerAppointmentReminders()
        {
            BackgroundJob.Enqueue(() => _jobService.SendAppointmentRemindersAsync());
            return Ok(new { message = "تم جدولة إرسال تذكيرات المواعيد" });
        }

        [HttpPost("mark-overdue")]
        public IActionResult TriggerMarkOverdue()
        {
            BackgroundJob.Enqueue(() => _jobService.MarkOverduePaymentsAsync());
            return Ok(new { message = "تم جدولة تحديث الدفعات المتأخرة" });
        }
    }
}
