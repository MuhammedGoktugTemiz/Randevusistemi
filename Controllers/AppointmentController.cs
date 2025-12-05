using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using RandevuWeb.Models;
using RandevuWeb.Services;

namespace RandevuWeb.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly IDataService _dataService;
        private readonly IWhatsAppService _whatsAppService;

        public AppointmentController()
        {
            _dataService = DependencyResolver.Current.GetService<IDataService>();
            _whatsAppService = DependencyResolver.Current.GetService<IWhatsAppService>();
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Patients = _dataService.GetPatients();
            ViewBag.Doctors = _dataService.GetDoctors();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Appointment appointment, string timeSelect)
        {
            // Combine date and time
            if (!string.IsNullOrEmpty(timeSelect) && appointment.Start != default(DateTime))
            {
                var timeParts = timeSelect.Split(':');
                if (timeParts.Length == 2 && int.TryParse(timeParts[0], out int hour) && int.TryParse(timeParts[1], out int minute))
                {
                    appointment.Start = appointment.Start.Date.AddHours(hour).AddMinutes(minute);
                }
            }

            if (!ModelState.IsValid || appointment.Start == default(DateTime))
            {
                ViewBag.Patients = _dataService.GetPatients();
                ViewBag.Doctors = _dataService.GetDoctors();
                if (appointment.Start == default(DateTime))
                {
                    ModelState.AddModelError("", "Tarih ve saat seçilmelidir!");
                }
                return View(appointment);
            }

            // Check doctor conflict
            if (_dataService.HasDoctorConflict(appointment.DoctorId, appointment.Start, appointment.DurationMinutes, null))
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

            // Check if this is a new appointment
            var isNewAppointment = appointment.Id == 0;
            
            _dataService.SaveAppointment(appointment);
            
            // Send WhatsApp notification to doctor for new appointments only
            if (isNewAppointment && doctor != null && !string.IsNullOrWhiteSpace(doctor.PhoneNumber))
            {
                Task.Run(async () =>
                {
                    try
                    {
                        await _whatsAppService.SendAppointmentCreatedAsync(appointment, doctor);
                    }
                    catch
                    {
                        // Log error but don't block the request
                    }
                });
            }
            
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var appointment = _dataService.GetAppointment(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }

            ViewBag.Patients = _dataService.GetPatients();
            ViewBag.Doctors = _dataService.GetDoctors();
            return View(appointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Appointment appointment, string timeSelect)
        {
            // Combine date and time
            if (!string.IsNullOrEmpty(timeSelect) && appointment.Start != default(DateTime))
            {
                var timeParts = timeSelect.Split(':');
                if (timeParts.Length == 2 && int.TryParse(timeParts[0], out int hour) && int.TryParse(timeParts[1], out int minute))
                {
                    appointment.Start = appointment.Start.Date.AddHours(hour).AddMinutes(minute);
                }
            }

            if (!ModelState.IsValid || appointment.Start == default(DateTime))
            {
                ViewBag.Patients = _dataService.GetPatients();
                ViewBag.Doctors = _dataService.GetDoctors();
                if (appointment.Start == default(DateTime))
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
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            _dataService.DeleteAppointment(id);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus(int id, string status, int? patientId = null)
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
}
