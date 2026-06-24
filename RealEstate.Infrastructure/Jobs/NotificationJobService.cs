
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Persistence;

namespace RealEstate.Infrastructure.Jobs
{
    public class NotificationJobService : INotificationJobService
    {
        private readonly AppDbContext _context;
        private readonly IWhatsAppService _whatsApp;
        private readonly ILogger<NotificationJobService> _logger;

        public NotificationJobService(
            AppDbContext context,
            IWhatsAppService whatsApp,
            ILogger<NotificationJobService> logger)
        {
            _context = context;
            _whatsApp = whatsApp;
            _logger = logger;
        }

        public void ScheduleJobs()
        {
            // كل يوم الساعة 8 صباحاً
            RecurringJob.AddOrUpdate(
                "payment-reminders",
                () => SendPaymentRemindersAsync(),
                "0 8 * * *");

            // كل يوم الساعة 9 صباحاً
            RecurringJob.AddOrUpdate(
                "contract-expiry-alerts",
                () => SendContractExpiryAlertsAsync(),
                "0 9 * * *");

            // كل يوم الساعة 7 صباحاً
            RecurringJob.AddOrUpdate(
                "appointment-reminders",
                () => SendAppointmentRemindersAsync(),
                "0 7 * * *");

            // كل يوم الساعة 1 صباحاً
            RecurringJob.AddOrUpdate(
                "mark-overdue-payments",
                () => MarkOverduePaymentsAsync(),
                "0 1 * * *");

            _logger.LogInformation("Background jobs scheduled successfully");
        }

        // 1 — تذكير الدفعات قبل 3 أيام
        public async Task SendPaymentRemindersAsync()
        {
            var targetDate = DateTime.UtcNow.AddDays(3);
            var today = DateTime.UtcNow.Date;

            var payments = await _context.Payments
                .AsNoTracking()
                .Include(x => x.Contract)
                    .ThenInclude(x => x.Client)
                .Include(x => x.Contract)
                    .ThenInclude(x => x.Property)
                .Where(x =>
                    x.Status == "Pending" &&
                    x.DueDate.Date >= today &&
                    x.DueDate.Date <= targetDate.Date)
                .ToListAsync();

            _logger.LogInformation("Sending {Count} payment reminders", payments.Count);

            foreach (var payment in payments)
            {
                await _whatsApp.SendPaymentReminderAsync(
                    payment.Contract.Client.Phone,
                    payment.Contract.Client.FullName,
                    payment.Amount,
                    payment.DueDate
                );
            }
        }

        // 2 — تنبيه انتهاء العقود قبل 30 يوم
        public async Task SendContractExpiryAlertsAsync()
        {
            var targetDate = DateTime.UtcNow.AddDays(30);
            var today = DateTime.UtcNow.Date;

            var contracts = await _context.Contracts
               .AsNoTracking()
                 .Include(x => x.Client)
                .Include(x => x.Property)
                 .Where(x =>
                   x.ContractStatus == ContractStatus.Active &&
                    x.EndDate.HasValue &&                              // ✅ Null check FIRST
                   x.EndDate.Value.Date >= today.Date &&              // ✅ Compare dates properly
                     x.EndDate.Value.Date <= targetDate.Date)
                   .ToListAsync();
                   _logger.LogInformation("Sending {Count} contract expiry alerts", contracts.Count);

            foreach (var contract in contracts)
            {
                await _whatsApp.SendContractExpiryAsync(
                    contract.Client.Phone,
                    contract.Client.FullName,
                    contract.EndDate!.Value,
                    contract.Property.Title
                );
            }
        }

        // 3 — تذكير المواعيد قبل يوم
        public async Task SendAppointmentRemindersAsync()
        {
            var tomorrow = DateTime.UtcNow.AddDays(1).Date;

            var appointments = await _context.Appointments
                .AsNoTracking()
                .Include(x => x.Client)
                .Include(x => x.Property)
                .Where(x =>
                    x.Status == AppointmentStatus.Confirmed &&
                    x.ScheduledAt.Date == tomorrow)
                .ToListAsync();

            _logger.LogInformation("Sending {Count} appointment reminders", appointments.Count);

            foreach (var appointment in appointments)
            {
                await _whatsApp.SendAppointmentReminderAsync(
                    appointment.Client.Phone,
                    appointment.Client.FullName,
                    appointment.ScheduledAt,
                    appointment.Property.Title
                );
            }
        }

        // 4 — تحديث الدفعات المتأخرة تلقائياً
        public async Task MarkOverduePaymentsAsync()
        {
            var today = DateTime.UtcNow.Date;

            var overduePayments = await _context.Payments
                .Where(x => x.Status == "Pending" && x.DueDate.Date < today)
                .ToListAsync();

            foreach (var payment in overduePayments)
                payment.Status = "Late";

            await _context.SaveChangesAsync();

            _logger.LogInformation("Marked {Count} payments as Late", overduePayments.Count);
        }

       
    }
}
