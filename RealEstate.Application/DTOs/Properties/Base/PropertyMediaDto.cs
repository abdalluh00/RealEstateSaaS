using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.DTOs.Properties.Base
{
    public record PropertyMediaDto
    {
        public Guid Id { get; init; }
        public string MediaUrl { get; init; } = string.Empty;
        public string MediaType { get; init; } = string.Empty;
        public bool IsCover { get; init; }
        public int SortOrder { get; init; }
    }
}
