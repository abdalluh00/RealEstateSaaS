using MediatR;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Interfaces;
using RealEstate.Domain.Interfaces.Properties;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Contracts.Commands.DeleteContract
{
    public class DeleteContractHandler : IRequestHandler<DeleteContractCommand, ApiResponse<bool>>
    {
        private readonly IContractRepository _contractRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteContractHandler(
            IContractRepository contractRepository,
            IUnitOfWork unitOfWork)
        {
            _contractRepository = contractRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<bool>> Handle(DeleteContractCommand request, CancellationToken ct)
        {
            var contract = await _contractRepository.GetByIdAsync(
                request.ContractId,
                request.CompanyId,
                ct);

            if (contract is null)
                throw new NotFoundException("العقد غير موجود");

            contract.IsDeleted = true;

            await _unitOfWork.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم حذف العقد بنجاح");
        }
    }
}
