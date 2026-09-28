using MediatR;
using RealEstate.Application.DTOs.Properties.Villa;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Properties.Villas.Queries.GetVillasPaged
{
    public class GetVillasPagedQueryHandler
        : IRequestHandler<GetVillasPagedQuery, ApiResponse<PagedResult<VillaListDto>>>
    {
        private readonly IVillaRepository _villaRepository;

        public GetVillasPagedQueryHandler(IVillaRepository villaRepository)
        {
            _villaRepository = villaRepository;
        }

        public async Task<ApiResponse<PagedResult<VillaListDto>>> Handle(GetVillasPagedQuery query, CancellationToken ct)
        {
            var paged = await _villaRepository.GetPagedAsync
                (
                companyId: query.CompanyId,
                page: query.Page,
                pageSize: query.PageSize,
                status: query.status,
                purpose: query.purpose,
                minBedrooms: query.MinBedrooms,
                maxBedrooms: query.MaxBedrooms,
                furnishedStatus: query.FurnishedStatus,
                ct: ct);

           

            return ApiResponse<PagedResult<VillaListDto>>.Ok(paged);
        }
    }
}
