using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Clients.Commands.UpdateClient
{
    public sealed class UpdateClientCommandHandler
        : IRequestHandler<UpdateClientCommand, ApiResponse<bool>>
    {
        private readonly IClientRepository _clients;
        private readonly IUnitOfWork _uow;

        public UpdateClientCommandHandler(
            IClientRepository clients,
            IUnitOfWork uow)
        {
            _clients = clients;
            _uow = uow;
        }

        public async Task<ApiResponse<bool>> Handle(
            UpdateClientCommand cmd,
            CancellationToken ct)
        {
            var client = await _clients.Query()
                .FirstOrDefaultAsync(x => x.Id == cmd.Id
                                       && x.CompanyId == cmd.CompanyId, ct);

            if (client is null)
                throw new NotFoundException("العميل", cmd.Id);

            var phoneTaken = await _clients.PhoneExistsForAnotherClientAsync(
                cmd.Phone, cmd.Id, cmd.CompanyId, ct);

            if (phoneTaken)
                throw new ConflictException("رقم الجوال مستخدم بالفعل");

            client.FullName = cmd.FullName;
            client.Phone = cmd.Phone;
            client.Email = cmd.Email;
            client.NationalId = cmd.NationalId;
            client.Nationality = cmd.Nationality;
            client.Source =(LeadSource) cmd.Source!;
            client.IsActive = cmd.IsActive;
            client.Notes = cmd.Notes;

            await _uow.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم تحديث العميل بنجاح");
        }
    }
}