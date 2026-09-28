//using MediatR;
//using RealEstate.Domain.Interfaces;
//using RealEstate.Shared.Common;
//using RealEstate.Shared.Common.Exceptions;

//namespace RealEstate.Application.Features.Auth.Queries.GetMe
//{
//    public class GetMeHandler : IRequestHandler<GetMeQuery, ApiResponse<MeDto>>
//    {
//        private readonly IUserRepository _repo;

//        public GetMeHandler(IUserRepository repo) => _repo = repo;

//        public async Task<ApiResponse<MeDto>> Handle(
//            GetMeQuery request,
//            CancellationToken ct)
//        {
//            var user = await _repo.GetWithCompanyAsync(request.UserId);

//            if (user is null)
//                throw new NotFoundException("المستخدم", request.UserId);

//            return ApiResponse<MeDto>.Ok(new MeDto(
//                user.Id,
//                user.FullName,
//                user.Email,
//                user.Phone,
//                user.Role.ToString(),
//                user.CompanyId,
//                user.CompanyName
//            ));
//        }
//    }
//}
