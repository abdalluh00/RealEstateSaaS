using MediatR;
using RealEstate.Domain.Interfaces;
using RealEstate.Domain.Interfaces.Properties;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Buildings.Commands.DeleteBuilding
{
    public class DeleteBuildingHandler : IRequestHandler<DeleteBuildingCommand, ApiResponse<bool>>
    {
        private readonly IBuildingRepository _buildingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteBuildingHandler(
            IBuildingRepository buildingRepository,
            IUnitOfWork unitOfWork)
        {
            _buildingRepository = buildingRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<bool>> Handle(DeleteBuildingCommand request, CancellationToken ct)
        {
            var entity = await _buildingRepository.GetByIdAsync(request.Id, request.CompanyId);
            if (entity is null)
                throw new NotFoundException("المبنى غير موجود");

            var hasChildren = await _buildingRepository.HasChildrenAsync(request.Id, request.CompanyId);
            if (hasChildren)
                throw new ConflictException("لا يمكن حذف المبنى لوجود وحدات مرتبطة به، احذف الشقق أولاً");

            entity.IsDeleted = true;

            _buildingRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم حذف المبنى بنجاح");
        }
    }
}
