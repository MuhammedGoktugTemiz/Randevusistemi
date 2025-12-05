using System;
using System.Linq;
using System.Web.Mvc;
using RandevuWeb.Services;

namespace RandevuWeb.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly IDataService _dataService;

        public NotificationsController()
        {
            _dataService = DependencyResolver.Current.GetService<IDataService>();
        }

        public ActionResult Index()
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
}

