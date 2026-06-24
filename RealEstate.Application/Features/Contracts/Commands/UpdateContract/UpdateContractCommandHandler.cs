using MediatR;
using RealEstate.Application.Features.Contracts.DTO;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Domain.Interfaces;
using RealEstate.Domain.Interfaces.Properties;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Contracts.Commands.UpdateContract
{
    public class UpdateContractCommandHandler
       : IRequestHandler<UpdateContractCommand, ApiResponse<ContractDetailDto>>
    {
        private readonly IContractRepository _contractRepository;
        private readonly IPropertyLookupRepository _propertyLookupRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateContractCommandHandler(
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

        public async Task<ApiResponse<ContractDetailDto>> Handle(UpdateContractCommand request, CancellationToken ct)
        {
            // 1) Load tracked contract
            var contract = await _contractRepository.GetByIdForUpdateAsync(request.Id, ct);
            if (contract is null || contract.CompanyId != request.CompanyId)
                throw new NotFoundException("العقد غير موجود");

            var oldPropertyId = contract.PropertyId;
            var oldContractType = contract.ContractType;
            var oldContractStatus = contract.ContractStatus;

            // 2) Contract number uniqueness
            if (await _contractRepository.ContractNumberExistsAsync(
                    request.CompanyId,
                    request.ContractNumber,
                    request.Id,
                    ct))
            {
                throw new ConflictException("رقم العقد مستخدم مسبقاً");
            }

            // 3) Load target property
            var targetProperty = await _propertyLookupRepository.GetTrackedByIdAsync(request.PropertyId, ct);
            if (targetProperty is null || targetProperty.CompanyId != request.CompanyId)
                throw new NotFoundException("العقار غير موجود");

            // 4) Load client
            var client = await _clientRepository.GetByIdAsync(request.ClientId);
            if (client is null || client.CompanyId != request.CompanyId)
                throw new NotFoundException("العميل غير موجود");

            if (!client.IsActive)
                throw new ValidationException("العميل غير نشط");

            // 5) Load agent
            var agent = await _userRepository.GetByIdAsync(request.AgentId);
            if (agent is null || agent.CompanyId != request.CompanyId)
                throw new NotFoundException("الوسيط غير موجود");

            // 6) Validate request business rules
            ValidateBusinessRules(request);

            var isPropertyChanged = oldPropertyId != request.PropertyId;
            var willRemainOpen = request.ContractStatus == ContractStatus.Active ||
                                 request.ContractStatus == ContractStatus.Renewed;

            // 7) If property changed and contract will remain open -> new property must be available
            if (isPropertyChanged && willRemainOpen)
            {
                if (targetProperty.PropertyStatus != PropertyStatus.Available)
                    throw new ValidationException("العقار الجديد ليس متاحاً للتعاقد");

                var hasAnotherOpenContract = await _contractRepository.HasOpenContractForPropertyAsync(
                    request.CompanyId,
                    request.PropertyId,
                    request.Id,
                    ct);

                if (hasAnotherOpenContract)
                    throw new ConflictException("يوجد عقد نشط بالفعل لهذا العقار");
            }

            // 8) If same property and contract will remain open -> ensure no other active contract exists
            if (!isPropertyChanged && willRemainOpen)
            {
                var hasAnotherOpenContract = await _contractRepository.HasOpenContractForPropertyAsync(
                    request.CompanyId,
                    request.PropertyId,
                    request.Id,
                    ct);

                if (hasAnotherOpenContract)
                    throw new ConflictException("يوجد عقد نشط آخر لهذا العقار");
            }

            // 9) Load old property if property changed
            Property? oldProperty = null;
            if (isPropertyChanged)
            {
                oldProperty = await _propertyLookupRepository.GetTrackedByIdAsync(oldPropertyId, ct);
                if (oldProperty is null || oldProperty.CompanyId != request.CompanyId)
                    throw new NotFoundException("العقار السابق غير موجود");
            }

            // 10) Update contract fields
            contract.ContractNumber = request.ContractNumber.Trim();
            contract.ContractType = request.ContractType;
            contract.ContractStatus = request.ContractStatus;
            contract.Amount = request.Amount;

            contract.SecurityDeposit = request.ContractType == ContractType.Rent
                ? request.SecurityDeposit
                : null;

            contract.PaymentCycle = request.ContractType == ContractType.Rent
                ? request.PaymentCycle
                : null;

            contract.PaymentMethod = request.ContractType == ContractType.Sale
                ? request.PaymentMethod
                : null;

            contract.CommissionType = request.CommissionType;
            contract.Commission = request.Commission;
            contract.CommissionStatus = request.CommissionStatus;

            contract.StartDate = request.StartDate;
            contract.EndDate = request.EndDate;
            contract.CancellationReason = request.CancellationReason;
            contract.Notes = request.Notes;

            contract.PropertyId = request.PropertyId;
            contract.ClientId = request.ClientId;
            contract.AgentId = request.AgentId;

            // 11) Update property statuses
            await HandlePropertyStatusesAsync(
                oldProperty,
                targetProperty,
                isPropertyChanged,
                oldContractStatus,
                request.ContractStatus,
                oldContractType,
                request.ContractType);

            await _unitOfWork.SaveChangesAsync(ct);

            return ApiResponse<ContractDetailDto>.Ok(new ContractDetailDto
            {
                Id = contract.Id,
                ContractNumber = contract.ContractNumber,
                ContractType = contract.ContractType,
                ContractStatus = contract.ContractStatus,
                Amount = contract.Amount,
                SecurityDeposit = contract.SecurityDeposit,
                PaymentCycle = contract.PaymentCycle,
                PaymentMethod = contract.PaymentMethod,
                CommissionType = contract.CommissionType,
                Commission = contract.Commission,
                CommissionStatus = contract.CommissionStatus,
                StartDate = contract.StartDate,
                EndDate = contract.EndDate,
                CancellationReason = contract.CancellationReason,
                Notes = contract.Notes,

                PropertyId = contract.PropertyId,
                PropertyTitle = targetProperty.Title,

                ClientId = contract.ClientId,
                ClientName = client.FullName,
                ClientPhone = client.Phone,

                AgentId = contract.AgentId,
                AgentName = agent.FullName,

                CompanyId = contract.CompanyId
            }, "تم تحديث العقد بنجاح");
        }

        private static void ValidateBusinessRules(UpdateContractCommand request)
        {
            if (string.IsNullOrWhiteSpace(request.ContractNumber))
                throw new ValidationException("رقم العقد مطلوب");

            if (request.Amount <= 0)
                throw new ValidationException("قيمة العقد يجب أن تكون أكبر من صفر");

            if (request.Commission < 0)
                throw new ValidationException("العمولة لا يمكن أن تكون أقل من صفر");

            if (request.StartDate == default)
                throw new ValidationException("تاريخ بداية العقد مطلوب");

            if (request.ContractType == ContractType.Rent)
            {
                if (!request.EndDate.HasValue)
                    throw new ValidationException("تاريخ نهاية العقد مطلوب في عقد الإيجار");

                if (!request.PaymentCycle.HasValue)
                    throw new ValidationException("دورة السداد مطلوبة في عقد الإيجار");
            }

            if (request.ContractType == ContractType.Sale)
            {
                if (!request.PaymentMethod.HasValue)
                    throw new ValidationException("طريقة الدفع مطلوبة في عقد البيع");
            }

            if (request.EndDate.HasValue && request.EndDate.Value <= request.StartDate)
                throw new ValidationException("تاريخ نهاية العقد يجب أن يكون بعد تاريخ البداية");

            if (request.ContractStatus == ContractStatus.Cancelled &&
                string.IsNullOrWhiteSpace(request.CancellationReason))
            {
                throw new ValidationException("سبب الإلغاء مطلوب عند إلغاء العقد");
            }
        }

        private static Task HandlePropertyStatusesAsync(
            Property? oldProperty,
            Property targetProperty,
            bool isPropertyChanged,
            ContractStatus oldStatus,
            ContractStatus newStatus,
            ContractType oldType,
            ContractType newType)
        {
            // Case 1: property changed
            if (isPropertyChanged)
            {
                // release old property if old contract was occupying it
                if (oldProperty is not null && IsOpenStatus(oldStatus))
                {
                    oldProperty.PropertyStatus = PropertyStatus.Available;
                }

                // occupy new property if updated contract is still open
                if (IsOpenStatus(newStatus))
                {
                    targetProperty.PropertyStatus = newType == ContractType.Rent
                        ? PropertyStatus.Rented
                        : PropertyStatus.Sold;
                }
                else
                {
                    targetProperty.PropertyStatus = PropertyStatus.Available;
                }

                return Task.CompletedTask;
            }

            // Case 2: same property, but contract status/type may change
            if (!IsOpenStatus(newStatus))
            {
                targetProperty.PropertyStatus = PropertyStatus.Available;
                return Task.CompletedTask;
            }

            targetProperty.PropertyStatus = newType == ContractType.Rent
                ? PropertyStatus.Rented
                : PropertyStatus.Sold;

            return Task.CompletedTask;
        }

        private static bool IsOpenStatus(ContractStatus status)
        {
            return status == ContractStatus.Active ||
                   status == ContractStatus.Renewed;
        }
    }
}
