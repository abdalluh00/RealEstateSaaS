using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Authorization;

namespace RealEstate.API.Controllers
{
    /// <summary>
    /// إدارة المهام المجدولة
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [EnableRateLimiting(RateLimitPolicies.General)]
    public class JobsController : ControllerBase
    {
        private readonly INotificationJobService _jobService;

        public JobsController(INotificationJobService jobService) =>
            _jobService = jobService;

        /// <summary>
        /// جدولة إرسال تذكيرات الدفعات.
        /// </summary>
        [HttpPost("payment-reminders")]
        [ProducesResponseType(200)]
        public IActionResult TriggerPaymentReminders()
        {
            BackgroundJob.Enqueue(() => _jobService.SendPaymentRemindersAsync());
            return Ok(new { message = "تم جدولة إرسال التذكيرات" });
        }

        /// <summary>
        /// جدولة إرسال تنبيهات انتهاء العقود.
        /// </summary>
        [HttpPost("contract-expiry")]
        [ProducesResponseType(200)]
        public IActionResult TriggerContractExpiry()
        {
            BackgroundJob.Enqueue(() => _jobService.SendContractExpiryAlertsAsync());
            return Ok(new { message = "تم جدولة إرسال تنبيهات انتهاء العقود" });
        }

        /// <summary>
        /// جدولة إرسال تذكيرات المواعيد.
        /// </summary>
        [HttpPost("appointment-reminders")]
        [ProducesResponseType(200)]
        public IActionResult TriggerAppointmentReminders()
        {
            BackgroundJob.Enqueue(() => _jobService.SendAppointmentRemindersAsync());
            return Ok(new { message = "تم جدولة إرسال تذكيرات المواعيد" });
        }

        /// <summary>
        /// جدولة تحديث الدفعات المتأخرة.
        /// </summary>
        [HttpPost("mark-overdue")]
        [ProducesResponseType(200)]
        public IActionResult TriggerMarkOverdue()
        {
            BackgroundJob.Enqueue(() => _jobService.MarkOverduePaymentsAsync());
            return Ok(new { message = "تم جدولة تحديث الدفعات المتأخرة" });
        }
    }
}