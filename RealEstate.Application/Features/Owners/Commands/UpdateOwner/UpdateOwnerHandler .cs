using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
namespace RealEstate.Application.Features.Owners.Commands.UpdateOwner
{
    public sealed class UpdateOwnerCommandHandler
       : IRequestHandler<UpdateOwnerCommand, ApiResponse<bool>>
    {
        private readonly IOwnerRepository _owners;
        private readonly IUnitOfWork _uow;

        public UpdateOwnerCommandHandler(IOwnerRepository owners, IUnitOfWork uow)
        {
            _owners = owners;
            _uow = uow;
        }

        public async Task<ApiResponse<bool>> Handle(
            UpdateOwnerCommand cmd,
            CancellationToken ct)
        {
            var owner = await _owners.Query().FirstOrDefaultAsync(x => x.Id == cmd.Id
                                       && x.CompanyId == cmd.CompanyId, ct);

            if (owner is null)
                throw new NotFoundException("المالك", cmd.Id);

            // ── Phone unique check — exclude current owner ─
            var phoneTaken = await _owners.PhoneExistsForAnotherOwnerAsync(
                cmd.Phone, cmd.Id, cmd.CompanyId, ct);

            if (phoneTaken)
                throw new ConflictException("رقم الجوال مستخدم بالفعل");

            owner.FullName = cmd.FullName;
            owner.Phone = cmd.Phone;
            owner.Email = cmd.Email;
            owner.NationalId = cmd.NationalId;
            owner.Nationality = cmd.Nationality;
            owner.OwnerType = cmd.OwnerType;
            owner.CompanyName = cmd.CompanyName;
            owner.IBAN = cmd.IBAN;
            owner.CommissionRate = cmd.CommissionRate;
            owner.IsActive = cmd.IsActive;
            owner.Notes = cmd.Notes;

            await _uow.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم تحديث المالك بنجاح");
        }
    }
}
