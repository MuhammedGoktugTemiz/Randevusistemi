using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RandevuWeb.Services;

namespace RandevuWeb.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly IDataService _dataService;

    public HomeController(IDataService dataService)
    {
        _dataService = dataService;
    }

    [AllowAnonymous]
    public IActionResult Index(DateTime? selectedDate)
    {
        var date = selectedDate ?? DateTime.Today;
        var appointments = _dataService.GetAppointments();
        var doctors = _dataService.GetDoctors();
        var patients = _dataService.GetPatients();

        // Get week dates
        var startOfWeek = date.AddDays(-(int)date.DayOfWeek + (int)DayOfWeek.Monday);
        if (date.DayOfWeek == DayOfWeek.Sunday)
        {
            startOfWeek = date.AddDays(-6);
        }

        var weekDates = Enumerable.Range(0, 7)
            .Select(i => startOfWeek.AddDays(i))
            .ToList();

        ViewBag.SelectedDate = date;
        ViewBag.WeekDates = weekDates;
        ViewBag.Doctors = doctors;
        ViewBag.Appointments = appointments;
        ViewBag.Patients = patients;

        return View();
    }

    [HttpGet]
    public IActionResult GetAppointmentsForDate(DateTime date)
    {
        var appointments = _dataService.GetAppointmentsByDate(date);
        return Json(appointments);
    }

    [AllowAnonymous]
    public IActionResult Error()
    {
        return View();
    }
}

