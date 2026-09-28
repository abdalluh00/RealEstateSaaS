using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Properties.Apartments.Commands.DeleteApartment
{
    public sealed class DeleteApartmentCommandHandler
        : IRequestHandler<DeleteApartmentCommand, ApiResponse<bool>>
    {
        private readonly IApartmentRepository _apartments;
        private readonly IUnitOfWork _uow;

        public DeleteApartmentCommandHandler(
            IApartmentRepository apartments,
            IUnitOfWork uow)
        {
            _apartments = apartments;
            _uow = uow;
        }

        public async Task<ApiResponse<bool>> Handle(
            DeleteApartmentCommand cmd,
            CancellationToken ct)
        {
            // ── Load tracked via Query() ──────────────────
            var apartment = await _apartments.Query()
                .FirstOrDefaultAsync(x => x.Id == cmd.Id
                                       && x.CompanyId == cmd.CompanyId, ct);

            if (apartment is null)
                throw new NotFoundException("الشقة", cmd.Id);

            _apartments.SoftDelete(apartment);
            await _uow.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم حذف الشقة بنجاح");
        }
    }
}
