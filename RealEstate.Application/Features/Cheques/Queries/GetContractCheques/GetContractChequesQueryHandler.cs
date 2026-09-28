using MediatR;
using RealEstate.Application.DTOs.Cheques;
using RealEstate.Application.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Cheques.Queries.GetContractCheques
{
    public sealed class GetContractChequesQueryHandler
        : IRequestHandler<
            GetContractChequesQuery,
            ApiResponse<IReadOnlyList<ChequeListDto>>>
    {
        private readonly IChequeRepository _cheques;
        private readonly IContractRepository _contracts;

        public GetContractChequesQueryHandler(
            IChequeRepository cheques,
            IContractRepository contracts)
        {
            _cheques = cheques;
            _contracts = contracts;
        }

        public async Task<ApiResponse<IReadOnlyList<ChequeListDto>>> Handle(
            GetContractChequesQuery query,
            CancellationToken ct)
        {
            var contract = await _contracts.GetByIdForCommandAsync(
                query.ContractId,
                query.CompanyId,
                ct);

            if (contract is null)
                throw new NotFoundException("العقد غير موجود");

            var cheques = await _cheques.GetByContractAsync(
                query.ContractId,
                query.CompanyId,
                ct);

            return ApiResponse<IReadOnlyList<ChequeListDto>>.Ok(
                cheques);
        }
    }
}