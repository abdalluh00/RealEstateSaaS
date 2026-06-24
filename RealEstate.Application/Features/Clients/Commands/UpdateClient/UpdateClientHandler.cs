using MediatR;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Clients.Commands.UpdateClient
{
    public class UpdateClientHandler : IRequestHandler<UpdateClientCommand, ApiResponse<bool>>
    {
        private readonly IClientRepository _repo;

        public UpdateClientHandler(IClientRepository repo) => _repo = repo;

        public async Task<ApiResponse<bool>> Handle(
            UpdateClientCommand request,
            CancellationToken ct)
        {
            var client = await _repo.GetByIdAsync(request.Id);

            if (client is null)
                throw new NotFoundException("العميل", request.Id);

            client.FullName = request.FullName;
            client.Phone = request.Phone;
            client.Email = request.Email;
            client.LeadStatus = request.LeadStatus;
            client.Source = request.Source;
            client.Notes = request.Notes;

            _repo.Update(client);
            await _repo.SaveChangesAsync();

            return ApiResponse<bool>.Ok(true, "تم تحديث العميل بنجاح");
        }
    }
}
