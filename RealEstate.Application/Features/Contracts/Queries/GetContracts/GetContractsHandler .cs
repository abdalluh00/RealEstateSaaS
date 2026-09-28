using MediatR;
using RealEstate.Application.DTOs.Contracts;
using RealEstate.Application.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Contracts.Queries.GetPagedContracts
{
    public sealed class GetPagedContractsQueryHandler
        : IRequestHandler<GetPagedContractsQuery, ApiResponse<PagedResult<ContractListDto>>>
    {
        private readonly IContractRepository _contracts;

        public GetPagedContractsQueryHandler(IContractRepository contracts)
            => _contracts = contracts;

        public async Task<ApiResponse<PagedResult<ContractListDto>>> Handle(
            GetPagedContractsQuery query,
            CancellationToken ct)
        {
            var result = await _contracts.GetPagedAsync(
                companyId: query.CompanyId,
                page: query.Page,
                pageSize: query.PageSize,
                status: query.Status,
                contractType: query.ContractType,
                agentId: query.AgentId,
                clientId: query.ClientId,
                propertyId: query.PropertyId,
                dateFrom: query.DateFrom,
                dateTo: query.DateTo,
                ct: ct);

            return ApiResponse<PagedResult<ContractListDto>>.Ok(result, "Success");
        }
    }
}