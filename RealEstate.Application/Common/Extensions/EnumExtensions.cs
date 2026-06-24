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
    }
}
