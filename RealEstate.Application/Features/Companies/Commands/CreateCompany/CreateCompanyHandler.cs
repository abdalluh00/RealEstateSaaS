
using MediatR;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Companies.Commands.CreateCompany
{
    public class CreateCompanyHandler : IRequestHandler<CreateCompanyCommand, ApiResponse<Guid>>
    {
        private readonly ICompanyRepository _repo;

        public CreateCompanyHandler(ICompanyRepository repo) => _repo = repo;

        public async Task<ApiResponse<Guid>> Handle(
            CreateCompanyCommand request,
            CancellationToken ct)
        {
            var phoneExists = await _repo.PhoneExistsAsync(request.Phone);
            if (phoneExists)
                throw new ConflictException("رقم الجوال مسجل مسبقاً");

            var company = new Company
            {
                Name = request.Name,
                Phone = request.Phone,
                Address = request.Address,
                Logo = request.Logo,
                SubscriptionPlan = request.SubscriptionPlan,
                SubscriptionExpiry = request.SubscriptionExpiry,
                IsActive = true
            };

            await _repo.AddAsync(company);
            await _repo.SaveChangesAsync();

            return ApiResponse<Guid>.Ok(company.Id, "تم إنشاء الشركة بنجاح");
        }
    }
}
