//using FluentAssertions;
//using Moq;
//using RealEstate.Application.Features.Properties.Commands.CreateProperty;
//using RealEstate.Domain.Entities;
//using RealEstate.Domain.Interfaces;
//using RealEstate.Tests.Common;

//namespace RealEstate.Tests.Unit.Handlers.Properties
//{
//    public class CreatePropertyHandlerTests
//    {
//        private readonly Mock<IPropertyRepository> _repoMock = new();
//        private readonly CreatePropertyHandler _handler;

//        public CreatePropertyHandlerTests()
//        {
//            _handler = new CreatePropertyHandler(_repoMock.Object);
//        }

//        [Fact]
//        public async Task Should_Create_Property_Successfully()
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
//               // CompanyId: TestConstants.CompanyId,
//                OwnerId: null
//            );

//            _repoMock.Setup(x => x.AddAsync(It.IsAny<Property>()))
//                     .Returns(Task.CompletedTask);

//            _repoMock.Setup(x => x.SaveChangesAsync())
//                     .ReturnsAsync(1);

//            // Act
//            var result = await _handler.Handle(command, CancellationToken.None);

//            // Assert
//            result.Success.Should().BeTrue();
//            result.Data.Should().NotBeEmpty();
//            result.Message.Should().Be("تم إضافة العقار بنجاح");

//            // تحقق إن AddAsync استُدعي مرة واحدة
//            _repoMock.Verify(x => x.AddAsync(It.IsAny<Property>()), Times.Once);
//            _repoMock.Verify(x => x.SaveChangesAsync(), Times.Once);
//        }
//    }
//}
