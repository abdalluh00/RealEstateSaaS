using MediatR;
using RealEstate.Domain.Interfaces;
using RealEstate.Domain.Interfaces.Properties;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Villas.Commands.DeleteVilla
{
    public class DeleteVillaCommandHandler
       : IRequestHandler<DeleteVillaCommand, ApiResponse<bool>>
    {
        private readonly IVillaRepository _villaRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteVillaCommandHandler(
            IVillaRepository villaRepository,
            IUnitOfWork unitOfWork)
        {
            _villaRepository = villaRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<bool>> Handle(DeleteVillaCommand request, CancellationToken ct)
        {
            var entity = await _villaRepository.GetByIdAsync(request.Id, request.CompanyId, ct);
            if (entity is null)
                throw new NotFoundException("الفيلا غير موجودة");

            entity.IsDeleted = true;

            _villaRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم حذف الفيلا بنجاح");
        }
    }
}
