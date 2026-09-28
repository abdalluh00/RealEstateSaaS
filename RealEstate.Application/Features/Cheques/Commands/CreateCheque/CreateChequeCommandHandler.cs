using MediatR;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Cheques.Commands.CreateCheque
{
    public sealed class CreateChequeCommandHandler
        : IRequestHandler<CreateChequeCommand, ApiResponse<Guid>>
    {
        private readonly IChequeRepository _cheques;
        private readonly IContractRepository _contracts;
        private readonly IUnitOfWork _uow;

        public CreateChequeCommandHandler(
            IChequeRepository cheques,
            IContractRepository contracts,
            IUnitOfWork uow)
        {
            _cheques = cheques;
            _contracts = contracts;
            _uow = uow;
        }

        public async Task<ApiResponse<Guid>> Handle(
            CreateChequeCommand cmd,
            CancellationToken ct)
        {
            // ── Validate contract ─────────────────────────

            var contract = await _contracts.GetByIdForCommandAsync(
                cmd.ContractId,
                cmd.CompanyId,
                ct);

            if (contract is null)
                throw new NotFoundException("العقد غير موجود");

            // ── Validate cheque number ─────────────────────

            var chequeNumberExists =
                await _cheques.ChequeNumberExistsAsync(
                    cmd.ChequeNumber,
                    cmd.CompanyId,
                    ct: ct);

            if (chequeNumberExists)
                throw new ConflictException(
                    "رقم الشيك مستخدم مسبقاً");

            // ── Create ────────────────────────────────────

            var cheque = new Cheque
            {
                ChequeNumber = cmd.ChequeNumber,
                BankName = cmd.BankName,

                Amount = cmd.Amount,
                DueDate = cmd.DueDate,

                ChequeOrder = cmd.ChequeOrder,

                ContractId = cmd.ContractId,
                CompanyId = cmd.CompanyId,

                Notes = cmd.Notes,

                Status = ChequeStatus.Pending
            };

            _cheques.Add(cheque);

            await _uow.SaveChangesAsync(ct);

            return ApiResponse<Guid>.Ok(
                cheque.Id,
                "تم إنشاء الشيك بنجاح");
        }
    }
}