using MediatR;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Interfaces;
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
           

           
            return ApiResponse<bool>.Ok(true, "تم حذف العقد بنجاح");
        }
    }
}
