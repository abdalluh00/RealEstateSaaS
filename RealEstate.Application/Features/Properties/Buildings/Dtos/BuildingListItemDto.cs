namespace RealEstate.Application.Features.Properties.Buildings.Dtos
{
    public sealed class BuildingListItemDto
    {
        public Guid Id { get; set; }
        public string PropertyCode { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal Area { get; set; }
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string PropertyStatus { get; set; } = string.Empty;
        public int? TotalFloors { get; set; }
        public int? UnitsCount { get; set; }
        public bool Elevator { get; set; }
        public bool ParkingFloor { get; set; }
        public Guid OwnerId { get; set; }
        public Guid AgentId { get; set; }
    }
}
