using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Maintenance.Commands.CreateMaintenanceRequest
{
    public sealed class CreateMaintenanceRequestCommandHandler
        : IRequestHandler<CreateMaintenanceRequestCommand, ApiResponse<Guid>>
    {
        private readonly IMaintenanceRepository _maintenance;
        private readonly IUnitOfWork _uow;
        private readonly IMaintenanceNumberGenerator _numberGenerator;

        public CreateMaintenanceRequestCommandHandler(
            IMaintenanceRepository maintenance,
            IUnitOfWork uow,
            IMaintenanceNumberGenerator numberGenerator)
        {
            _maintenance = maintenance;
            _uow = uow;
            _numberGenerator = numberGenerator;
        }

        public async Task<ApiResponse<Guid>> Handle(
            CreateMaintenanceRequestCommand cmd,
            CancellationToken ct)
        {
            var number = await _numberGenerator.GenerateAsync(cmd.CompanyId, ct);

            var request = new MaintenanceRequest
            {
                RequestNumber = number,
                Title = cmd.Title,
                Description = cmd.Description,
                Category = cmd.Category,
                Priority = cmd.Priority,
                Status = MaintenanceStatus.Open,
                PropertyId = cmd.PropertyId,
                ClientId = cmd.ClientId,
                AssignedToId = cmd.AssignedToId,
                CompanyId = cmd.CompanyId
            };

            _maintenance.Add(request);
            await _uow.SaveChangesAsync(ct);

            return ApiResponse<Guid>.Ok(request.Id, "تم إنشاء طلب الصيانة بنجاح");
        }
    }
}