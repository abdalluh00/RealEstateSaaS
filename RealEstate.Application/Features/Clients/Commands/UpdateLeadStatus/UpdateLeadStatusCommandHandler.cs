using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Clients.Commands.UpdateLeadStatus
{
    public sealed class UpdateLeadStatusCommandHandler
        : IRequestHandler<UpdateLeadStatusCommand, ApiResponse<bool>>
    {
        private readonly IClientRepository _clients;
        private readonly IUnitOfWork _uow;

        public UpdateLeadStatusCommandHandler(
            IClientRepository clients,
            IUnitOfWork uow)
        {
            _clients = clients;
            _uow = uow;
        }

        public async Task<ApiResponse<bool>> Handle(
            UpdateLeadStatusCommand cmd,
            CancellationToken ct)
        {
            var client = await _clients.Query()
                .FirstOrDefaultAsync(x => x.Id == cmd.Id
                                       && x.CompanyId == cmd.CompanyId, ct);

            if (client is null)
                throw new NotFoundException("العميل", cmd.Id);

            client.LeadStatus = cmd.LeadStatus;

            await _uow.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم تحديث حالة العميل بنجاح");
        }
    }
}