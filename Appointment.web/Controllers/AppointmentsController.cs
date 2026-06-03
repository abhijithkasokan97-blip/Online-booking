using Appointment.Application.Appointment.Queries.GetAppointments;
using Appointment.Application.Appointments.Commands.BookAppointment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Appointment.web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AppointmentsController(IMediator mediator)
        {
          _mediator = mediator;  
        }

        [HttpGet]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult<List<AppointmentDto>>> GetAllAppointment()
        {
            var appointments = await _mediator.Send(new GetAppointmentsQuery());
            return Ok(appointments);
        }

        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> BookAppointmentCommand(BookAppointmentCommand command)
        {
            var appointmentId = await _mediator.Send(command);
            
            return Ok(new {id= appointmentId,message = "Appointment booked successfully"});
        }
    }
}
