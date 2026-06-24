using MediatR;
using RealEstate.Application.Features.Clients.DTO;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Clients.Queries.GetClientById
{
    public class GetClientByIdHandler : IRequestHandler<GetClientByIdQuery, ApiResponse<ClientDetailDto>>
    {
        private readonly IClientRepository _repo;

        public GetClientByIdHandler(IClientRepository repo) => _repo = repo;

        public async Task<ApiResponse<ClientDetailDto>> Handle(
            GetClientByIdQuery request,
            CancellationToken ct)
        {
            var client = await _repo.GetByIdAsync(request.Id);

            if (client is null)
                throw new NotFoundException("العميل", request.Id);

            var result = new ClientDetailDto(
                client.Id,
                client.FullName,
                client.Phone,
                client.Email,
                client.LeadStatus,
                client.Source,
                client.Notes,
                client.CreatedAt
            );

            return ApiResponse<ClientDetailDto>.Ok(result);
        }
    }
}
