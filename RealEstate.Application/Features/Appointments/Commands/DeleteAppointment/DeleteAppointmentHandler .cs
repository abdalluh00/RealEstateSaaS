using MediatR;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Appointments.Commands.DeleteAppointment
{
    public class DeleteAppointmentHandler : IRequestHandler<DeleteAppointmentCommand, ApiResponse<bool>>
    {
        private readonly IAppointmentRepository _repo;

        public DeleteAppointmentHandler(IAppointmentRepository repo) => _repo = repo;

        public async Task<ApiResponse<bool>> Handle(
            DeleteAppointmentCommand request,
            CancellationToken ct)
        {
            var appointment = await _repo.GetByIdAsync(request.Id);

            if (appointment is null)
                throw new NotFoundException("الموعد", request.Id);

            appointment.IsDeleted = true;
            _repo.Update(appointment);
            await _repo.SaveChangesAsync();

            return ApiResponse<bool>.Ok(true, "تم حذف الموعد بنجاح");
        }
    }
}
