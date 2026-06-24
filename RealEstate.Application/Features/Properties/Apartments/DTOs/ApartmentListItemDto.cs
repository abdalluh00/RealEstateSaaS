using RealEstate.Domain.Common.Enums;


namespace RealEstate.Application.Features.Properties.Apartments.DTOs
{
    public class ApartmentListItemDto
    {
        public Guid Id { get; set; }
        public string PropertyCode { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;

        public PropertyPurpose Purpose { get; set; }
        public PropertyStatus PropertyStatus { get; set; }

        public decimal Price { get; set; }
        public decimal Area { get; set; }

        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;

        public Guid? ParentPropertyId { get; set; }
        public Guid OwnerId { get; set; }
        public Guid AgentId { get; set; }

        public string UnitNumber { get; set; } = string.Empty;
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int FloorNumber { get; set; }

        public bool IsFeatured { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
