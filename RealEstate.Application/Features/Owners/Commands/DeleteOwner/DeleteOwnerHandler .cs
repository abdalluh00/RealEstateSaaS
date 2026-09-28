using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Owners.Commands.DeleteOwner
{
    public sealed class DeleteOwnerCommandHandler
        : IRequestHandler<DeleteOwnerCommand, ApiResponse<bool>>
    {
        private readonly IOwnerRepository _owners;
        private readonly IPropertyRepository _properties;
        private readonly IUnitOfWork _uow;

        public DeleteOwnerCommandHandler(
            IOwnerRepository owners,
            IPropertyRepository properties,
            IUnitOfWork uow)
        {
            _owners = owners;
            _properties = properties;
            _uow = uow;
        }

        public async Task<ApiResponse<bool>> Handle(
            DeleteOwnerCommand cmd,
            CancellationToken ct)
        {
            var owner = await _owners.Query()
                .FirstOrDefaultAsync(x => x.Id == cmd.Id
                                       && x.CompanyId == cmd.CompanyId, ct);

            if (owner is null)
                throw new NotFoundException("المالك", cmd.Id);

            // ── Cannot delete owner with active properties ─
            var hasProperties = await _properties.IsOwnerLinkedToAnyPropertyAsync(
                cmd.Id, cmd.CompanyId, ct);

            if (hasProperties)
                throw new ConflictException(
                    "لا يمكن حذف المالك لأن لديه عقارات مرتبطة به");

            _owners.SoftDelete(owner);
            await _uow.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم حذف المالك بنجاح");
        }
    }
}
