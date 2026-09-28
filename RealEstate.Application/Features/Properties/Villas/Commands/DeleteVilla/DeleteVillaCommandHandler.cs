namespace RealEstate.Application.Features.Properties.Villas.Commands.DeleteVilla
{
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using RealEstate.Application.Interfaces.Properties;
    using RealEstate.Domain.Interfaces;
    using RealEstate.Shared.Common;
    using RealEstate.Shared.Common.Exceptions;
    public sealed class DeleteVillaCommandHandler
        : IRequestHandler<DeleteVillaCommand, ApiResponse<bool>>
    {
        private readonly IVillaRepository _villas;
        private readonly IUnitOfWork _uow;
        public DeleteVillaCommandHandler(IVillaRepository villas, IUnitOfWork uow)
        {
            _villas = villas;
            _uow = uow;
        }
        public async Task<ApiResponse<bool>> Handle(
            DeleteVillaCommand cmd,
            CancellationToken ct)
        {
            var villa = await _villas.Query()
                .FirstOrDefaultAsync(x => x.Id == cmd.Id
                                       && x.CompanyId == cmd.CompanyId, ct);
            if (villa is null)
                throw new NotFoundException("الفيلا", cmd.Id);
            _villas.SoftDelete(villa);
            await _uow.SaveChangesAsync(ct);
            return ApiResponse<bool>.Ok(true, "تم حذف الفيلا بنجاح");
        }
    }
}