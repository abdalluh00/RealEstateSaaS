using MediatR;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, ApiResponse<bool>>
    {
        private readonly IUserRepository _repo;

        public UpdateUserHandler(IUserRepository repo) => _repo = repo;

        public async Task<ApiResponse<bool>> Handle(
            UpdateUserCommand request,
            CancellationToken ct)
        {
            var user = await _repo.GetByIdAsync(request.Id);

            if (user is null)
                throw new NotFoundException("المستخدم", request.Id);

            user.FullName = request.FullName;
            user.Phone = request.Phone;
            user.Role = request.Role;
            user.IsActive = request.IsActive;

            _repo.Update(user);
            await _repo.SaveChangesAsync();

            return ApiResponse<bool>.Ok(true, "تم تحديث بيانات المستخدم بنجاح");
        }
    }
}
