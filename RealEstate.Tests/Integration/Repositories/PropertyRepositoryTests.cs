//using FluentAssertions;
//using RealEstate.Domain.Entities;
//using RealEstate.Infrastructure.Repositories;
//using RealEstate.Tests.Common;
//using static Twilio.Rest.Intelligence.V3.ConfigurationResource;

//namespace RealEstate.Tests.Integration.Repositories
//{
//    public class PropertyRepositoryTests : TestBase
//    {
//        private readonly PropertyRepository _repo;

//        public PropertyRepositoryTests()
//        {
//            _repo = new PropertyRepository(Context);
//        }

//        [Fact]
//        public async Task Should_Add_And_Get_Property()
//        {
//            // Arrange
//            //var property = new Property
//            //{
//            //    Title = "شقة النرجس",
//            //    Type = "Apartment",
//            //    Price = 35000,
//            //    Area = 120,
//            //    City = "الرياض",
//            //    District = "النرجس",
//            //    Status = "Available",
//            //    CompanyId = TestConstants.CompanyId
//            //};

//            //// Act
//            //await _repo.AddAsync(property);
//            //await _repo.SaveChangesAsync();

//            //var result = await _repo.GetByIdAsync(property.Id);

//            // Assert
//            result.Should().NotBeNull();
//            result!.Title.Should().Be("شقة النرجس");
//            result.Price.Should().Be(35000);
//        }

//        [Fact]
//        public async Task Should_Get_Properties_By_Company()
//        {
//            // Arrange
//            var property1 = new Property
//            {
//                Title = "شقة 1",
//                Type = "Apartment",
//                Price = 35000,
//                Area = 120,
//                City = "الرياض",
//                District = "النرجس",
//                Status = "Available",
//                CompanyId = TestConstants.CompanyId
//            };

//            var property2 = new Property
//            {
//                Title = "شقة 2",
//                Type = "Apartment",
//                Price = 45000,
//                Area = 150,
//                City = "الرياض",
//                District = "الملقا",
//                Status = "Available",
//                CompanyId = TestConstants.CompanyId
//            };

//            await _repo.AddAsync(property1);
//            await _repo.AddAsync(property2);
//            await _repo.SaveChangesAsync();

//            // Act
//            var result = await _repo.GetByCompanyAsync(TestConstants.CompanyId);

//            // Assert
//            result.Should().HaveCount(2);
//            result.Should().Contain(x => x.Title == "شقة 1");
//            result.Should().Contain(x => x.Title == "شقة 2");
//        }

//        [Fact]
//        public async Task Should_Not_Return_Deleted_Properties()
//        {
//            // Arrange
//            var property = new Property
//            {
//                Title = "شقة محذوفة",
//                Type = "Apartment",
//                Price = 35000,
//                Area = 120,
//                City = "الرياض",
//                District = "النرجس",
//                Status = "Available",
//                CompanyId = TestConstants.CompanyId,
//                IsDeleted = true // ← محذوف
//            };

//            await _repo.AddAsync(property);
//            await _repo.SaveChangesAsync();

//            // Act
//            var result = await _repo.GetByCompanyAsync(TestConstants.CompanyId);

//            // Assert
//            result.Should().NotContain(x => x.Title == "شقة محذوفة");
//        }

//        [Fact]
//        public async Task Should_Soft_Delete_Property()
//        {
//            // Arrange
//            var property = new Property
//            {
//                Title = "شقة للحذف",
//                Type = "Apartment",
//                Price = 35000,
//                Area = 120,
//                City = "الرياض",
//                District = "النرجس",
//                Status = "Available",
//                CompanyId = TestConstants.CompanyId
//            };

//            await _repo.AddAsync(property);
//            await _repo.SaveChangesAsync();

//            // Act
//            property.IsDeleted = true;
//            _repo.Update(property);
//            await _repo.SaveChangesAsync();

//          //  var result = await _repo.GetByIdAsync(property.Id);

//            // Assert
//            //result.Should().BeNull(); // لأن Global Filter يخفيه
//            var result = await _repo.GetByCompanyAsync(TestConstants.CompanyId);
//            result.Should().NotContain(x => x.Id == property.Id);
//        }
//    }
//}
