using MediatR;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
using RealEstate.Shared.Helpers;

namespace RealEstate.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, ApiResponse<Guid>>
    {
        private readonly IUserRepository _repo;

        public CreateUserHandler(IUserRepository repo) => _repo = repo;

        public async Task<ApiResponse<Guid>> Handle(
            CreateUserCommand request,
            CancellationToken ct)
        {
            var emailExists = await _repo.EmailExistsAsync(request.Email);
            if (emailExists)
                throw new ConflictException("البريد الإلكتروني مسجل مسبقاً");

            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                Phone = request.Phone,
                PasswordHash = PasswordHelper.Hash(request.Password),
                Role = request.Role,
                CompanyId = request.CompanyId,
                IsActive = true
            };

            await _repo.AddAsync(user);
            await _repo.SaveChangesAsync();

            return ApiResponse<Guid>.Ok(user.Id, "تم إنشاء المستخدم بنجاح");
        }
    }
}
