using MediatR;
using RealEstate.Application.Common.Behaviors;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Companies.Commands.UpdateCompany
{
    [Authorize(Roles = $"{Roles.Owner}")]
    public sealed class UpdateCompanyCommand
        : IRequest<ApiResponse<bool>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public string Name { get; init; } = string.Empty;
        public string? Logo { get; init; }
        public string? Address { get; init; }
        public string Phone { get; init; } = string.Empty;
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}