using MediatR;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Companies.Commands.UpdateCompany
{
    public sealed class UpdateCompanyCommandHandler
        : IRequestHandler<UpdateCompanyCommand, ApiResponse<bool>>
    {
        private readonly ICompanyRepository _companies;
        private readonly IUnitOfWork _uow;

        public UpdateCompanyCommandHandler(
            ICompanyRepository companies,
            IUnitOfWork uow)
        {
            _companies = companies;
            _uow = uow;
        }

        public async Task<ApiResponse<bool>> Handle(
            UpdateCompanyCommand cmd,
            CancellationToken ct)
        {
            var company = await _companies.GetByIdForCommandAsync(
                cmd.CompanyId, ct);

            if (company is null)
                throw new NotFoundException("الشركة", cmd.CompanyId);

            company.Name = cmd.Name;
            company.Logo = cmd.Logo;
            company.Address = cmd.Address;
            company.Phone = cmd.Phone;

            await _uow.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم تحديث بيانات الشركة بنجاح");
        }
    }
}