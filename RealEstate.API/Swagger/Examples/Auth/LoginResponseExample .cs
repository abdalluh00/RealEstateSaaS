using RealEstate.Application.DTOs.Users;
using RealEstate.Shared.Common;
using Swashbuckle.AspNetCore.Filters;

namespace RealEstate.API.Swagger.Examples.Auth
{
    public class LoginResponseExample : IExamplesProvider<ApiResponse<LoginResponseDto>>
    {
        public ApiResponse<LoginResponseDto> GetExamples() =>
            ApiResponse<LoginResponseDto>.Ok(new LoginResponseDto
            {
                Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
                UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                FullName = "مالك شركة الأفق للعقارات",
                Email = "owner.basic@realestate.test",
                Role = "Owner",
                CompanyId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            }, "تم تسجيل الدخول بنجاح");
    }
}