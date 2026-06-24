
using MediatR;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Companies.Commands.UpdateCompany
{
    public class UpdateCompanyMeHandler
        : IRequestHandler<UpdateCompanyMeCommand, ApiResponse<bool>>
    {
        private readonly ICompanyRepository _repo;

        public UpdateCompanyMeHandler(ICompanyRepository repo) => _repo = repo;

        public async Task<ApiResponse<bool>> Handle(
            UpdateCompanyMeCommand request,
            CancellationToken ct)
        {
            var company = await _repo.GetByIdAsync(request.Id);
            if (company is null)
                throw new NotFoundException("الشركة", request.Id);

            company.Name = request.Name;
            company.Phone = request.Phone;
            company.Address = request.Address;
            company.Logo = request.Logo;

            _repo.Update(company);
            await _repo.SaveChangesAsync();

            return ApiResponse<bool>.Ok(true, "تم تحديث بيانات الشركة بنجاح");
        }
    }
}
