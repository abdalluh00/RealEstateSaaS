using MediatR;
using RealEstate.Application.Features.PropertyMedia.DTO;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.PropertyMedia.Queries.GetPropertyMedia
{
    public class GetPropertyMediaHandler
     : IRequestHandler<GetPropertyMediaQuery, ApiResponse<List<MediaDto>>>
    {
        private readonly IPropertyMediaRepository _repo;

        public GetPropertyMediaHandler(IPropertyMediaRepository repo) => _repo = repo;

        public async Task<ApiResponse<List<MediaDto>>> Handle(
            GetPropertyMediaQuery request,
            CancellationToken ct)
        {
            var media = await _repo.GetByPropertyAsync(request.PropertyId);

            var result = media.Select(m => new MediaDto(
                m.Id, m.MediaUrl, m.MediaType,
                m.IsCover, m.SortOrder
            )).ToList();

            return ApiResponse<List<MediaDto>>.Ok(result);
        }
    }

}
