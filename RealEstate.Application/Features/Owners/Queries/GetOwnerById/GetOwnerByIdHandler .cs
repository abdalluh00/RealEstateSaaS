using MediatR;
using RealEstate.Application.DTOs.Owner;
using RealEstate.Application.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Owners.Queries.GetOwnerDetail
{
    public sealed class GetOwnerDetailQueryHandler
        : IRequestHandler<GetOwnerDetailQuery, ApiResponse<OwnerDetailDto>>
    {
        private readonly IOwnerRepository _owners;

        public GetOwnerDetailQueryHandler(IOwnerRepository owners)
            => _owners = owners;

        public async Task<ApiResponse<OwnerDetailDto>> Handle(
            GetOwnerDetailQuery query,
            CancellationToken ct)
        {
            var owner = await _owners.GetDetailByIdAsync(
                query.Id, query.CompanyId, ct);

            if (owner is null)
                throw new NotFoundException("المالك", query.Id);

            return ApiResponse<OwnerDetailDto>.Ok(owner, "تم استرجاع بيانات المالك بنجاح");
        }
    }
}