using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Clients.Commands.ReassignAgent
{
    public sealed class ReassignAgentCommandHandler
        : IRequestHandler<ReassignAgentCommand, ApiResponse<bool>>
    {
        private readonly IClientRepository _clients;
        private readonly IUnitOfWork _uow;

        public ReassignAgentCommandHandler(
            IClientRepository clients,
            IUnitOfWork uow)
        {
            _clients = clients;
            _uow = uow;
        }

        public async Task<ApiResponse<bool>> Handle(
            ReassignAgentCommand cmd,
            CancellationToken ct)
        {
            var client = await _clients.Query()
                .FirstOrDefaultAsync(x => x.Id == cmd.Id
                                       && x.CompanyId == cmd.CompanyId, ct);

            if (client is null)
                throw new NotFoundException("العميل", cmd.Id);

            client.AssignedAgentId = cmd.NewAgentId;

            await _uow.SaveChangesAsync(ct);

            var message = cmd.NewAgentId.HasValue
                ? "تم إعادة تعيين الوكيل بنجاح"
                : "تم إلغاء تعيين الوكيل بنجاح";

            return ApiResponse<bool>.Ok(true, message);
        }
    }
}