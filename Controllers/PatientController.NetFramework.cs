using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RandevuWeb.Models;
using RandevuWeb.Services;

namespace RandevuWeb.Controllers
{
    [Authorize]
    public class PatientController : Controller
    {
        private readonly IDataService _dataService;

        public PatientController()
        {
            _dataService = DependencyResolver.Current.GetService<IDataService>();
        }

        public ActionResult Index(string search = null, string statusFilter = null)
        {
            var patients = _dataService.GetPatients();

            if (!string.IsNullOrWhiteSpace(search))
            {
                patients = patients.Where(p =>
                    p.FullName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    p.Phone.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    p.TcKimlik.Contains(search, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            if (!string.IsNullOrWhiteSpace(statusFilter))
            {
                patients = patients.Where(p => p.Status == statusFilter).ToList();
            }

            var appointments = _dataService.GetAppointments();
            ViewBag.TotalPatients = _dataService.GetPatients().Count;
            ViewBag.ActivePatients = _dataService.GetPatients().Count(p => p.Status == "Aktif");
            ViewBag.NewPatients = _dataService.GetPatients().Count(p => p.CreatedAt >= DateTime.Now.AddDays(-30));
            ViewBag.TotalAppointments = appointments.Count;
            ViewBag.PatientAppointments = appointments
                .Where(a => a.PatientId != 0 && !string.IsNullOrWhiteSpace(a.ToothNumbers))
                .GroupBy(a => a.PatientId)
                .ToDictionary(g => g.Key, g => g.ToList());

            return View(patients);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Patient patient, HttpPostedFileBase photoFile)
        {
            // Manual validation for required fields
            if (string.IsNullOrWhiteSpace(patient.FullName))
            {
                ModelState.AddModelError("FullName", "Ad Soyad gereklidir.");
            }
            if (string.IsNullOrWhiteSpace(patient.Phone))
            {
                ModelState.AddModelError("Phone", "Telefon gereklidir.");
            }

            // Clear any other validation errors except FullName and Phone
            var keysToRemove = ModelState.Keys.Where(k => k != "FullName" && k != "Phone").ToList();
            foreach (var key in keysToRemove)
            {
                ModelState.Remove(key);
            }

            if (string.IsNullOrWhiteSpace(patient.FullName) || string.IsNullOrWhiteSpace(patient.Phone))
            {
                return View(patient);
            }

            try
            {
                patient.CreatedAt = DateTime.Now;

                // Handle photo upload
                if (photoFile != null && photoFile.ContentLength > 0)
                {
                    var uploadsFolder = Server.MapPath("~/uploads/patients");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var fileName = $"patient_{Guid.NewGuid()}{Path.GetExtension(photoFile.FileName)}";
                    var filePath = Path.Combine(uploadsFolder, fileName);
                    
                    photoFile.SaveAs(filePath);

                    patient.PhotoUrl = $"/uploads/patients/{fileName}";
                }

                _dataService.SavePatient(patient);
                TempData["PatientSuccess"] = patient.FullName;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hasta kaydedilirken bir hata oluştu: {ex.Message}");
                return View(patient);
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var patient = _dataService.GetPatient(id);
            if (patient == null)
            {
                return HttpNotFound();
            }

            // Get patient appointments
            var appointments = _dataService.GetAppointments()
                .Where(a => a.PatientId == id)
                .OrderByDescending(a => a.Start)
                .ToList();

            ViewBag.Appointments = appointments;
            return View(patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Patient patient, HttpPostedFileBase photoFile)
        {
            // Only validate FullName and Phone as required fields
            if (string.IsNullOrWhiteSpace(patient.FullName))
            {
                ModelState.AddModelError("FullName", "Ad Soyad gereklidir.");
            }
            if (string.IsNullOrWhiteSpace(patient.Phone))
            {
                ModelState.AddModelError("Phone", "Telefon gereklidir.");
            }

            // Clear any other validation errors except FullName and Phone
            var keysToRemove = ModelState.Keys.Where(k => k != "FullName" && k != "Phone").ToList();
            foreach (var key in keysToRemove)
            {
                ModelState.Remove(key);
            }

            if (string.IsNullOrWhiteSpace(patient.FullName) || string.IsNullOrWhiteSpace(patient.Phone))
            {
                var appointments = _dataService.GetAppointments()
                    .Where(a => a.PatientId == patient.Id)
                    .OrderByDescending(a => a.Start)
                    .ToList();
                ViewBag.Appointments = appointments;
                return View(patient);
            }

            // Handle photo upload
            if (photoFile != null && photoFile.ContentLength > 0)
            {
                var uploadsFolder = Server.MapPath("~/uploads/patients");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = $"patient_{patient.Id}_{Guid.NewGuid()}{Path.GetExtension(photoFile.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);
                
                photoFile.SaveAs(filePath);

                patient.PhotoUrl = $"/uploads/patients/{fileName}";
            }

            _dataService.SavePatient(patient);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            _dataService.DeletePatient(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Appointments(int id)
        {
            var patient = _dataService.GetPatient(id);
            if (patient == null)
            {
                return HttpNotFound();
            }

            var appointments = _dataService.GetAppointments()
                .Where(a => a.PatientId == id)
                .OrderBy(a => a.Start)
                .ToList();

            ViewBag.Patient = patient;
            return View(appointments);
        }
    }
}

