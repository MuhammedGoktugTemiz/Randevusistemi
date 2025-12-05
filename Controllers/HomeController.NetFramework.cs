using System;
using System.Linq;
using System.Web.Mvc;
using RandevuWeb.Services;

namespace RandevuWeb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IDataService _dataService;

        public HomeController()
        {
            _dataService = DependencyResolver.Current.GetService<IDataService>();
        }

        [AllowAnonymous]
        public ActionResult Index(DateTime? selectedDate)
        {
            // Eğer kullanıcı giriş yapmamışsa, giriş seçim sayfasını göster
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LoginSelection");
            }

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

        [AllowAnonymous]
        public ActionResult LoginSelection()
        {
            // Eğer kullanıcı zaten giriş yapmışsa dashboard'a yönlendir
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpGet]
        public JsonResult GetAppointmentsForDate(DateTime date)
        {
            var appointments = _dataService.GetAppointmentsByDate(date);
            return Json(appointments, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult Error()
        {
            return View();
        }
    }
}

