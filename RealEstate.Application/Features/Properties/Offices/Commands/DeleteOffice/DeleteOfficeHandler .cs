using MediatR;
using RealEstate.Domain.Interfaces;
using RealEstate.Domain.Interfaces.Properties;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;


namespace RealEstate.Application.Features.Properties.Offices.Commands.DeleteOffice
{
    public class DeleteOfficeHandler : IRequestHandler<DeleteOfficeCommand, ApiResponse<bool>>
    {
        private readonly IOfficeRepository _officeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteOfficeHandler(
            IOfficeRepository officeRepository,
            IUnitOfWork unitOfWork)
        {
            _officeRepository = officeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<bool>> Handle(DeleteOfficeCommand request, CancellationToken ct)
        {
            var office = await _officeRepository.GetByIdForCompanyAsync(request.Id, request.CompanyId, ct);
            if (office is null)
                throw new NotFoundException("المكتب غير موجود");

            office.IsDeleted = true;

            await _unitOfWork.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم حذف المكتب بنجاح");
        }
    }
}
