using MediatR;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Clients.Commands.CreateClient
{
    public sealed class CreateClientCommandHandler
        : IRequestHandler<CreateClientCommand, ApiResponse<Guid>>
    {
        private readonly IClientRepository _clients;
        private readonly IUnitOfWork _uow;

        public CreateClientCommandHandler(
            IClientRepository clients,
            IUnitOfWork uow)
        {
            _clients = clients;
            _uow = uow;
        }

        public async Task<ApiResponse<Guid>> Handle(
            CreateClientCommand cmd,
            CancellationToken ct)
        {
            var phoneExists = await _clients.PhoneExistsAsync(
                cmd.Phone, cmd.CompanyId, ct);

            if (phoneExists)
                throw new ConflictException("رقم الجوال مستخدم بالفعل");

            var client = new Client
            {
                FullName = cmd.FullName,
                Phone = cmd.Phone,
                Email = cmd.Email,
                NationalId = cmd.NationalId,
                Nationality = cmd.Nationality,
                Source =(LeadSource) cmd.Source!,
                AssignedAgentId = cmd.AssignedAgentId,
                Notes = cmd.Notes,
                CompanyId = cmd.CompanyId,
                LeadStatus = LeadStatus.Lead,
                IsActive = true
            };

            _clients.Add(client);
            await _uow.SaveChangesAsync(ct);

            return ApiResponse<Guid>.Ok(client.Id, "تم إضافة العميل بنجاح");
        }
    }
}