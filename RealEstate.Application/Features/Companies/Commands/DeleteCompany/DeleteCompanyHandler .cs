using MediatR;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Companies.Commands.DeleteCompany
{
    public class DeleteCompanyHandler : IRequestHandler<DeleteCompanyCommand, ApiResponse<bool>>
    {
        private readonly ICompanyRepository _repo;

        public DeleteCompanyHandler(ICompanyRepository repo) => _repo = repo;

        public async Task<ApiResponse<bool>> Handle(
            DeleteCompanyCommand request,
            CancellationToken ct)
        {
            var company = await _repo.GetByIdAsync(request.Id);

            if (company is null)
                throw new NotFoundException("الشركة", request.Id);

            if (company.Properties?.Any() == true)
                throw new ConflictException("لا يمكن حذف شركة عندها عقارات مسجلة");

            company.IsDeleted = true;
            _repo.Update(company);
            await _repo.SaveChangesAsync();

            return ApiResponse<bool>.Ok(true, "تم حذف الشركة بنجاح");
        }
    }
}
