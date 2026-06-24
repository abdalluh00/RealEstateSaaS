using MediatR;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
namespace RealEstate.Application.Features.Clients.Commands.DeleteClient
{
    public class DeleteClientHandler : IRequestHandler<DeleteClientCommand, ApiResponse<bool>>
    {
        private readonly IClientRepository _repo;

        public DeleteClientHandler(IClientRepository repo) => _repo = repo;

        public async Task<ApiResponse<bool>> Handle(
            DeleteClientCommand request,
            CancellationToken ct)
        {
            var client = await _repo.GetByIdAsync(request.Id);

            if (client is null)
                throw new NotFoundException("العميل", request.Id);

            client.IsDeleted = true;
            _repo.Update(client);
            await _repo.SaveChangesAsync();

            return ApiResponse<bool>.Ok(true, "تم حذف العميل بنجاح");
        }
    }
}
