using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Cheques.Commands.CancelCheque
{
    public sealed class CancelChequeCommandHandler
        : IRequestHandler<CancelChequeCommand, ApiResponse<Guid>>
    {
        private readonly IChequeRepository _cheques;
        private readonly IUnitOfWork _uow;

        public CancelChequeCommandHandler(
            IChequeRepository cheques,
            IUnitOfWork uow)
        {
            _cheques = cheques;
            _uow = uow;
        }

        public async Task<ApiResponse<Guid>> Handle(
            CancelChequeCommand cmd,
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

            if (cheque.Status == ChequeStatus.Cleared)
                throw new ConflictException(
                    "لا يمكن إلغاء شيك تم تحصيله");

            if (cheque.Status == ChequeStatus.Bounced)
                throw new ConflictException(
                    "الشيك المرتجع لا يمكن إلغاؤه");

            if (cheque.Status == ChequeStatus.Cancelled)
                throw new ConflictException(
                    "الشيك ملغي بالفعل");

            cheque.Status = ChequeStatus.Cancelled;
            cheque.CancelledAt = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(cmd.Reason))
                cheque.Notes = cmd.Reason;

            _cheques.Update(cheque);

            await _uow.SaveChangesAsync(ct);

            return ApiResponse<Guid>.Ok(
                cheque.Id,
                "تم إلغاء الشيك بنجاح");
        }
    }
}