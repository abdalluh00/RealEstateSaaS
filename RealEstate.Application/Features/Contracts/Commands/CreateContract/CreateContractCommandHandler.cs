
using MediatR;
using RealEstate.Application.Features.Contracts.DTO;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Domain.Interfaces.Properties;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Contracts.Commands.CreateContract
{
    public class CreateContractHandler : IRequestHandler<CreateContractCommand, ApiResponse<Guid>>
    {
        private readonly IContractRepository _contractRepository;
        private readonly IPropertyLookupRepository _propertyLookupRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateContractHandler(
            IContractRepository contractRepository,
            IPropertyLookupRepository propertyLookupRepository,
            IClientRepository clientRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _contractRepository = contractRepository;
            _propertyLookupRepository = propertyLookupRepository;
            _clientRepository = clientRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<Guid>> Handle(CreateContractCommand request, CancellationToken ct)
        {
            // 1) Property must exist in same company (TRACKED because we will update status)
            var property = await _propertyLookupRepository.GetTrackedByIdAsync(request.PropertyId, ct);
            if (property is null || property.CompanyId != request.CompanyId)
                throw new NotFoundException("العقار غير موجود");

            // 2) Property must be available
            if (property.PropertyStatus != PropertyStatus.Available)
                throw new ConflictException("لا يمكن إنشاء عقد لأن العقار غير متاح حالياً");

            // 3) Client must exist in same company
            var client = await _clientRepository.GetByIdAsync(request.ClientId);
            if (client is null || client.CompanyId != request.CompanyId)
                throw new NotFoundException("العميل غير موجود");

            // 4) Client must be active
            if (!client.IsActive)
                throw new ValidationException("لا يمكن إنشاء عقد لعميل غير نشط");

            // 5) Agent must exist in same company
            var agent = await _userRepository.GetByIdAsync(request.AgentId);
            if (agent is null || agent.CompanyId != request.CompanyId)
                throw new NotFoundException("الوسيط غير موجود");

            // 6) Prevent another active/renewed contract for same property
            var hasOpenContract = await _contractRepository.HasOpenContractForPropertyAsync(
                request.CompanyId,
                request.PropertyId,
                null,
                ct);

            if (hasOpenContract)
                throw new ConflictException("يوجد عقد نشط أو مجدد مسبقاً لهذا العقار");

            // 7) Validate contract-type-specific fields
            ValidateContractBusinessRules(request);

            // 8) Generate contract number
            var contractNumber = await _contractRepository.GenerateNextContractNumberAsync(
                request.CompanyId,
                ct);

            // 9) Create contract
            var contract = new Contract
            {
                ContractNumber = contractNumber,
                ContractType = request.ContractType,
                ContractStatus = ContractStatus.Active,
                Amount = request.Amount,
                SecurityDeposit = request.SecurityDeposit,
                PaymentCycle = request.PaymentCycle ?? default,
                PaymentMethod = request.PaymentMethod ?? default,
                CommissionType = request.CommissionType,
                Commission = request.Commission,
                CommissionStatus = request.CommissionStatus,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Notes = request.Notes,
                PropertyId = request.PropertyId,
                ClientId = request.ClientId,
                AgentId = request.AgentId,
                CompanyId = request.CompanyId
            };

            // 10) Update property status based on contract type
            property.PropertyStatus = request.ContractType switch
            {
                ContractType.Rent => PropertyStatus.Rented,
                ContractType.Sale => PropertyStatus.Sold,
                _ => property.PropertyStatus
            };

            await _contractRepository.AddAsync(contract);
            await _unitOfWork.SaveChangesAsync(ct);

            return ApiResponse<Guid>.Ok(contract.Id, "تم إنشاء العقد بنجاح");
        }

        private static void ValidateContractBusinessRules(CreateContractCommand request)
        {
            if (request.StartDate == default)
                throw new ValidationException("تاريخ بداية العقد مطلوب");

            if (request.EndDate is null)
                throw new ValidationException("تاريخ نهاية العقد مطلوب");

            if (request.EndDate <= request.StartDate)
                throw new ValidationException("تاريخ نهاية العقد يجب أن يكون بعد تاريخ البداية");

            if (request.Amount <= 0)
                throw new ValidationException("قيمة العقد يجب أن تكون أكبر من صفر");

            if (request.Commission < 0)
                throw new ValidationException("العمولة لا يمكن أن تكون أقل من صفر");

            switch (request.ContractType)
            {
                case ContractType.Rent:
                    if (!request.PaymentCycle.HasValue)
                        throw new ValidationException("دورة السداد مطلوبة في عقد الإيجار");

                    break;

                case ContractType.Sale:
                    if (!request.PaymentMethod.HasValue)
                        throw new ValidationException("طريقة الدفع مطلوبة في عقد البيع");

                    break;
            }
        }
    }
}
