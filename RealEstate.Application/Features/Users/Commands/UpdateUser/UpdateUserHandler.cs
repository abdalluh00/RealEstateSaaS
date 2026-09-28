using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Users.Commands.UpdateUser
{
    public sealed class UpdateUserCommandHandler
        : IRequestHandler<UpdateUserCommand, ApiResponse<bool>>
    {
        private readonly IUserRepository _users;
        private readonly IUnitOfWork _uow;

        public UpdateUserCommandHandler(IUserRepository users, IUnitOfWork uow)
        {
            _users = users;
            _uow = uow;
        }

        public async Task<ApiResponse<bool>> Handle(
            UpdateUserCommand cmd,
            CancellationToken ct)
        {
            var user = await _users.Query()
                .FirstOrDefaultAsync(u => u.Id == cmd.Id
                                       && u.CompanyId == cmd.CompanyId, ct);

            if (user is null)
                throw new NotFoundException("المستخدم", cmd.Id);

            // ── Cannot change Owner role ───────────────────
            if (user.Role == UserRole.Owner)
                throw new ForbiddenException("لا يمكن تعديل صلاحيات المالك");

            user.FullName = cmd.FullName;
            user.Phone = cmd.Phone;
            user.Role = cmd.Role;

            await _uow.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم تحديث المستخدم بنجاح");
        }
    }
}