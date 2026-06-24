
namespace RealEstate.Domain.Interfaces
{
    public interface IWhatsAppService
    {
        Task SendAsync(string phone, string message);
        Task SendPaymentReminderAsync(string phone, string clientName, decimal amount, DateTime dueDate);
        Task SendContractExpiryAsync(string phone, string clientName, DateTime expiryDate, string propertyTitle);
        Task SendAppointmentReminderAsync(string phone, string clientName, DateTime scheduledAt, string propertyTitle);
        Task SendWelcomeAsync(string phone, string clientName);
    }
}
