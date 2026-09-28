using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Persistence;

namespace RealEstate.Infrastructure.Jobs
{
    public class NotificationJobService : INotificationJobService
    {
        private readonly AppDbContext _context;
        private readonly IWhatsAppService _whatsApp;
        private readonly IPaymentRepository _payments;
        private readonly ILogger<NotificationJobService> _logger;

        public NotificationJobService(
            AppDbContext context,
            IWhatsAppService whatsApp,
            IPaymentRepository payments,
            ILogger<NotificationJobService> logger)
        {
            _context = context;
            _whatsApp = whatsApp;
            _payments = payments;
            _logger = logger;
        }

        public void ScheduleJobs()
        {
            RecurringJob.AddOrUpdate(
                "payment-reminders",
                () => SendPaymentRemindersAsync(),
                "0 8 * * *");

            RecurringJob.AddOrUpdate(
                "contract-expiry-alerts",
                () => SendContractExpiryAlertsAsync(),
                "0 9 * * *");

            RecurringJob.AddOrUpdate(
                "appointment-reminders",
                () => SendAppointmentRemindersAsync(),
                "0 7 * * *");

            RecurringJob.AddOrUpdate(
                "mark-overdue-payments",
                () => MarkOverduePaymentsAsync(),
                "0 1 * * *");

            _logger.LogInformation("Background jobs scheduled successfully");
        }

        // 1 — تذكير الدفعات قبل 3 أيام
        public async Task SendPaymentRemindersAsync()
        {
            var today = DateTime.UtcNow.Date;
            var targetDate = today.AddDays(3);

            var payments = await _context.Payments
                .AsNoTracking()
                .Include(p => p.Contract)
                    .ThenInclude(c => c.Client)
                .Include(p => p.Contract)
                    .ThenInclude(c => c.Property)
                .Where(p => p.PaymentStatus == PaymentStatus.Pending
                         && p.DueDate.Date >= today
                         && p.DueDate.Date <= targetDate)
                .ToListAsync();

            _logger.LogInformation(
                "Sending {Count} payment reminders", payments.Count);

            foreach (var payment in payments)
                await _whatsApp.SendPaymentReminderAsync(
                    payment.Contract.Client.Phone,
                    payment.Contract.Client.FullName,
                    payment.Amount,
                    payment.DueDate);
        }

        // 2 — تنبيه انتهاء العقود قبل 30 يوم
        public async Task SendContractExpiryAlertsAsync()
        {
            var today = DateTime.UtcNow.Date;
            var targetDate = today.AddDays(30);

            var contracts = await _context.Contracts
                .AsNoTracking()
                .Include(c => c.Client)
                .Include(c => c.Property)
                .Where(c => c.ContractStatus == ContractStatus.Active
                         && c.EndDate.HasValue
                         && c.EndDate.Value.Date >= today
                         && c.EndDate.Value.Date <= targetDate)
                .ToListAsync();

            _logger.LogInformation(
                "Sending {Count} contract expiry alerts", contracts.Count);

            foreach (var contract in contracts)
                await _whatsApp.SendContractExpiryAsync(
                    contract.Client.Phone,
                    contract.Client.FullName,
                    contract.EndDate!.Value,
                    contract.Property.Title);
        }

        // 3 — تذكير المواعيد قبل يوم
        public async Task SendAppointmentRemindersAsync()
        {
            var tomorrow = DateTime.UtcNow.AddDays(1).Date;

            var appointments = await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Client)
                .Include(a => a.Property)
                .Where(a => a.Status == AppointmentStatus.Confirmed
                         && a.ScheduledAt.Date == tomorrow)
                .ToListAsync();

            _logger.LogInformation(
                "Sending {Count} appointment reminders", appointments.Count);

            foreach (var appointment in appointments)
                await _whatsApp.SendAppointmentReminderAsync(
                    appointment.Client.Phone,
                    appointment.Client.FullName,
                    appointment.ScheduledAt,
                    appointment.Property.Title);
        }

        // 4 — تحديث الدفعات المتأخرة
        public async Task MarkOverduePaymentsAsync()
        {
            var count = await _payments.MarkOverdueAsync();
            _logger.LogInformation("Marked {Count} payments as Overdue", count);
        }
    }
}