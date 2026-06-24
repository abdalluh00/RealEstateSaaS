//using MediatR;
//using RealEstate.Domain.Interfaces;
//using RealEstate.Shared.Common;
//namespace RealEstate.Application.Features.Appointments.Queries.GetAppointments
//{
//    public class GetAppointmentsHandler
//     : IRequestHandler<GetAppointmentsQuery, ApiResponse<List<AppointmentDto>>>
//    {
//        private readonly IAppointmentRepository _repo;

//        public GetAppointmentsHandler(IAppointmentRepository repo) => _repo = repo;

//        public async Task<ApiResponse<List<AppointmentDto>>> Handle(
//            GetAppointmentsQuery request,
//            CancellationToken ct)
//        {
//            var appointments = request.TodayOnly
//                ? await _repo.GetTodayAsync(request.CompanyId)
//                : await _repo.GetByCompanyAsync(request.CompanyId);

//            var result = appointments.Select(a => new AppointmentDto(
//                a.Id,
//                a.PropertyTitle,
//                a.PropertyCity,
//                a.ClientName,
//                a.ClientPhone,
//                a.AgentName,
//                a.ScheduledAt,
//                a.Status,
//                a.Notes,
//                a.Feedback
//            )).ToList();

//            return ApiResponse<List<AppointmentDto>>.Ok(result);
//        }
//    }
//}
