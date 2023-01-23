
using Domain;
using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_IT.View;

namespace Project_IT.Controllers; 


[ApiController]
[Route("Reception")]
public class AppointmentController: ControllerBase {
    private readonly ReceptionService _service;
    private readonly DoctorService _doctorService;

    public AppointmentController(ReceptionService service, DoctorService doctorService) {
        _service = service;
        _doctorService = doctorService;
    }

    [Authorize]
    [HttpPost("add")]
    public async Task<ActionResult<ReceptionView>> SaveAppointment(ReceptionView appointmentView) {
        var appointment = new Reception(
            appointmentView.Id,
            appointmentView.StartTime,
            appointmentView.EndTime,
            appointmentView.PatientId,
            appointmentView.DoctorId
        );
        var res = await _service.AddToConcreteDate(appointment);
        if (!res.Success)
            return Problem(statusCode: 404, detail: res.Error);
        
        return Ok(appointmentView);
    }
    
    [Authorize]
    [HttpPost("add_by_spec")]
    public async Task<ActionResult<ReceptionView>> SaveAppointmentBySpec(DateTime dateTime, Profile spec) {
        var res = await _service.AddToConcreteDate(dateTime, spec);
        if (!res.Success)
            return Problem(statusCode: 404, detail: res.Error);

        var appointmentView = new ReceptionView() {
            DoctorId = res.Value.DoctorId,
            Id = res.Value.Id,
            StartTime = res.Value.StartTime,
            EndTime = res.Value.EndTime
        };
        return Ok(appointmentView);
    }

    [HttpGet("get_free_by_spec")]
    public async Task<ActionResult<List<DateTime>>> GetFreeBySpec(Profile spec) {
        var res = await _service.GetFreeBySpec(spec);
        if (!res.Success)
            return Problem(statusCode: 404, detail: res.Error);

        return Ok(res.Value);

    }
    
    [HttpGet("get_free_by_doctor")]
    public async Task<ActionResult<List<DateTime>>> GetFreeByDoctor(int doctorId) {
        var doctorRes = await _doctorService.GetById(doctorId);
        if (!doctorRes.Success)
            return Problem(statusCode: 404, detail: doctorRes.Error);

        
        var res = await _service.GetFreeByDoctor(doctorRes.Value);
        if (!res.Success)
            return Problem(statusCode: 404, detail: res.Error);

        return Ok(res.Value);

    }
    
    
}