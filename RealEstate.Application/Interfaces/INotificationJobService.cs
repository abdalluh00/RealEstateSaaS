using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Domain.Interfaces
{
    public interface INotificationJobService
    {
        void ScheduleJobs();
        Task SendPaymentRemindersAsync();
        Task SendContractExpiryAlertsAsync();
        Task SendAppointmentRemindersAsync();
        Task MarkOverduePaymentsAsync();
    }
}
