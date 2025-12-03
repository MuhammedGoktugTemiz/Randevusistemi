using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RandevuWeb.Services;

namespace RandevuWeb.Controllers;

[Authorize]
public class NotificationsController : Controller
{
    private readonly IDataService _dataService;

    public NotificationsController(IDataService dataService)
    {
        _dataService = dataService;
    }

    public IActionResult Index()
    {
        var now = DateTime.Now;
        var tomorrow = now.AddDays(1);

        var upcomingAppointments = _dataService.GetAppointments()
            .Where(a => a.Start >= now && a.Start <= tomorrow && a.Status == "Bekliyor")
            .OrderBy(a => a.Start)
            .ToList();

        return View(upcomingAppointments);
    }
}

