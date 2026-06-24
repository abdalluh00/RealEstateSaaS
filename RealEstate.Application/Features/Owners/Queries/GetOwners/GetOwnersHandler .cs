using MediatR;
using RealEstate.Application.Features.Owners.DTO;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.Owners.Queries.GetOwners
{
    public class GetOwnersHandler : IRequestHandler<GetOwnersQuery, ApiResponse<List<OwnerDto>>>
    {
        private readonly IOwnerRepository _repo;

        public GetOwnersHandler(IOwnerRepository repo) => _repo = repo;

        public async Task<ApiResponse<List<OwnerDto>>> Handle(
            GetOwnersQuery request,
            CancellationToken ct)
        {
            var owners = await _repo.GetByCompanyAsync(request.CompanyId);

            var result = owners.Select(o => new OwnerDto(
                o.Id, o.FullName, o.Phone,
                o.Email, o.IdNumber,
                o.TotalProperties, o.ActiveContracts,
                o.CreatedAt
            )).ToList();

            return ApiResponse<List<OwnerDto>>.Ok(result);
        }
    }
}
