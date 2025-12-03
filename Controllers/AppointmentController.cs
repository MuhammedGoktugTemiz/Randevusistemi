using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RandevuWeb.Models;
using RandevuWeb.Services;

namespace RandevuWeb.Controllers;

[Authorize]
public class AppointmentController : Controller
{
    private readonly IDataService _dataService;
    private readonly IWhatsAppService _whatsAppService;

    public AppointmentController(IDataService dataService, IWhatsAppService whatsAppService)
    {
        _dataService = dataService;
        _whatsAppService = whatsAppService;
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Patients = _dataService.GetPatients();
        ViewBag.Doctors = _dataService.GetDoctors();
        return View();
    }

    [HttpPost]
    public IActionResult Create(Appointment appointment, string timeSelect)
    {
        // Combine date and time
        if (!string.IsNullOrEmpty(timeSelect) && appointment.Start != default)
        {
            var timeParts = timeSelect.Split(':');
            if (timeParts.Length == 2 && int.TryParse(timeParts[0], out int hour) && int.TryParse(timeParts[1], out int minute))
            {
                appointment.Start = appointment.Start.Date.AddHours(hour).AddMinutes(minute);
            }
        }

        if (!ModelState.IsValid || appointment.Start == default)
        {
            ViewBag.Patients = _dataService.GetPatients();
            ViewBag.Doctors = _dataService.GetDoctors();
            if (appointment.Start == default)
            {
                ModelState.AddModelError("", "Tarih ve saat seçilmelidir!");
            }
            return View(appointment);
        }

        // Check doctor conflict
        if (_dataService.HasDoctorConflict(appointment.DoctorId, appointment.Start, appointment.DurationMinutes))
        {
            ModelState.AddModelError("", "Bu saatte doktorun başka bir randevusu var!");
            ViewBag.Patients = _dataService.GetPatients();
            ViewBag.Doctors = _dataService.GetDoctors();
            return View(appointment);
        }

        // Set patient and doctor names
        var patient = _dataService.GetPatient(appointment.PatientId);
        var doctor = _dataService.GetDoctor(appointment.DoctorId);

        if (patient != null)
        {
            appointment.PatientName = patient.FullName;
        }

        if (doctor != null)
        {
            appointment.DoctorName = doctor.Name;
            appointment.DoctorSpecialty = doctor.Specialty;
        }

        // Check if this is a new appointment (Id is 0 or default)
        var isNewAppointment = appointment.Id == 0;
        
        _dataService.SaveAppointment(appointment);
        
        // Send WhatsApp notification to doctor for new appointments only
        if (isNewAppointment && doctor != null && !string.IsNullOrWhiteSpace(doctor.PhoneNumber))
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    await _whatsAppService.SendAppointmentCreatedAsync(appointment, doctor);
                }
                catch
                {
                    // Log error but don't block the request
                    // In production, use proper logging
                }
            });
        }
        
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var appointment = _dataService.GetAppointment(id);
        if (appointment == null)
        {
            return NotFound();
        }

        ViewBag.Patients = _dataService.GetPatients();
        ViewBag.Doctors = _dataService.GetDoctors();
        return View(appointment);
    }

    [HttpPost]
    public IActionResult Edit(Appointment appointment, string timeSelect)
    {
        // Combine date and time
        if (!string.IsNullOrEmpty(timeSelect) && appointment.Start != default)
        {
            var timeParts = timeSelect.Split(':');
            if (timeParts.Length == 2 && int.TryParse(timeParts[0], out int hour) && int.TryParse(timeParts[1], out int minute))
            {
                appointment.Start = appointment.Start.Date.AddHours(hour).AddMinutes(minute);
            }
        }

        if (!ModelState.IsValid || appointment.Start == default)
        {
            ViewBag.Patients = _dataService.GetPatients();
            ViewBag.Doctors = _dataService.GetDoctors();
            if (appointment.Start == default)
            {
                ModelState.AddModelError("", "Tarih ve saat seçilmelidir!");
            }
            return View(appointment);
        }

        // Check doctor conflict (exclude current appointment)
        if (_dataService.HasDoctorConflict(appointment.DoctorId, appointment.Start, appointment.DurationMinutes, appointment.Id))
        {
            ModelState.AddModelError("", "Bu saatte doktorun başka bir randevusu var!");
            ViewBag.Patients = _dataService.GetPatients();
            ViewBag.Doctors = _dataService.GetDoctors();
            return View(appointment);
        }

        // Set patient and doctor names
        var patient = _dataService.GetPatient(appointment.PatientId);
        var doctor = _dataService.GetDoctor(appointment.DoctorId);

        if (patient != null)
        {
            appointment.PatientName = patient.FullName;
        }

        if (doctor != null)
        {
            appointment.DoctorName = doctor.Name;
            appointment.DoctorSpecialty = doctor.Specialty;
        }

        _dataService.SaveAppointment(appointment);
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        _dataService.DeleteAppointment(id);
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult UpdateStatus(int id, string status, int? patientId = null)
    {
        var appointment = _dataService.GetAppointment(id);
        if (appointment != null)
        {
            appointment.Status = status;
            _dataService.SaveAppointment(appointment);
        }
        
        // If patientId is provided, redirect to patient appointments page
        if (patientId.HasValue)
        {
            return RedirectToAction("Appointments", "Patient", new { id = patientId.Value });
        }
        
        return RedirectToAction("Index", "Home");
    }
}

