using MediatR;
using Microsoft.AspNetCore.Authorization;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Companies.Commands.DeleteCompany
{
    // فقط Owner يقدر يحذف شركة
    [Authorize(Roles = "Owner")]
    public record DeleteCompanyCommand(Guid Id) : IRequest<ApiResponse<bool>>;
}
