using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Domain.ReadModels.PropertyModel
{
    public sealed record PropertyDashboardReadModel
    {
        public int TotalProperties { get; init; }
        public int Available { get; init; }
        public int Rented { get; init; }
        public int Sold { get; init; }
        public int Reserved { get; init; }
        public int Featured { get; init; }
        public int Published { get; init; }
    }
}
