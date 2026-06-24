using MediatR;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
namespace RealEstate.Application.Features.Clients.Commands.CreateClient
{
    public class CreateClientHandler : IRequestHandler<CreateClientCommand, ApiResponse<Guid>>
    {
        private readonly IClientRepository _repo;

        public CreateClientHandler(IClientRepository repo) => _repo = repo;

        public async Task<ApiResponse<Guid>> Handle(
            CreateClientCommand request,
            CancellationToken ct)
        {
            // تحقق من عدم تكرار رقم الجوال
            var phoneExists = await _repo.PhoneExistsAsync(request.CompanyId, request.Phone);
            if (phoneExists)
                throw new ConflictException("رقم الجوال مسجل مسبقاً");

            var client = new Client
            {
                FullName = request.FullName,
                Phone = request.Phone,
                Email = request.Email,
                Source = request.Source,
                Notes = request.Notes,
                CompanyId = request.CompanyId,
                LeadStatus = "New"
            };

            await _repo.AddAsync(client);
            await _repo.SaveChangesAsync();

            return ApiResponse<Guid>.Ok(client.Id, "تم إضافة العميل بنجاح");
        }
    }
}
