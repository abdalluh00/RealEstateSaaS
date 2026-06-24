using MediatR;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Owners.Commands.DeleteOwner
{
    public class DeleteOwnerHandler : IRequestHandler<DeleteOwnerCommand, ApiResponse<bool>>
    {
        private readonly IOwnerRepository _repo;
        private readonly IPropertyRepository _property;

        public DeleteOwnerHandler(IOwnerRepository repo, IPropertyRepository property)
        {
            _repo = repo;
            _property = property;       
        }

        public async Task<ApiResponse<bool>> Handle(
            DeleteOwnerCommand request,
            CancellationToken ct)
        {
            bool IsExist = await _property.IsOwnerHasPropertyExistAsync(request.Id);
            var owner = await _repo.GetByIdAsync(request.Id);
            if (owner is null)
                throw new NotFoundException("المالك", request.Id);

            if (IsExist)
                throw new ConflictException("لا يمكن حذف مالك عنده عقارات مسجلة");

            owner.IsDeleted = true;
            _repo.Update(owner);
            await _repo.SaveChangesAsync();

            return ApiResponse<bool>.Ok(true, "تم حذف المالك بنجاح");
        }
    }
}
