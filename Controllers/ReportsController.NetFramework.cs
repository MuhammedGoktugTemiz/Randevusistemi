using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using RandevuWeb.Services;

namespace RandevuWeb.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly IDataService _dataService;

        public ReportsController()
        {
            _dataService = DependencyResolver.Current.GetService<IDataService>();
        }

        public ActionResult Index(string period = "daily")
        {
            var allAppointments = _dataService.GetAppointments();
            var now = DateTime.Now;
            var filteredAppointments = new List<RandevuWeb.Models.Appointment>();
            
            switch (period.ToLower())
            {
                case "daily":
                    var today = now.Date;
                    filteredAppointments = allAppointments.Where(a => a.Start.Date == today).ToList();
                    break;
                case "weekly":
                    var weekStart = now.AddDays(-(int)now.DayOfWeek + (int)DayOfWeek.Monday);
                    if (now.DayOfWeek == DayOfWeek.Sunday) weekStart = now.AddDays(-6);
                    var weekEnd = weekStart.AddDays(7);
                    filteredAppointments = allAppointments.Where(a => a.Start >= weekStart && a.Start < weekEnd).ToList();
                    break;
                case "monthly":
                    var monthStart = new DateTime(now.Year, now.Month, 1);
                    var monthEnd = monthStart.AddMonths(1);
                    filteredAppointments = allAppointments.Where(a => a.Start >= monthStart && a.Start < monthEnd).ToList();
                    break;
                case "yearly":
                    var yearStart = new DateTime(now.Year, 1, 1);
                    var yearEnd = yearStart.AddYears(1);
                    filteredAppointments = allAppointments.Where(a => a.Start >= yearStart && a.Start < yearEnd).ToList();
                    break;
                default:
                    filteredAppointments = allAppointments;
                    break;
            }
            
            ViewBag.TotalAppointments = filteredAppointments.Count;
            ViewBag.CompletedAppointments = filteredAppointments.Count(a => a.Status == "Tamamlandı");
            ViewBag.WaitingAppointments = filteredAppointments.Count(a => a.Status == "Bekliyor");
            ViewBag.CancelledAppointments = filteredAppointments.Count(a => a.Status == "İptal");
            ViewBag.Period = period;
            ViewBag.Appointments = allAppointments; // For chart - show all data

            return View();
        }
    }
}

