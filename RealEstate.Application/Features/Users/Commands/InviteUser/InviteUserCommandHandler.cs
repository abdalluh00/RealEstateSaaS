using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Users.Commands.InviteUser
{
    public sealed class InviteUserCommandHandler
        : IRequestHandler<InviteUserCommand, ApiResponse<Guid>>
    {
        private readonly IUserRepository _users;
        private readonly IUnitOfWork _uow;
        private readonly IWhatsAppService _whatsApp;

        public InviteUserCommandHandler(
            IUserRepository users,
            IUnitOfWork uow,
            IWhatsAppService whatsApp)
        {
            _users = users;
            _uow = uow;
            _whatsApp = whatsApp;
        }

        public async Task<ApiResponse<Guid>> Handle(
            InviteUserCommand cmd,
            CancellationToken ct)
        {
            // ── Email must be unique globally ─────────────
            var emailExists = await _users.EmailExistsAsync(cmd.Email, ct);

            if (emailExists)
                throw new ConflictException("البريد الإلكتروني مستخدم بالفعل");

            var token = Guid.NewGuid().ToString("N"); // clean token no dashes

            var user = new User
            {
                FullName = cmd.FullName,
                Email = cmd.Email,
                Phone = cmd.Phone,
                Role = cmd.Role,
                CompanyId = cmd.CompanyId,
                IsActive = false,            // active after accepting
                IsInvitationAccepted = false,
                InvitationToken = token,
                InvitationExpiry = DateTime.UtcNow.AddHours(48),
                PasswordHash = string.Empty      // set on AcceptInvitation
            };

            _users.Add(user);
            await _uow.SaveChangesAsync(ct);

            // ── Send invitation via WhatsApp ───────────────
            await _whatsApp.SendAsync(
                cmd.Phone,
                $"""
                مرحباً {cmd.FullName} 👋

                تمت دعوتك للانضمام إلى النظام العقاري.
                رمز الدعوة: {token}
                صالح لمدة 48 ساعة.

                يرجى تفعيل حسابك وتعيين كلمة المرور.
                """);

            return ApiResponse<Guid>.Ok(user.Id, "تم إرسال الدعوة بنجاح");
        }
    }
}