using MediatR;
using RealEstate.Application.Features.Owners.DTO;
using RealEstate.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Owners.Queries.GetOwnerById
{
    public record GetOwnerByIdQuery(Guid Id) : IRequest<ApiResponse<OwnerDetailDto>>;

}
