
using Appointment.Application.Appointment.Queries.GetAppointments;
using Grpc.Core;
using MediatR;
using OnlineAppointment.Grpc;

namespace Appointment.web.Services;

public class AppointmentGrpcService: AppointmentService.AppointmentServiceBase
{
    private IMediator  _mediator;

    public AppointmentGrpcService(
         IMediator  mediator
    )
    {
        _mediator = mediator;
    }

    public override async Task<AppointmentListResponse> GetAppointmentList(AppointmentListRequest request, ServerCallContext context)
    {
        var appointments = await _mediator.Send(new GetAppointmentsQuery());

        var response = new AppointmentListResponse();
        foreach (var item in appointments)
        {
            var protoItem = new AppointmentItem
            {
                Id = item.Id.ToString(),
                PatientName = item.PatientName,
                DoctorName = item.DoctorName,
                AppointmentTime = item.AppointmentTime.ToString("o"),
                Reason = item.Reason
            };

            response.Appointments.Add(protoItem);
        }

        return response;
    }
}