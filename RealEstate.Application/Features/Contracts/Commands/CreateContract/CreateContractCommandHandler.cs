using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.Features.Contracts.Commands.CreateContract;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

public sealed class CreateContractCommandHandler
    : IRequestHandler<CreateContractCommand, ApiResponse<Guid>>
{
    private readonly IContractRepository _contracts;
    private readonly IPropertyRepository _properties;
    private readonly IPaymentRepository _payments;
    private readonly IUnitOfWork _uow;
    private readonly IContractNumberGenerator _numberGenerator;
    private readonly IPaymentGeneratorService _paymentGenerator;

    public CreateContractCommandHandler(
        IContractRepository contracts,
        IPropertyRepository properties,
        IPaymentRepository payments,
        IUnitOfWork uow,
        IContractNumberGenerator numberGenerator,
        IPaymentGeneratorService paymentGenerator)
    {
        _contracts = contracts;
        _properties = properties;
        _payments = payments;
        _uow = uow;
        _numberGenerator = numberGenerator;
        _paymentGenerator = paymentGenerator;
    }

    public async Task<ApiResponse<Guid>> Handle(
        CreateContractCommand cmd,
        CancellationToken ct)
    {
        // ── Property must exist ───────────────────────────
        var propertyExists = await _properties.ExistsAsync(
            cmd.PropertyId, cmd.CompanyId, ct);

        if (!propertyExists)
            throw new NotFoundException("العقار", cmd.PropertyId);

        // ── No active contract on same property ───────────
        var hasOpenContract = await _contracts.HasOpenContractForPropertyAsync(
            cmd.PropertyId, cmd.CompanyId, ct: ct);

        if (hasOpenContract)
            throw new ConflictException(
                "يوجد عقد نشط مرتبط بهذا العقار بالفعل");

        var contractNumber = await _numberGenerator.GenerateAsync(
            cmd.CompanyId, ct);

        var contract = new Contract
        {
            ContractNumber = contractNumber,
            ContractType = cmd.ContractType,
            ContractStatus = ContractStatus.Active,
            Amount = cmd.Amount,
            SecurityDeposit = cmd.SecurityDeposit,
            PaymentCycle = cmd.PaymentCycle,
            PaymentMethod = cmd.PaymentMethod,
            CommissionType = cmd.CommissionType,
            Commission = cmd.Commission,
            CommissionStatus = CommissionStatus.Pending,
            StartDate = cmd.StartDate,
            EndDate = cmd.EndDate,
            Notes = cmd.Notes,
            PropertyId = cmd.PropertyId,
            ClientId = cmd.ClientId,
            AgentId = cmd.AgentId,
            CompanyId = cmd.CompanyId
        };

        // ── Update property status ────────────────────────
        var property = await _properties.GetByIdForDeleteAsync(
            cmd.PropertyId, cmd.CompanyId, ct);

        property!.PropertyStatus = cmd.ContractType == ContractType.Rent
            ? PropertyStatus.Rented
            : PropertyStatus.Sold;

        _contracts.Add(contract);

        // ── Auto-generate payments for rent contracts ─────
        if (cmd.ContractType == ContractType.Rent
            && cmd.PaymentCycle.HasValue
            && cmd.EndDate.HasValue)
        {
            var payments = _paymentGenerator.Generate(
                contractId: contract.Id,
                companyId: cmd.CompanyId,
                totalAmount: cmd.Amount,
                startDate: cmd.StartDate,
                endDate: cmd.EndDate.Value,
                paymentCycle: cmd.PaymentCycle.Value);

            _payments.AddRange(payments);
        }

        await _uow.SaveChangesAsync(ct);

        return ApiResponse<Guid>.Ok(contract.Id, "تم إنشاء العقد بنجاح");
    }
}