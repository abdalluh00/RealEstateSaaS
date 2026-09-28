using MediatR;
using Microsoft.AspNetCore.Authorization;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.Owners.Commands.UpdateOwner
{
    [Authorize(Roles = $"{Roles.Admin}")]
    public sealed class UpdateOwnerCommand
        : IRequest<ApiResponse<bool>>, IAutoTenantRequest
    {
        public Guid Id { get; init; }
        public Guid CompanyId { get; private set; }

        public string FullName { get; init; } = string.Empty;
        public string Phone { get; init; } = string.Empty;
        public string? Email { get; init; }
        public string? NationalId { get; init; }
        public string? Nationality { get; init; }
        public OwnerType OwnerType { get; init; }
        public string? CompanyName { get; init; }
        public string? IBAN { get; init; }
        public decimal? CommissionRate { get; init; }
        public bool IsActive { get; init; }
        public string? Notes { get; init; }

        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
