using MediatR;
using RealEstate.Application.Features.Contracts.DTO;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.Contracts.Queries.GetContracts
{
    public class GetContractsHandler
       : IRequestHandler<GetContractsQuery, ApiResponse<List<ContractDto>>>
    {
        private readonly IContractRepository _contractRepository;

        public GetContractsHandler(IContractRepository contractLookupRepository)
        {
            _contractRepository = contractLookupRepository;
        }

        public async Task<ApiResponse<List<ContractDto>>> Handle(
            GetContractsQuery request,
            CancellationToken ct)
        {
            var contracts = await _contractRepository.GetByCompanyAsync(
                request.CompanyId,
                ct);

            var result = contracts.Select(c => new ContractDto
            {
                Id = c.Id,
                ContractNumber = c.ContractNumber,
                PropertyTitle = c.PropertyTitle,
                ClientName = c.ClientName,
                ClientPhone = c.ClientPhone,
                AgentName = c.AgentName,
                ContractType = c.ContractType.ToString(),
                Status = c.Status.ToString(),
                Amount = c.Amount,
                Commission = c.Commission,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                TotalPayments = c.TotalPayments,
                PaidPayments = c.PaidPayments,
                TotalPaid = c.TotalPaid
            }).ToList();

            return ApiResponse<List<ContractDto>>.Ok(result);
        }
    }
}
