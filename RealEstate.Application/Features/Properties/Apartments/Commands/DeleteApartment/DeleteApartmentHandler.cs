using MediatR;
using RealEstate.Domain.Interfaces;
using RealEstate.Domain.Interfaces.Properties.RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Properties.Apartments.Commands.DeleteApartment
{
    public class DeleteApartmentHandler : IRequestHandler<DeleteApartmentCommand, ApiResponse<Guid>>
    {
        private readonly IApartmentRepository _apartmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteApartmentHandler(
            IApartmentRepository apartmentRepository,
            IUnitOfWork unitOfWork)
        {
            _apartmentRepository = apartmentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<Guid>> Handle(DeleteApartmentCommand request, CancellationToken ct)
        {
            var apartment = await _apartmentRepository.GetByIdAsync(request.ApartmentId, request.CompanyId);
            if (apartment is null)
                throw new NotFoundException("الشقة غير موجودة");

            apartment.IsDeleted = true;
            _apartmentRepository.Update(apartment);

            await _unitOfWork.SaveChangesAsync(ct);

            return ApiResponse<Guid>.Ok(apartment.Id, "تم حذف الشقة بنجاح");
        }
    }
}
