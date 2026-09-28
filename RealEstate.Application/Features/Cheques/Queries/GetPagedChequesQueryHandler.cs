using MediatR;
using RealEstate.Application.DTOs.Cheques;
using RealEstate.Application.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Cheques.Queries.GetPagedCheques
{
    public sealed class GetPagedChequesQueryHandler
        : IRequestHandler<
            GetPagedChequesQuery,
            ApiResponse<PagedResult<ChequeListDto>>>
    {
        private readonly IChequeRepository _cheques;

        public GetPagedChequesQueryHandler(
            IChequeRepository cheques)
        {
            _cheques = cheques;
        }

        public async Task<ApiResponse<PagedResult<ChequeListDto>>> Handle(
            GetPagedChequesQuery query,
            CancellationToken ct)
        {
            var result = await _cheques.GetPagedAsync(
                query.CompanyId,
                query.Page,
                query.PageSize,
                query.ContractId,
                query.Status,
                query.From,
                query.To,
                ct);

            return ApiResponse<PagedResult<ChequeListDto>>.Ok(result);
        }
    }
}