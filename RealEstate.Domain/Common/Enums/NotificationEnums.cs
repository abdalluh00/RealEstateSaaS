using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Domain.Common.Enums
{
    public enum NotificationType
    {
        PaymentReminder,
        ContractExpiry,
        AppointmentReminder,
        Welcome,
        Custom
    }

    public enum NotificationChannel
    {
        WhatsApp,
        SMS,
        Email
    }

    public enum NotificationStatus
    {
        Pending,
        Processing,
        Sent,
        Failed,
        Skipped
    }

    //public enum PaymentStatus
    //{
    //    Pending,
    //    Paid,
    //    Late,
    //    PartiallyPaid,
    //    Cancelled
    //}

    
}
