using MediatR;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Companies.Commands.UpdateSubscription
{
    public sealed class UpdateSubscriptionCommandHandler
        : IRequestHandler<UpdateSubscriptionCommand, ApiResponse<bool>>
    {
        private readonly ICompanyRepository _companies;
        private readonly IUnitOfWork _uow;

        public UpdateSubscriptionCommandHandler(
            ICompanyRepository companies,
            IUnitOfWork uow)
        {
            _companies = companies;
            _uow = uow;
        }

        public async Task<ApiResponse<bool>> Handle(
            UpdateSubscriptionCommand cmd,
            CancellationToken ct)
        {
            var company = await _companies.GetByIdForCommandAsync(
                cmd.CompanyId, ct);

            if (company is null)
                throw new NotFoundException("الشركة", cmd.CompanyId);

            company.SubscriptionPlan = cmd.Plan;
            company.SubscriptionExpiry = cmd.NewExpiry;

            await _uow.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم تحديث الاشتراك بنجاح");
        }
    }
}