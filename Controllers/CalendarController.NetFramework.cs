using System;
using System.Linq;
using System.Web.Mvc;
using RandevuWeb.Services;

namespace RandevuWeb.Controllers
{
    [Authorize]
    public class CalendarController : Controller
    {
        private readonly IDataService _dataService;

        public CalendarController()
        {
            _dataService = DependencyResolver.Current.GetService<IDataService>();
        }

        public ActionResult Index(int? year, int? month, DateTime? selectedDate)
        {
            var date = new DateTime(year ?? DateTime.Now.Year, month ?? DateTime.Now.Month, 1);
            var appointments = _dataService.GetAppointments();

            var selectedDay = selectedDate ?? DateTime.Today;
            if (selectedDay.Year != date.Year || selectedDay.Month != date.Month)
            {
                selectedDay = new DateTime(date.Year, date.Month, Math.Min(selectedDay.Day, DateTime.DaysInMonth(date.Year, date.Month)));
            }

            ViewBag.CurrentDate = date;
            ViewBag.Appointments = appointments;
            ViewBag.SelectedDay = selectedDay;
            ViewBag.SelectedDayAppointments = appointments
                .Where(a => a.Start.Date == selectedDay.Date)
                .OrderBy(a => a.Start)
                .ToList();

            return View();
        }

        [HttpGet]
        public JsonResult GetAppointmentsForDate(DateTime date)
        {
            var appointments = _dataService.GetAppointmentsByDate(date);
            return Json(appointments, JsonRequestBehavior.AllowGet);
        }
    }
}

