using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.DTOs.Payments;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Payments.Queries.GetContractPayments
{
    public sealed class GetContractPaymentsQuery
        : IRequest<ApiResponse<IReadOnlyList<PaymentListDto>>>, IAutoTenantRequest
    {
        public Guid ContractId { get; init; }
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}