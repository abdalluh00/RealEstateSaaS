using RealEstate.Domain.Common.Enums;

namespace RealEstate.Application.Features.Properties.Apartments.DTOs
{
    public class ApartmentFilterDto
    {
        public Guid? CompanyId { get; set; }
        public Guid? ContainerPropertyId { get; set; }

        public string? Search { get; set; }

        public PropertyPurpose? Purpose { get; set; }
        public PropertyStatus? PropertyStatus { get; set; }

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public decimal? MinArea { get; set; }
        public decimal? MaxArea { get; set; }

        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }

        public string? City { get; set; }
        public string? District { get; set; }

        public bool? IsFeatured { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
