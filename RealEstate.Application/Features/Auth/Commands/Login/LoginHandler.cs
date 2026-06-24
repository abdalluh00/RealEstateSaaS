using MediatR;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
using RealEstate.Shared.Helpers;

namespace RealEstate.Application.Features.Auth.Commands.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, ApiResponse<LoginResult>>
    {
        private readonly IUserRepository _userRepo;
        private readonly IJwtService _jwtService;

        public LoginHandler(IUserRepository userRepo, IJwtService jwtService)
        {
            _userRepo = userRepo;
            _jwtService = jwtService;
        }

        public async Task<ApiResponse<LoginResult>> Handle(
            LoginCommand request,
            CancellationToken ct)
        {
            var user = await _userRepo.GetByEmailAsync(request.Email);

            if (user is null || !PasswordHelper.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedException("البريد الإلكتروني أو كلمة المرور غير صحيحة");

            if (!user.IsActive)
                throw new UnauthorizedException("الحساب موقوف — تواصل مع المدير");

            var token = _jwtService.GenerateToken(user);

            var result = new LoginResult(
                user.Id,
                token,
                user.FullName,
                user.Email,
                user.Role.ToString(),
                user.CompanyId,
                DateTime.UtcNow.AddDays(7)
            );

            return ApiResponse<LoginResult>.Ok(result, "تم تسجيل الدخول بنجاح");
        }
    }
}
