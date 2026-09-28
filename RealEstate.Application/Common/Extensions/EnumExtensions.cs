using RealEstate.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Common.Extensions
{
    public static class EnumExtensions
    {
        public static string ToArabicString(this PropertyStatus status) => status switch
        {
            PropertyStatus.Available => "متاح",
            PropertyStatus.Rented => "مؤجر",
            PropertyStatus.Sold => "مباع",
            PropertyStatus.Reserved => "محجوز",
            _ => status.ToString()
        };

        public static string ToArabicString(this PropertyPurpose purpose) => purpose switch
        {
            PropertyPurpose.ForRent => "للإيجار",
            PropertyPurpose.ForSale => "للبيع",
            PropertyPurpose.ForRentAndSale => "للبيع أو للايجار",
            _ => purpose.ToString()
        };
        public static string ToArabicString(this PropertyType type) => type switch
        {
            PropertyType.Office => "مكتب",
            PropertyType.Warehouse => "مستودع",
            PropertyType.Villa => "فيلا",
            PropertyType.Building => "بناء",
            PropertyType.Apartment => "شقة",
            PropertyType.Land => "أرض",
            _ => type.ToString()
        };
        public static string ToArabicString(this FurnishedStatus furnishedStatus) => furnishedStatus switch
        {
            FurnishedStatus.FullyFurnished => "مفروشة",
            FurnishedStatus.Unfurnished => "غير مفروشه",
            FurnishedStatus.SemiFurnished => "نصف مفروشة",
            _ => furnishedStatus.ToString()
        };

        public static string ToArabicString(this ZoningType value) => value switch
        {
            ZoningType.Agricultural => "زراعي",
            ZoningType.Industrial => "صناعي",
            ZoningType.Residential => "سكني",
            ZoningType.Commercial => "تجاري",
            _ => value.ToString()
        };

        public static string ToArabicString(this LandShape value) => value switch
        {
            LandShape.Square => "مربع",
            LandShape.Rectangular => "مستطيل",
            LandShape.Irregular => "غير منظم",
            _ => value.ToString()
        };


        public static string ToArabicString(this FacingDirection value) => value switch
        {
            FacingDirection.North => "شمال",
            FacingDirection.South => "جنوب",
            FacingDirection.East => "شرق",
            FacingDirection.West => "غرب",
            FacingDirection.NorthEast => "شمال شرق",
            FacingDirection.NorthWest => "شمال غرب",
            FacingDirection.SouthEast => "جنوب شرق",
            FacingDirection.SouthWest => "جنوب غرب",
            _ => value.ToString()
        };

        public static string ToArabicString(this ElectricityCapacity value) => value switch
        {
            ElectricityCapacity.V220 => "220 فولت",
            ElectricityCapacity.V380 => "380 فولت",
            ElectricityCapacity.V440 => "440 فولت",
            _ => value.ToString()
        };

        public static string ToArabicString(this OwnerType value) => value switch
        {
            OwnerType.Individual => "فرد",
            OwnerType.Company => "شركة",
            _ => value.ToString()
        };

        public static string ToArabicString(this AppointmentStatus value) => value switch
        {
            AppointmentStatus.Pending => "قيد الانتظار",
            AppointmentStatus.Confirmed => "مؤكد",
            AppointmentStatus.Done => "تم",
            AppointmentStatus.Cancelled => "ملغى",
            AppointmentStatus.NoShow => "لم يحضر",
            _ => value.ToString()
        };

        public static string ToArabicString(this LeadStatus value) => value switch
        {
            LeadStatus.Lead => "عميل محتمل",
            LeadStatus.Prospect => "عميل متوقع",
            LeadStatus.Active => "عميل نشط",
            LeadStatus.Inactive => "عميل غير نشط",
            _ => value.ToString()
        };

        public static string ToArabicString(this LeadSource value) => value switch
        {
            LeadSource.WhatsApp => "واتساب",
            LeadSource.Website => "موقع إلكتروني",
            LeadSource.Referral => "إحالة",
            LeadSource.WalkIn => "زيارة مباشرة",
            LeadSource.Call => "مكالمة هاتفية",
            _ => value.ToString()
        };


        public static string ToArabicString(this PaymentStatus value) => value switch
        {
            PaymentStatus.Pending => "قيد الانتظار",
            PaymentStatus.Paid => "تم الدفع",
            PaymentStatus.Overdue => "متأخر",
            PaymentStatus.Cancelled => "ملغى",
            _ => value.ToString()
        };

        public static string ToArabicString(this PaymentMethod value) => value switch
        {
            PaymentMethod.Cash => "نقداً",
            PaymentMethod.BankTransfer => "تحويل بنكي",
            PaymentMethod.Moyasar => "مويسار",
            PaymentMethod.Cheque => "شيك",
            _ => value.ToString()
        };

      
        public static string ToArabicString(this UserRole value) => value switch
        {
            UserRole.Admin => "مدير",
            UserRole.Agent => "وكيل",
            UserRole.Viewer => "مشاهد",
            UserRole.Owner => "المالك",
            _ => value.ToString()
        };

        public static string ToArabicString(this SubscriptionPlan value) => value switch
        {
            SubscriptionPlan.Basic => "أساسي",
            SubscriptionPlan.Pro => "متقدم",
            SubscriptionPlan.Business => "أعمال",
            _ => value.ToString()
        };

        public static string ToArabicString(this ChequeStatus status) =>
           status switch
         {
        ChequeStatus.Pending => "معلق",
        ChequeStatus.Deposited => "تم الإيداع",
        ChequeStatus.Cleared => "تم التحصيل",
        ChequeStatus.Bounced => "مرتجع",
        ChequeStatus.Cancelled => "ملغي",
        _ => status.ToString()
        };

        public static string ToArabicString(this MediaType type) =>
           type switch
         {
        MediaType.Image => "صورة",
        MediaType.Video => "فيديو",
        MediaType.Document => "مستند",
        _ => type.ToString()
       };

        public static string ToArabicString(this MaintenanceCategory category) =>
    category switch
    {
        MaintenanceCategory.Plumbing => "سباكة",
        MaintenanceCategory.Electric => "كهرباء",
        MaintenanceCategory.AC => "تكييف",
        MaintenanceCategory.Elevator => "مصعد",
        MaintenanceCategory.Structure => "إنشائي",
        MaintenanceCategory.Painting => "دهان",
        MaintenanceCategory.Other => "أخرى",
        _ => category.ToString()
    };

        public static string ToArabicString(this MaintenancePriority priority) =>
            priority switch
            {
                MaintenancePriority.Low => "منخفض",
                MaintenancePriority.Medium => "متوسط",
                MaintenancePriority.High => "مرتفع",
                MaintenancePriority.Urgent => "عاجل",
                _ => priority.ToString()
            };

        public static string ToArabicString(this MaintenanceStatus status) =>
            status switch
            {
                MaintenanceStatus.Open => "مفتوح",
                MaintenanceStatus.InProgress => "قيد التنفيذ",
                MaintenanceStatus.Done => "مكتمل",
                MaintenanceStatus.Cancelled => "ملغى",
                _ => status.ToString()
            };

        public static string ToArabicString(this MaintenanceMediaType mediaType) =>
    mediaType switch
    {
        MaintenanceMediaType.Image => "صورة",
        MaintenanceMediaType.Video => "فيديو",
        _ => mediaType.ToString()
    };

        public static string ToArabicString(this UploadedByType uploadedByType) =>
            uploadedByType switch
            {
                UploadedByType.Client => "عميل",
                UploadedByType.User => "مستخدم",
                _ => uploadedByType.ToString()
            };

        public static string ToArabicString(this MediaStage stage) =>
            stage switch
            {
                MediaStage.Before => "قبل",
                MediaStage.After => "بعد",
                _ => stage.ToString()
            };


        // ── Nullable overloads ────────────────────────────────────────────────
        public static string? ToArabicString(this FurnishedStatus? value) =>
            value.HasValue ? value.Value.ToArabicString() : null;

        public static string? ToArabicString(this ZoningType? value) =>
            value.HasValue ? value.Value.ToArabicString() : null;

        public static string? ToArabicString(this LandShape? value) =>
            value.HasValue ? value.Value.ToArabicString() : null;

        public static string? ToArabicString(this FacingDirection? value) =>
            value.HasValue ? value.Value.ToArabicString() : null;

        public static string? ToArabicString(this ElectricityCapacity? value) =>
            value.HasValue ? value.Value.ToArabicString() : null;

        public static string? ToArabicString(this PaymentMethod? value) =>
            value.HasValue ? value.Value.ToArabicString() : null;

        public static string? ToArabicString(this LeadSource? value) =>
            value.HasValue ? value.Value.ToArabicString() : null;

        public static string? ToArabicString(this AppointmentResult? value) =>
            value.HasValue ? value.Value.ToArabicString() : null;

        // ── Missing non-nullable enums ────────────────────────────────────────
        public static string ToArabicString(this ContractType value) => value switch
        {
            ContractType.Rent => "إيجار",
            ContractType.Sale => "بيع",
            _ => value.ToString()
        };

        public static string ToArabicString(this ContractStatus value) => value switch
        {
            ContractStatus.Active => "نشط",
            ContractStatus.Expired => "منتهي",
            ContractStatus.Cancelled => "ملغى",
            ContractStatus.Renewed => "مجدد",
            _ => value.ToString()
        };

        public static string ToArabicString(this CommissionType value) => value switch
        {
            CommissionType.Fixed => "ثابت",
            CommissionType.Percentage => "نسبة مئوية",
            _ => value.ToString()
        };

        public static string ToArabicString(this CommissionStatus value) => value switch
        {
            CommissionStatus.Pending => "قيد الانتظار",
            CommissionStatus.Paid => "مدفوعة",
            _ => value.ToString()
        };

        public static string ToArabicString(this PaymentCycle value) => value switch
        {
            PaymentCycle.Monthly => "شهري",
            PaymentCycle.Quarterly => "ربع سنوي",
            PaymentCycle.SemiAnnual => "نصف سنوي",
            PaymentCycle.Annual => "سنوي",
            _ => value.ToString()
        };

        public static string? ToArabicString(this PaymentCycle? value) =>
            value.HasValue ? value.Value.ToArabicString() : null;

        public static string ToArabicString(this AppointmentResult value) => value switch
        {
            AppointmentResult.Interested => "مهتم",
            AppointmentResult.NotInterested => "غير مهتم",
            AppointmentResult.NeedsFollowUp => "يحتاج متابعة",
            _ => value.ToString()
        };
    }
}
