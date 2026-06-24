using MediatR;
using RealEstate.Application.Features.Companies.DTO;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Companies.Queries.GetCompanies
{
    public record GetCompaniesQuery : IRequest<ApiResponse<List<CompanyDto>>>;

   
}
