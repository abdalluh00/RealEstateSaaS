using MediatR;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
namespace RealEstate.Application.Features.Owners.Commands.UpdateOwner
{
    public class UpdateOwnerHandler : IRequestHandler<UpdateOwnerCommand, ApiResponse<bool>>
    {
        private readonly IOwnerRepository _repo;

        public UpdateOwnerHandler(IOwnerRepository repo) => _repo = repo;

        public async Task<ApiResponse<bool>> Handle(
            UpdateOwnerCommand request,
            CancellationToken ct)
        {
            var owner = await _repo.GetByIdAsync(request.Id);

            if (owner is null)
                throw new NotFoundException("المالك", request.Id);

            owner.FullName = request.FullName;
            owner.Phone = request.Phone;
            owner.Email = request.Email;
            owner.NationalId = request.IdNumber;
            owner.Notes = request.Notes;

            _repo.Update(owner);
            await _repo.SaveChangesAsync();

            return ApiResponse<bool>.Ok(true, "تم تحديث بيانات المالك بنجاح");
        }
    }
}
