using MediatR;
using RealEstate.Application.Features.Companies.DTO;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Companies.Queries.GetCompanyById
{
    public record GetCompanyByIdQuery(Guid Id) : IRequest<ApiResponse<CompanyDetailDto>>;

    
}
