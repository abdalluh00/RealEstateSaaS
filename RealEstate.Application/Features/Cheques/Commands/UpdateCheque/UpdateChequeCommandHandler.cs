using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Cheques.Commands.UpdateCheque
{
    public sealed class UpdateChequeCommandHandler
        : IRequestHandler<UpdateChequeCommand, ApiResponse<Guid>>
    {
        private readonly IChequeRepository _cheques;
        private readonly IUnitOfWork _uow;

        public UpdateChequeCommandHandler(
            IChequeRepository cheques,
            IUnitOfWork uow)
        {
            _cheques = cheques;
            _uow = uow;
        }

        public async Task<ApiResponse<Guid>> Handle(
            UpdateChequeCommand cmd,
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

            // Don't allow editing completed financial states.
            if (cheque.Status != ChequeStatus.Pending)
                throw new ConflictException(
                    "لا يمكن تعديل الشيك بعد إيداعه");

            var numberExists =
                await _cheques.ChequeNumberExistsAsync(
                    cmd.ChequeNumber,
                    cmd.CompanyId,
                    cmd.Id,
                    ct);

            if (numberExists)
                throw new ConflictException(
                    "رقم الشيك مستخدم مسبقاً");

            cheque.ChequeNumber = cmd.ChequeNumber;
            cheque.BankName = cmd.BankName;

            cheque.Amount = cmd.Amount;
            cheque.DueDate = cmd.DueDate;

            cheque.ChequeOrder = cmd.ChequeOrder;

            cheque.Notes = cmd.Notes;

            _cheques.Update(cheque);

            await _uow.SaveChangesAsync(ct);

            return ApiResponse<Guid>.Ok(
                cheque.Id,
                "تم تحديث الشيك بنجاح");
        }
    }
}