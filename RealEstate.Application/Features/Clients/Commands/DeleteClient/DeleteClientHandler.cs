using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Clients.Commands.DeleteClient
{
    public sealed class DeleteClientCommandHandler
        : IRequestHandler<DeleteClientCommand, ApiResponse<bool>>
    {
        private readonly IClientRepository _clients;
        private readonly IContractRepository _contracts;
        private readonly IUnitOfWork _uow;

        public DeleteClientCommandHandler(
            IClientRepository clients,
            IContractRepository contracts,
            IUnitOfWork uow)
        {
            _clients = clients;
            _contracts = contracts;
            _uow = uow;
        }

        public async Task<ApiResponse<bool>> Handle(
            DeleteClientCommand cmd,
            CancellationToken ct)
        {
            var client = await _clients.Query()
                .FirstOrDefaultAsync(x => x.Id == cmd.Id
                                       && x.CompanyId == cmd.CompanyId, ct);

            if (client is null)
                throw new NotFoundException("العميل", cmd.Id);

            // ── Cannot delete client with active contracts ─
            var hasActiveContract = await _contracts.AnyAsync(
                c => c.ClientId == cmd.Id
                  && c.ContractStatus == Domain.Common.Enums.ContractStatus.Active, ct);

            if (hasActiveContract)
                throw new ConflictException(
                    "لا يمكن حذف العميل لأن لديه عقد نشط");

            _clients.SoftDelete(client);
            await _uow.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم حذف العميل بنجاح");
        }
    }
}