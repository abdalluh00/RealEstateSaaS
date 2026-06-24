//using FluentAssertions;
//using RealEstate.Tests.Common;

//namespace RealEstate.Tests.Unit.Validators
//{
//    public class CreatePropertyValidatorTests
//    {
//        private readonly CreatePropertyValidator _validator = new();

//        [Fact]
//        public async Task Should_Pass_When_Valid()
//        {
//            // Arrange
//            var command = new CreatePropertyCommand(
//                Title: "شقة النرجس",
//                Type: "Apartment",
//                Price: 35000,
//                Area: 120,
//                Bedrooms: 3,
//                Bathrooms: 2,
//                City: "الرياض",
//                District: "النرجس",
//                Description: null,
//                //CompanyId: TestConstants.CompanyId,
//                OwnerId: null
//            );

//            // Act
//            var result = await _validator.ValidateAsync(command);

//            // Assert
//            result.IsValid.Should().BeTrue();
//        }

//        [Fact]
//        public async Task Should_Fail_When_Title_Empty()
//        {
//            var command = new CreatePropertyCommand(
//                Title: "", // ← فاضي
//                Type: "Apartment",
//                Price: 35000,
//                Area: 120,
//                Bedrooms: null,
//                Bathrooms: null,
//                City: "الرياض",
//                District: "النرجس",
//                Description: null,
//                //CompanyId: TestConstants.CompanyId,
//                OwnerId: null
//            );

//            var result = await _validator.ValidateAsync(command);

//            result.IsValid.Should().BeFalse();
//            result.Errors.Should().Contain(x =>
//                x.ErrorMessage == "عنوان العقار مطلوب");
//        }

//        [Fact]
//        public async Task Should_Fail_When_Price_Zero()
//        {
//            var command = new CreatePropertyCommand(
//                Title: "شقة النرجس",
//                Type: "Apartment",
//                Price: 0, // ← صفر
//                Area: 120,
//                Bedrooms: null,
//                Bathrooms: null,
//                City: "الرياض",
//                District: "النرجس",
//                Description: null,
//                //CompanyId: TestConstants.CompanyId,
//                OwnerId: null
//            );

//            var result = await _validator.ValidateAsync(command);

//            result.IsValid.Should().BeFalse();
//            result.Errors.Should().Contain(x =>
//                x.ErrorMessage == "السعر يجب أن يكون أكبر من صفر");
//        }

//        [Theory]
//        [InlineData(-1)]
//        [InlineData(0)]
//        public async Task Should_Fail_When_Price_Negative(decimal price)
//        {
//            var command = new CreatePropertyCommand(
//                Title: "شقة",
//                Type: "Apartment",
//                Price: price,
//                Area: 120,
//                Bedrooms: null,
//                Bathrooms: null,
//                City: "الرياض",
//                District: "النرجس",
//                Description: null,
//                //CompanyId: TestConstants.CompanyId,
//                OwnerId: null
//            );

//            var result = await _validator.ValidateAsync(command);

//            result.IsValid.Should().BeFalse();
//        }
//    }
//}
