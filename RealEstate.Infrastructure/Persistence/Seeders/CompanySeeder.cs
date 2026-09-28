using Microsoft.Extensions.Logging;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Persistence;

namespace RealEstate.Infrastructure.Persistence.Seeders
{
    public static class CompanySeeder
    {
        public static async Task<List<Company>> SeedAsync(
            AppDbContext context,
            ILogger logger)
        {
            var companies = new List<Company>
            {
                new()
                {
                    Id                 = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name               = "شركة الأفق للعقارات",
                    Phone              = "0112345678",
                    Address            = "الرياض، حي العليا، شارع التخصصي",
                    SubscriptionPlan   = SubscriptionPlan.Basic,
                    SubscriptionExpiry = DateTime.UtcNow.AddYears(1),
                    IsActive           = true,
                    CreatedAt          = DateTime.UtcNow
                },
                new()
                {
                    Id                 = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name               = "مجموعة النخيل العقارية",
                    Phone              = "0123456789",
                    Address            = "جدة، حي الزهراء، شارع الأمير سلطان",
                    SubscriptionPlan   = SubscriptionPlan.Pro,
                    SubscriptionExpiry = DateTime.UtcNow.AddYears(1),
                    IsActive           = true,
                    CreatedAt          = DateTime.UtcNow
                },
                new()
                {
                    Id                 = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Name               = "الرواد للتطوير العقاري",
                    Phone              = "0134567890",
                    Address            = "الدمام، حي الفيصلية، شارع الملك فهد",
                    SubscriptionPlan   = SubscriptionPlan.Business,
                    SubscriptionExpiry = DateTime.UtcNow.AddYears(1),
                    IsActive           = true,
                    CreatedAt          = DateTime.UtcNow
                }
            };

            await context.Companies.AddRangeAsync(companies);
            await context.SaveChangesAsync();

            logger.LogInformation("Seeded {Count} companies", companies.Count);
            return companies;
        }
    }
}