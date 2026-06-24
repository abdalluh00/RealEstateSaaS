using MediatR;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
namespace RealEstate.Application.Features.Owners.Commands.CreateOwner
{
    public class CreateOwnerHandler : IRequestHandler<CreateOwnerCommand, ApiResponse<Guid>>
    {
        private readonly IOwnerRepository _repo;

        public CreateOwnerHandler(IOwnerRepository repo) => _repo = repo;

        public async Task<ApiResponse<Guid>> Handle(
            CreateOwnerCommand request,
            CancellationToken ct)
        {
            var phoneExists = await _repo.PhoneExistsAsync(request.CompanyId, request.Phone);
            if (phoneExists)
                throw new ConflictException("رقم الجوال مسجل مسبقاً");

            var owner = new Owner
            {
                FullName = request.FullName,
                Phone = request.Phone,
                Email = request.Email,
                NationalId = request.IdNumber,
                Notes = request.Notes,
                CompanyId = request.CompanyId
            };

            await _repo.AddAsync(owner);
            await _repo.SaveChangesAsync();

            return ApiResponse<Guid>.Ok(owner.Id, "تم إضافة المالك بنجاح");
        }
    }
}
