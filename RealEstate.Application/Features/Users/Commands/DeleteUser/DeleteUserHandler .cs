using MediatR;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
namespace RealEstate.Application.Features.Users.Commands.DeleteUser
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, ApiResponse<bool>>
    {
        private readonly IUserRepository _repo;

        public DeleteUserHandler(IUserRepository repo) => _repo = repo;

        public async Task<ApiResponse<bool>> Handle(
            DeleteUserCommand request,
            CancellationToken ct)
        {
            var user = await _repo.GetByIdAsync(request.Id);

            if (user is null)
                throw new NotFoundException("المستخدم", request.Id);

            user.IsDeleted = true;
            user.IsActive = false;

            _repo.Update(user);
            await _repo.SaveChangesAsync();

            return ApiResponse<bool>.Ok(true, "تم حذف المستخدم بنجاح");
        }
    }
}
