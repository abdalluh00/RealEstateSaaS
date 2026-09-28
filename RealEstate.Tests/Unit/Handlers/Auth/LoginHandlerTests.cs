//using FluentAssertions;
//using Moq;
//using RealEstate.Application.Features.Auth.Commands.Login;
//using RealEstate.Application.Interfaces;
//using RealEstate.Domain.Entities;
//using RealEstate.Domain.Interfaces;
//using RealEstate.Shared.Common.Exceptions;
//using RealEstate.Tests.Common;

//namespace RealEstate.Tests.Unit.Handlers.Auth
//{
//    public class LoginHandlerTests
//    {
//        private readonly Mock<IUserRepository> _userRepoMock = new();
//        private readonly Mock<IJwtService> _jwtMock = new();
//        private readonly LoginHandler _handler;

//        public LoginHandlerTests()
//        {
//            _handler = new LoginHandler(_userRepoMock.Object, _jwtMock.Object);
//        }

//        [Fact]
//        public async Task Should_Login_Successfully()
//        {
//            // Arrange
//            var user = new User
//            {
//                Id = TestConstants.UserId,
//                FullName = "أحمد المالك",
//                Email = "owner@test.com",
//                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123"),
//                Role =Domain.Common.Enums.UserRole.Owner,
//                CompanyId = TestConstants.CompanyId,
//                IsActive = true
//            };

//            _userRepoMock.Setup(x => x.GetByEmailAsync("owner@test.com"))
//                         .ReturnsAsync(user);

//            _jwtMock.Setup(x => x.GenerateToken(user))
//                    .Returns("fake-jwt-token");

//            var command = new LoginCommand("owner@test.com", "Admin123");

//            // Act
//            var result = await _handler.Handle(command, CancellationToken.None);

//            // Assert
//            result.Success.Should().BeTrue();
//            result.Data!.Token.Should().Be("fake-jwt-token");
//            result.Data.Role.Should().Be("Owner");
//        }

//        [Fact]
//        public async Task Should_Fail_When_User_Not_Found()
//        {
//            // Arrange
//            _userRepoMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
//                         .ReturnsAsync((User?)null);

//            var command = new LoginCommand("notfound@test.com", "Admin123");

//            // Act & Assert
//            await _handler.Invoking(h => h.Handle(command, CancellationToken.None))
//                .Should().ThrowAsync<UnauthorizedException>()
//                .WithMessage("البريد الإلكتروني أو كلمة المرور غير صحيحة");
//        }

//        [Fact]
//        public async Task Should_Fail_When_Wrong_Password()
//        {
//            // Arrange
//            var user = new User
//            {
//                Email = "owner@test.com",
//                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123"),
//                IsActive = true
//            };

//            _userRepoMock.Setup(x => x.GetByEmailAsync("owner@test.com"))
//                         .ReturnsAsync(user);

//            //var command = new LoginCommand("owner@test.com", "WrongPassword");

//            // Act & Assert
//            //await _handler.Invoking(h => h.Handle(command, CancellationToken.None))
//                //.Should().ThrowAsync<UnauthorizedException>();
//        }

//        [Fact]
//        public async Task Should_Fail_When_Account_Inactive()
//        {
//            // Arrange
//            var user = new User
//            {
//                Email = "owner@test.com",
//                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123"),
//                IsActive = false // ← موقوف
//            };

//            _userRepoMock.Setup(x => x.GetByEmailAsync("owner@test.com"))
//                         .ReturnsAsync(user);

//            var command = new LoginCommand("owner@test.com", "Admin123");

//            // Act & Assert
//            await _handler.Invoking(h => h.Handle(command, CancellationToken.None))
//                .Should().ThrowAsync<UnauthorizedException>()
//                .WithMessage("الحساب موقوف — تواصل مع المدير");
//        }
//    }

//}
