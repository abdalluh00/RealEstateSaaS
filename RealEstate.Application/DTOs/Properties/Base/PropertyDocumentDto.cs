using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.DTOs.Properties.Base
{
    public record PropertyDocumentDto
    {
        public Guid Id { get; init; }
        public string DocumentType { get; init; } = string.Empty;
        public string DocumentName { get; init; } = string.Empty;
        public string FileUrl { get; init; } = string.Empty;
        public DateTime? ExpiryDate { get; init; }
    }
}
