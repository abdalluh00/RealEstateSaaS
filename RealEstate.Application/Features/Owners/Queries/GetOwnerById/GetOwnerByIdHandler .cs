using MediatR;
using RealEstate.Application.Features.Owners.DTO;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
namespace RealEstate.Application.Features.Owners.Queries.GetOwnerById
{
    public class GetOwnerByIdHandler : IRequestHandler<GetOwnerByIdQuery, ApiResponse<OwnerDetailDto>>
    {
        private readonly IOwnerRepository _repo;

        public GetOwnerByIdHandler(IOwnerRepository repo) => _repo = repo;

        public async Task<ApiResponse<OwnerDetailDto>> Handle(
            GetOwnerByIdQuery request,
            CancellationToken ct)
        {
            var owner = await _repo.GetWithPropertiesAsync(request.Id);

            if (owner is null)
                throw new NotFoundException("المالك", request.Id);

            var result = new OwnerDetailDto(
                owner.Id,
                owner.FullName,
                owner.Phone,
                owner.Email,
                owner.IdNumber,
                owner.Notes,
                owner.Properties.Select(p => new OwnerPropertyDto(
                    p.Id, p.Title, p.Type,
                    p.Status, p.Price, p.City
                )).ToList()
            );

            return ApiResponse<OwnerDetailDto>.Ok(result);
        }
    }
}
