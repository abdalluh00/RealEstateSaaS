using MediatR;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Auth.Commands.SignUp
{
    public record SignUpCommand(
    // بيانات الشركة
    string CompanyName,
    string CompanyPhone,
    string? CompanyAddress,

    // بيانات المالك
    string FullName,
    string Email,
    string Phone,
    string Password,
    string ConfirmPassword
) : IRequest<ApiResponse<SignUpResult>>;

    public record SignUpResult(
        string Token,
        string FullName,
        string Email,
        UserRole Role,
        Guid CompanyId,
        DateTime ExpiresAt
    );
}
