using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Cheques.Commands.ClearCheque
{
    public sealed class ClearChequeCommandHandler
        : IRequestHandler<ClearChequeCommand, ApiResponse<Guid>>
    {
        private readonly IChequeRepository _cheques;
        private readonly IUnitOfWork _uow;

        public ClearChequeCommandHandler(
            IChequeRepository cheques,
            IUnitOfWork uow)
        {
            _cheques = cheques;
            _uow = uow;
        }

        public async Task<ApiResponse<Guid>> Handle(
            ClearChequeCommand cmd,
            CancellationToken ct)
        {
            var cheque = await _cheques
                .Query()
                .FirstOrDefaultAsync(
                    c => c.Id == cmd.Id &&
                         c.CompanyId == cmd.CompanyId,
                    ct);

            if (cheque is null)
                throw new NotFoundException("الشيك غير موجود");

            if (cheque.Status != ChequeStatus.Deposited)
                throw new ConflictException(
                    "لا يمكن تحصيل الشيك إلا بعد إيداعه");

            cheque.Status = ChequeStatus.Cleared;
            cheque.ClearedAt = DateTime.UtcNow;

            _cheques.Update(cheque);

            await _uow.SaveChangesAsync(ct);

            return ApiResponse<Guid>.Ok(
                cheque.Id,
                "تم تحصيل الشيك بنجاح");
        }
    }
}