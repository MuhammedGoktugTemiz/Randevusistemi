using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RandevuWeb.Models;
using RandevuWeb.Services;

namespace RandevuWeb.Controllers;

[Authorize]
public class PatientController : Controller
{
    private readonly IDataService _dataService;

    public PatientController(IDataService dataService)
    {
        _dataService = dataService;
    }

    public IActionResult Index(string? search, string? statusFilter)
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
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Patient patient, IFormFile? photoFile)
    {
        // Manual validation for required fields - only FullName and Phone are required
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

        // Only check for FullName and Phone errors
        if (string.IsNullOrWhiteSpace(patient.FullName) || string.IsNullOrWhiteSpace(patient.Phone))
        {
            return View(patient);
        }

        try
        {
            patient.CreatedAt = DateTime.Now;

            // Handle photo upload
            if (photoFile != null && photoFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "patients");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = $"patient_{Guid.NewGuid()}{Path.GetExtension(photoFile.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);
                
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photoFile.CopyToAsync(stream);
                }

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
    public IActionResult Edit(int id)
    {
        var patient = _dataService.GetPatient(id);
        if (patient == null)
        {
            return NotFound();
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
    public async Task<IActionResult> Edit(Patient patient, IFormFile? photoFile)
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

        // Only check for FullName and Phone errors
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
        if (photoFile != null && photoFile.Length > 0)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "patients");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = $"patient_{patient.Id}_{Guid.NewGuid()}{Path.GetExtension(photoFile.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);
            
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await photoFile.CopyToAsync(stream);
            }

            patient.PhotoUrl = $"/uploads/patients/{fileName}";
        }

        _dataService.SavePatient(patient);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        _dataService.DeletePatient(id);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Appointments(int id)
    {
        var patient = _dataService.GetPatient(id);
        if (patient == null)
        {
            return NotFound();
        }

        var appointments = _dataService.GetAppointments()
            .Where(a => a.PatientId == id)
            .OrderBy(a => a.Start)
            .ToList();

        ViewBag.Patient = patient;
        return View(appointments);
    }
}

