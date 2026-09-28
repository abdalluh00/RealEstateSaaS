using MediatR;
using RealEstate.Application.DTOs.Clients;
using RealEstate.Application.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Clients.Queries.GetClientDetail
{
    public sealed class GetClientDetailQueryHandler
        : IRequestHandler<GetClientDetailQuery, ApiResponse<ClientDetailDto>>
    {
        private readonly IClientRepository _clients;

        public GetClientDetailQueryHandler(IClientRepository clients)
            => _clients = clients;

        public async Task<ApiResponse<ClientDetailDto>> Handle(
            GetClientDetailQuery query,
            CancellationToken ct)
        {
            var client = await _clients.GetDetailByIdAsync(
                query.Id, query.CompanyId, ct);

            if (client is null)
                throw new NotFoundException("العميل", query.Id);

            return ApiResponse<ClientDetailDto>.Ok(client);
        }
    }
}