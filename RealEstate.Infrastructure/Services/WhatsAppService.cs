using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace RealEstate.Infrastructure.Services
{
    public class WhatsAppService : IWhatsAppService
    {
        private readonly TwilioSettings _settings;
        private readonly ILogger<WhatsAppService> _logger;

        public WhatsAppService(
            IOptions<TwilioSettings> settings,
            ILogger<WhatsAppService> logger)
        {
            _settings = settings.Value;
            _logger = logger;
            TwilioClient.Init(_settings.AccountSid, _settings.AuthToken);
        }

        public async Task SendAsync(string phone, string message)
        {
            try
            {
                var to = FormatPhone(phone);
                await MessageResource.CreateAsync(
                    from: new Twilio.Types.PhoneNumber(_settings.WhatsAppFrom),
                    to: new Twilio.Types.PhoneNumber($"whatsapp:{to}"),
                    body: message
                );
                _logger.LogInformation("WhatsApp sent to {Phone}", phone);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send WhatsApp to {Phone}", phone);
            }
        }

        public async Task SendPaymentReminderAsync(
            string phone, string clientName,
            decimal amount, DateTime dueDate) =>
            await SendAsync(phone, $"""
            مرحباً {clientName} 👋

            تذكير بموعد دفعة الإيجار:
            💰 المبلغ: {amount:N0} ريال
            📅 تاريخ الاستحقاق: {dueDate:dd/MM/yyyy}

            يرجى السداد في الموعد المحدد.
            شكراً لتعاملكم معنا 🏠
            """);

        public async Task SendContractExpiryAsync(
            string phone, string clientName,
            DateTime expiryDate, string propertyTitle) =>
            await SendAsync(phone, $"""
            مرحباً {clientName} 👋

            تنبيه: عقدك على العقار ({propertyTitle}) سينتهي قريباً.
            📅 تاريخ الانتهاء: {expiryDate:dd/MM/yyyy}

            للتجديد أو الاستفسار تواصل معنا. 🏠
            """);

        public async Task SendAppointmentReminderAsync(
            string phone, string clientName,
            DateTime scheduledAt, string propertyTitle) =>
            await SendAsync(phone, $"""
            مرحباً {clientName} 👋

            تذكير بموعد زيارة العقار:
            🏠 العقار: {propertyTitle}
            📅 التاريخ: {scheduledAt:dd/MM/yyyy}
            🕐 الوقت: {scheduledAt:hh:mm tt}

            نتطلع لاستقبالك! 😊
            """);

        public async Task SendWelcomeAsync(string phone, string clientName) =>
            await SendAsync(phone, $"""
            مرحباً {clientName} 👋

            أهلاً بك في نظامنا العقاري!
            يسعدنا خدمتك في إيجاد العقار المناسب. 🏠

            للاستفسار تواصل معنا في أي وقت.
            """);

        // تحويل رقم الجوال السعودي للصيغة الدولية
        private static string FormatPhone(string phone)
        {
            phone = phone.Replace(" ", "").Replace("-", "");

            if (phone.StartsWith("05"))
                return "+966" + phone[1..];

            if (phone.StartsWith("5") && phone.Length == 9)
                return "+966" + phone;

            return phone;
        }
    }
}
