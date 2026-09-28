using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.DTOs.Clients;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Clients.Queries.GetClientDetail
{
    public sealed class GetClientDetailQuery
        : IRequest<ApiResponse<ClientDetailDto>>, IAutoTenantRequest
    {
        public Guid Id { get; init; }
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}