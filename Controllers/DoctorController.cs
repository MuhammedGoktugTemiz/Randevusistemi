using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RandevuWeb.Models;
using RandevuWeb.Services;

namespace RandevuWeb.Controllers;

[Authorize]
public class DoctorController : Controller
{
    private readonly IDataService _dataService;
    private readonly IPasswordHasher _passwordHasher;

    public DoctorController(IDataService dataService, IPasswordHasher passwordHasher)
    {
        _dataService = dataService;
        _passwordHasher = passwordHasher;
    }

    public IActionResult Index()
    {
        var doctors = _dataService.GetDoctors();
        var now = DateTime.Now;
        var appointments = _dataService.GetAppointments()
            .Where(a => a.Start.Year == now.Year && a.Start.Month == now.Month)
            .ToList();

        var stats = doctors.ToDictionary(
            d => d.Id,
            d =>
            {
                var doctorAppointments = appointments.Where(a => a.DoctorId == d.Id).ToList();
                return new DoctorStats
                {
                    Total = doctorAppointments.Count,
                    Completed = doctorAppointments.Count(a => a.Status == "Tamamlandı"),
                    Waiting = doctorAppointments.Count(a => a.Status == "Bekliyor"),
                    Cancelled = doctorAppointments.Count(a => a.Status == "İptal")
                };
            });

        ViewBag.MonthLabel = now.ToString("MMMM yyyy", new System.Globalization.CultureInfo("tr-TR"));
        ViewBag.DoctorStats = stats;

        return View(doctors);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Doctor doctor, string? password)
    {
        if (!ModelState.IsValid)
        {
            return View(doctor);
        }

        // Şifre belirtilmişse hash'le, yoksa varsayılan şifre oluştur
        if (!string.IsNullOrWhiteSpace(password))
        {
            doctor.Password = _passwordHasher.HashPassword(password);
        }
        else if (string.IsNullOrWhiteSpace(doctor.Password))
        {
            // Varsayılan şifre: kullanıcı adının ilk 4 harfi + "123"
            var defaultPassword = doctor.Username.Length >= 4 
                ? doctor.Username.Substring(0, 4).ToLower() + "123"
                : doctor.Username.ToLower() + "123";
            doctor.Password = _passwordHasher.HashPassword(defaultPassword);
        }
        else if (!doctor.Password.StartsWith("$2a$") && !doctor.Password.StartsWith("$2b$") && !doctor.Password.StartsWith("$2y$"))
        {
            // Eğer şifre hash'lenmemişse hash'le
            doctor.Password = _passwordHasher.HashPassword(doctor.Password);
        }

        _dataService.SaveDoctor(doctor);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var doctor = _dataService.GetDoctor(id);
        if (doctor == null)
        {
            return NotFound();
        }
        return View(doctor);
    }

    [HttpPost]
    public IActionResult Edit(Doctor doctor, string? newPassword)
    {
        if (!ModelState.IsValid)
        {
            return View(doctor);
        }

        // Mevcut doktor bilgilerini al
        var existingDoctor = _dataService.GetDoctor(doctor.Id);
        if (existingDoctor == null)
        {
            return NotFound();
        }

        // Yeni şifre belirtilmişse hash'le ve güncelle
        if (!string.IsNullOrWhiteSpace(newPassword))
        {
            doctor.Password = _passwordHasher.HashPassword(newPassword);
        }
        else
        {
            // Şifre değiştirilmemişse mevcut şifreyi koru
            doctor.Password = existingDoctor.Password;
        }

        // Eğer şifre hash'lenmemişse (eski sistemden kalma) hash'le
        if (!string.IsNullOrWhiteSpace(doctor.Password) && 
            !doctor.Password.StartsWith("$2a$") && 
            !doctor.Password.StartsWith("$2b$") && 
            !doctor.Password.StartsWith("$2y$"))
        {
            doctor.Password = _passwordHasher.HashPassword(doctor.Password);
        }

        _dataService.SaveDoctor(doctor);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        _dataService.DeleteDoctor(id);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult MonthlySummary(int id, int year, int month)
    {
        var doctor = _dataService.GetDoctor(id);
        if (doctor == null)
        {
            return NotFound();
        }

        var appointments = _dataService.GetAppointments()
            .Where(a => a.DoctorId == id && a.Start.Year == year && a.Start.Month == month)
            .ToList();

        ViewBag.Doctor = doctor;
        ViewBag.Year = year;
        ViewBag.Month = month;
        ViewBag.Appointments = appointments;

        return View();
    }
}

