using MediatR;
using RealEstate.Application.Features.Contracts.DTO;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
namespace RealEstate.Application.Features.Contracts.Queries.GetContractById
{
    public class GetContractByIdHandler
        : IRequestHandler<GetContractByIdQuery, ApiResponse<ContractDetailDto>>
    {
        private readonly IContractRepository _repo;

        public GetContractByIdHandler(IContractRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse<ContractDetailDto>> Handle(
            GetContractByIdQuery request,
            CancellationToken ct)
        {
            var contract = await _repo.GetDetailsAsync(request.CompanyId, request.Id, ct);

            if (contract is null)
                throw new NotFoundException("العقد غير موجود");

            var result = new ContractDetailDto
            {
                Id = contract.Id,
                ContractNumber = contract.ContractNumber,

                ContractType = contract.ContractType,
                ContractStatus = contract.ContractStatus,

                Amount = contract.Amount,
                SecurityDeposit = contract.SecurityDeposit,

                PaymentCycle = contract.PaymentCycle,
                PaymentMethod = contract.PaymentMethod,

                CommissionType = contract.CommissionType,
                Commission = contract.Commission,
                CommissionStatus = contract.CommissionStatus,

                StartDate = contract.StartDate,
                EndDate = contract.EndDate,

                CancellationReason = contract.CancellationReason,
                Notes = contract.Notes,

                PropertyId = contract.PropertyId,
                PropertyTitle = contract.Property.Title,

                ClientId = contract.ClientId,
                ClientName = contract.Client.FullName,
                ClientPhone = contract.Client.Phone,

                AgentId = contract.AgentId,
                AgentName = contract.Agent.FullName,

                CompanyId = contract.CompanyId
            };

            return ApiResponse<ContractDetailDto>.Ok(result);
        }
    }
}
