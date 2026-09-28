//using MediatR;
//using RealEstate.Shared.Common;
//using RealEstate.Application.DTOs.Properties.Base;
//using RealEstate.Application.Interfaces;

//namespace RealEstate.Application.Features.PropertyMedia.Queries.GetPropertyMedia
//{
//    public class GetPropertyMediaHandler
//     : IRequestHandler<GetPropertyMediaQuery, ApiResponse<List<PropertyMediaDto>>>
//    {
//        private readonly IPropertyMediaRepository _repo;

//        public GetPropertyMediaHandler(IPropertyMediaRepository repo) => _repo = repo;

//        public async Task<ApiResponse<List<PropertyMediaDto>>> Handle(
//            GetPropertyMediaQuery request,
//            CancellationToken ct)
//        {
//            var media = await _repo.GetByPropertyAsync(request.PropertyId);

//            var result = media.Select(m => new PropertyMediaDto(
//                m.Id, m.MediaUrl, m.MediaType,
//                m.IsCover, m.SortOrder
//            )).ToList();

//            return ApiResponse<List<PropertyMediaDto>>.Ok(result);
//        }
//    }

//}
