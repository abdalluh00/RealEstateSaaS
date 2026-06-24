using RealEstate.Domain.Common.Enums;

namespace RealEstate.Application.Features.PropertyMedia.DTO
{
    public record MediaDto(
    Guid Id,
    string MediaUrl,
    MediaType MediaType,
    bool IsCover,
    int SortOrder
);
}
