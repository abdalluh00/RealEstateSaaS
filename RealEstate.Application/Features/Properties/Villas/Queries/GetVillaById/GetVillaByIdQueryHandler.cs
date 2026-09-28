using MediatR;
using RealEstate.Application.DTOs.Properties.Villa;
using RealEstate.Application.Features.Properties.Villas.Queries.GetVillaById;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Properties.Villas.Commands.DeleteVilla
{
    public sealed class GetVillaDetailsQueryHandler : IRequestHandler<GetVillaByIdQuery, ApiResponse<VillaDetailDto>>
    {
        public readonly IVillaRepository _villaRepository;

        public GetVillaDetailsQueryHandler(IVillaRepository villaRepository)
        {
            _villaRepository = villaRepository;
        }
        public async Task<ApiResponse<VillaDetailDto>> Handle(GetVillaByIdQuery request, CancellationToken cancellationToken)
        {
            var villa = await _villaRepository.GetDetailByIdAsync(request.Id, request.CompanyId, cancellationToken);

            if(villa == null)
                throw new NotFoundException("الفيلا غير موجودة", request.Id);

            return ApiResponse<VillaDetailDto>.Ok(villa, "تم استرجاع بيانات الفيلا بنجاح");
        }
    }
}