using MediatR;
using RealEstate.Application.DTOs.Cheques;
using RealEstate.Application.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Cheques.Queries.GetChequeDetail
{
    public sealed class GetChequeDetailQueryHandler
        : IRequestHandler<
            GetChequeDetailQuery,
            ApiResponse<ChequeDetailDto>>
    {
        private readonly IChequeRepository _cheques;

        public GetChequeDetailQueryHandler(
            IChequeRepository cheques)
        {
            _cheques = cheques;
        }

        public async Task<ApiResponse<ChequeDetailDto>> Handle(
            GetChequeDetailQuery query,
            CancellationToken ct)
        {
            var cheque = await _cheques.GetDetailByIdAsync(
                query.Id,
                query.CompanyId,
                ct);

            if (cheque is null)
                throw new NotFoundException("الشيك غير موجود");

            return ApiResponse<ChequeDetailDto>.Ok(cheque);
        }
    }
}