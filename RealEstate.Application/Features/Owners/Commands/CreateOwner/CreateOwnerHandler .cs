using MediatR;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
namespace RealEstate.Application.Features.Owners.Commands.CreateOwner
{
    public sealed class CreateOwnerCommandHandler
         : IRequestHandler<CreateOwnerCommand, ApiResponse<Guid>>
    {
        private readonly IOwnerRepository _owners;
        private readonly IUnitOfWork _uow;

        public CreateOwnerCommandHandler(IOwnerRepository owners, IUnitOfWork uow)
        {
            _owners = owners;
            _uow = uow;
        }

        public async Task<ApiResponse<Guid>> Handle(
            CreateOwnerCommand cmd,
            CancellationToken ct)
        {
            // ── Phone must be unique per company ──────────
            var phoneExists = await _owners.PhoneExistsAsync(
                cmd.Phone, cmd.CompanyId, ct);

            if (phoneExists)
                throw new ConflictException("رقم الجوال مستخدم بالفعل");

            var owner = new Owner
            {
                FullName = cmd.FullName,
                Phone = cmd.Phone,
                Email = cmd.Email,
                NationalId = cmd.NationalId,
                Nationality = cmd.Nationality,
                OwnerType = cmd.OwnerType,
                CompanyName = cmd.CompanyName,
                IBAN = cmd.IBAN,
                CommissionRate = cmd.CommissionRate,
                Notes = cmd.Notes,
                CompanyId = cmd.CompanyId,
                IsActive = true
            };

            _owners.Add(owner);
            await _uow.SaveChangesAsync(ct);

            return ApiResponse<Guid>.Ok(owner.Id, "تم إضافة المالك بنجاح");
        }
    }
}
