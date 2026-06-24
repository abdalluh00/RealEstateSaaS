using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Common.Enums;
using RealEstate.Infrastructure.Persistence;

namespace RealEstate.Tests.Common
{
    public abstract class TestBase : IDisposable
    {
        protected readonly AppDbContext Context;

        protected TestBase()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // ← DB جديدة لكل Test
                .Options;

            Context = new AppDbContext(options);
            Context.Database.EnsureCreated();

            SeedData();
        }

        private void SeedData()
        {
            // شركة تجريبية
            var company = new Domain.Entities.Company
            {
                Id = TestConstants.CompanyId,
                Name = "مكتب النرجس",
                Phone = "0512345678",
                SubscriptionPlan = "Basic",
                SubscriptionExpiry = DateTime.UtcNow.AddDays(30),
                IsActive = true
            };

            // مستخدم تجريبي
            var user = new Domain.Entities.User
            {
                Id = TestConstants.UserId,
                FullName = "أحمد المالك",
                Email = "owner@test.com",
                Phone = "0512345679",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123"),
                Role = UserRole.Owner,
                CompanyId = TestConstants.CompanyId,
                IsActive = true
            };

            Context.Companies.Add(company);
            Context.Users.Add(user);
            Context.SaveChanges();
        }

        public void Dispose() => Context.Dispose();
    }

  
}
