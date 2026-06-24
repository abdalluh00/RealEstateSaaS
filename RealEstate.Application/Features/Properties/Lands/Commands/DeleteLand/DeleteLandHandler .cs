using MediatR;
using RealEstate.Domain.Interfaces;
using RealEstate.Domain.Interfaces.Properties;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Lands.Commands.DeleteLand
{
    public class DeleteLandHandler : IRequestHandler<DeleteLandCommand, ApiResponse<bool>>
    {
        private readonly ILandRepository _landRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteLandHandler(
            ILandRepository landRepository,
            IUnitOfWork unitOfWork)
        {
            _landRepository = landRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<bool>> Handle(DeleteLandCommand request, CancellationToken ct)
        {
            var entity = await _landRepository.GetByIdAsync(request.Id, request.CompanyId);
            if (entity is null)
                throw new NotFoundException("الأرض غير موجودة");

            var hasChildren = await _landRepository.HasChildrenAsync(request.Id, request.CompanyId);
            if (hasChildren)
                throw new ConflictException("لا يمكن حذف الأرض لوجود عقارات مرتبطة بها");

            entity.IsDeleted = true;

            _landRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم حذف الأرض بنجاح");
        }
    }
}
