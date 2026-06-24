using MediatR;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.Auth.Queries.GetMe
{
    public record GetMeQuery(Guid UserId) : IRequest<ApiResponse<MeDto>>;

    public record MeDto(
        Guid Id,
        string FullName,
        string Email,
        string Phone,
        string Role,
        Guid CompanyId,
        string CompanyName
    );
}
