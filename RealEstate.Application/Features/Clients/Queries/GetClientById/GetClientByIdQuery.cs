using MediatR;
using RealEstate.Application.Features.Clients.DTO;
using RealEstate.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Clients.Queries.GetClientById
{
    public record GetClientByIdQuery(Guid Id) : IRequest<ApiResponse<ClientDetailDto>>;

    
}
