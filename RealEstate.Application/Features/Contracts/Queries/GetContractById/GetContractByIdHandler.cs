using MediatR;
using RealEstate.Application.DTOs.Contracts;
using RealEstate.Application.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Contracts.Queries.GetContractDetail
{
    public sealed class GetContractDetailQueryHandler
        : IRequestHandler<GetContractDetailQuery, ApiResponse<ContractDetailDto>>
    {
        private readonly IContractRepository _contracts;

        public GetContractDetailQueryHandler(IContractRepository contracts)
            => _contracts = contracts;

        public async Task<ApiResponse<ContractDetailDto>> Handle(
            GetContractDetailQuery query,
            CancellationToken ct)
        {
            var contract = await _contracts.GetDetailByIdAsync(
                query.Id, query.CompanyId, ct);

            if (contract is null)
                throw new NotFoundException("العقد", query.Id);

            return ApiResponse<ContractDetailDto>.Ok(contract, "Success");
        }
    }
}