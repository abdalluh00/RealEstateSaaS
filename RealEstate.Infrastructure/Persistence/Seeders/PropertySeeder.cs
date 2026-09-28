using Microsoft.Extensions.Logging;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Infrastructure.Persistence;

namespace RealEstate.Infrastructure.Persistence.Seeders
{
    public static class PropertySeeder
    {
        public static async Task<List<Property>> SeedAsync(
            AppDbContext context,
            List<Company> companies,
            List<Owner> owners,
            List<User> users,
            ILogger logger)
        {
            var properties = new List<Property>();
            var counter = 1;

            foreach (var company in companies)
            {
                var owner = owners.First(o => o.CompanyId == company.Id);
                var agent = users.First(u =>
                    u.CompanyId == company.Id &&
                    u.Role == UserRole.Agent);

                // ── Apartment ─────────────────────────────
                properties.Add(new ApartmentProperty
                {
                    Id = Guid.NewGuid(),
                    PropertyCode = $"PROP-{DateTime.UtcNow.Year}-{counter++:D4}",
                    Type = PropertyType.Apartment,
                    Title = "شقة فاخرة في حي العليا",
                    Description = "شقة مميزة بإطلالة رائعة في قلب الرياض",
                    Purpose = PropertyPurpose.ForRent,
                    PropertyStatus = PropertyStatus.Available,
                    Price = 2500,
                    Area = 150,
                    City = "الرياض",
                    District = "العليا",
                    Address = "شارع التخصصي، برج الأفق",
                    Bedrooms = 3,
                    Bathrooms = 2,
                    LivingRooms = 1,
                    FloorNumber = 5,
                    HasElevator = true,
                    HasBalcony = true,
                    HasCentralAC = true,
                    FurnishedStatus = FurnishedStatus.SemiFurnished,
                    IsFeatured = true,
                    IsPublished = true,
                    CompanyId = company.Id,
                    OwnerId = owner.Id,
                    AgentId = agent.Id,
                    CreatedAt = DateTime.UtcNow
                });

                // ── Villa ─────────────────────────────────
                properties.Add(new VillaProperty
                {
                    Id = Guid.NewGuid(),
                    PropertyCode = $"PROP-{DateTime.UtcNow.Year}-{counter++:D4}",
                    Type = PropertyType.Villa,
                    Title = "فيلا فاخرة في حي النرجس",
                    Description = "فيلا راقية مع مسبح وحديقة خاصة",
                    Purpose = PropertyPurpose.ForSale,
                    PropertyStatus = PropertyStatus.Available,
                    Price = 3500000,
                    Area = 600,
                    City = "الرياض",
                    District = "النرجس",
                    Bedrooms = 5,
                    Bathrooms = 6,
                    LivingRooms = 2,
                    Floors = 2,
                    HasPool = true,
                    HasGarden = true,
                    GardenArea = 200,
                    HasMajlis = true,
                    HasMaidRoom = true,
                    HasDriverRoom = true,
                    HasElevator = true,
                    HasCCTV = true,
                    HasGenerator = true,
                    FurnishedStatus = FurnishedStatus.Unfurnished,
                    IsFeatured = true,
                    IsPublished = true,
                    CompanyId = company.Id,
                    OwnerId = owner.Id,
                    AgentId = agent.Id,
                    CreatedAt = DateTime.UtcNow
                });

                // ── Office ────────────────────────────────
                properties.Add(new OfficeProperty
                {
                    Id = Guid.NewGuid(),
                    PropertyCode = $"PROP-{DateTime.UtcNow.Year}-{counter++:D4}",
                    Type = PropertyType.Office,
                    Title = "مكتب تجاري في برج المملكة",
                    Description = "مكتب راقٍ في أبرز برج تجاري",
                    Purpose = PropertyPurpose.ForRent,
                    PropertyStatus = PropertyStatus.Available,
                    Price = 8000,
                    Area = 200,
                    City = "الرياض",
                    District = "العليا",
                    FloorNumber = 15,
                    OfficesCount = 5,
                    MeetingRooms = 2,
                    HasElevator = true,
                    HasCentralAC = true,
                    HasReceptionArea = true,
                    HasKitchen = true,
                    HasCCTV = true,
                    FurnishedStatus = FurnishedStatus.FullyFurnished,
                    IsPublished = true,
                    CompanyId = company.Id,
                    OwnerId = owner.Id,
                    AgentId = agent.Id,
                    CreatedAt = DateTime.UtcNow
                });

                // ── Warehouse ─────────────────────────────
                properties.Add(new WarehouseProperty
                {
                    Id = Guid.NewGuid(),
                    PropertyCode = $"PROP-{DateTime.UtcNow.Year}-{counter++:D4}",
                    Type = PropertyType.Warehouse,
                    Title = "مستودع صناعي في المنطقة الصناعية",
                    Description = "مستودع كبير مع مدخل للشاحنات",
                    Purpose = PropertyPurpose.ForRent,
                    PropertyStatus = PropertyStatus.Available,
                    Price = 15000,
                    Area = 1000,
                    City = "الرياض",
                    District = "المنطقة الصناعية",
                    CeilingHeight = 8,
                    LoadingDocks = 3,
                    GateCount = 2,
                    ElectricityCapacity = ElectricityCapacity.V380,
                    HasOfficeSpace = true,
                    HasSecurityRoom = true,
                    HasCCTV = true,
                    HasFireSystem = true,
                    HasTruckAccess = true,
                    IsFenced = true,
                    IsPublished = true,
                    CompanyId = company.Id,
                    OwnerId = owner.Id,
                    AgentId = agent.Id,
                    CreatedAt = DateTime.UtcNow
                });

                // ── Land ──────────────────────────────────
                properties.Add(new LandProperty
                {
                    Id = Guid.NewGuid(),
                    PropertyCode = $"PROP-{DateTime.UtcNow.Year}-{counter++:D4}",
                    Type = PropertyType.Land,
                    Title = "أرض سكنية في حي الياسمين",
                    Description = "أرض مميزة في موقع استراتيجي",
                    Purpose = PropertyPurpose.ForSale,
                    PropertyStatus = PropertyStatus.Available,
                    Price = 1200000,
                    Area = 800,
                    City = "الرياض",
                    District = "الياسمين",
                    StreetWidth = 20,
                    NumberOfStreets = 2,
                    ZoningType = ZoningType.Residential,
                    LandShape = LandShape.Rectangular,
                    IsCornerLand = true,
                    HasElectricity = true,
                    HasWater = true,
                    HasSewer = true,
                    IsPublished = true,
                    CompanyId = company.Id,
                    OwnerId = owner.Id,
                    AgentId = agent.Id,
                    CreatedAt = DateTime.UtcNow
                });

                // ── Building ──────────────────────────────
                properties.Add(new BuildingProperty
                {
                    Id = Guid.NewGuid(),
                    PropertyCode = $"PROP-{DateTime.UtcNow.Year}-{counter++:D4}",
                    Type = PropertyType.Building,
                    Title = "عمارة سكنية في حي المروج",
                    Description = "عمارة حديثة مكونة من 10 طوابق",
                    Purpose = PropertyPurpose.ForSale,
                    PropertyStatus = PropertyStatus.Available,
                    Price = 8000000,
                    Area = 2000,
                    City = "الرياض",
                    District = "المروج",
                    TotalFloors = 10,
                    UnitsCount = 40,
                    BasementFloors = 1,
                    HasElevator = true,
                    HasParkingFloor = true,
                    HasMosque = true,
                    HasGuard = true,
                    HasGenerator = true,
                    HasCCTV = true,
                    IsFeatured = true,
                    IsPublished = true,
                    CompanyId = company.Id,
                    OwnerId = owner.Id,
                    AgentId = agent.Id,
                    CreatedAt = DateTime.UtcNow
                });
            }

            await context.Properties.AddRangeAsync(properties);
            await context.SaveChangesAsync();

            logger.LogInformation("Seeded {Count} properties", properties.Count);
            return properties;
        }
    }
}