using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using RandevuWeb.Models;
using RandevuWeb.Services;

namespace RandevuWeb.Controllers
{
    public class DoctorAuthController : Controller
    {
        private readonly IDataService _dataService;
        private readonly IPasswordHasher _passwordHasher;
        private const int MaxLoginAttempts = 5;
        private const int LockoutDurationMinutes = 15;

        public DoctorAuthController()
        {
            _dataService = DependencyResolver.Current.GetService<IDataService>();
            _passwordHasher = DependencyResolver.Current.GetService<IPasswordHasher>();
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated && Session["DoctorId"] != null)
            {
                var doctorId = (int)Session["DoctorId"];
                return RedirectToAction("MyAppointments", "DoctorAuth", new { id = doctorId });
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(DoctorLoginModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
            {
                ModelState.AddModelError("", "Kullanıcı adı ve şifre gereklidir!");
                return View(model ?? new DoctorLoginModel());
            }

            var username = model.Username?.Trim();
            var password = model.Password?.Trim();

            if (string.IsNullOrWhiteSpace(username))
            {
                ModelState.AddModelError("", "Kullanıcı adı gereklidir!");
                return View(model);
            }

            // Rate limiting kontrolü (Session kullanarak)
            var clientIp = Request.UserHostAddress ?? "unknown";
            var sessionKey = $"doctor_login_attempts_{clientIp}_{username}";
            
            var attemptCount = Session[sessionKey] as int? ?? 0;
            if (attemptCount >= MaxLoginAttempts)
            {
                ModelState.AddModelError("", $"Çok fazla başarısız giriş denemesi! Lütfen {LockoutDurationMinutes} dakika sonra tekrar deneyin.");
                return View(model);
            }

            var doctor = _dataService.GetDoctorByUsername(username);
            
            if (doctor == null)
            {
                IncrementLoginAttempts(sessionKey);
                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı!");
                return View(model);
            }

            // Şifre doğrulama
            var doctorPassword = doctor.Password?.Trim() ?? string.Empty;
            bool passwordValid = false;
            
            if (string.IsNullOrWhiteSpace(doctorPassword))
            {
                IncrementLoginAttempts(sessionKey);
                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı!");
                return View(model);
            }

            if (doctorPassword.StartsWith("$2a$") || doctorPassword.StartsWith("$2b$") || doctorPassword.StartsWith("$2y$"))
            {
                // BCrypt hash formatı
                passwordValid = _passwordHasher.VerifyPassword(password ?? "", doctorPassword);
            }
            else
            {
                // Eski düz metin şifre
                passwordValid = doctorPassword.Equals(password, StringComparison.Ordinal);
            }

            if (!passwordValid)
            {
                IncrementLoginAttempts(sessionKey);
                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı!");
                return View(model);
            }

            // Başarılı giriş - rate limiting'i temizle
            Session.Remove(sessionKey);

            // Forms Authentication ile giriş yap
            FormsAuthentication.SetAuthCookie($"Doctor_{doctor.Id}", true);
            Session["DoctorId"] = doctor.Id;
            Session["DoctorName"] = doctor.Name;
            Session["DoctorUsername"] = doctor.Username;

            return RedirectToAction("MyAppointments", "DoctorAuth", new { id = doctor.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("LoginSelection", "Home");
        }

        [HttpGet]
        [Authorize]
        public ActionResult MyAppointments(int id, string date = null, string view = "week")
        {
            var doctor = _dataService.GetDoctor(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }

            // Check if logged in doctor matches
            var sessionDoctorId = Session["DoctorId"] as int?;
            if (!sessionDoctorId.HasValue || sessionDoctorId.Value != id)
            {
                if (!sessionDoctorId.HasValue)
                {
                    return RedirectToAction("Login", "DoctorAuth");
                }
                return new HttpStatusCodeResult(403);
            }

            var appointments = _dataService.GetAppointments()
                .Where(a => a.DoctorId == id)
                .OrderBy(a => a.Start)
                .ToList();

            var now = DateTime.Now;
            var today = now.Date;
            
            // Parse date parameter or use today
            DateTime selectedDate = today;
            if (!string.IsNullOrEmpty(date) && DateTime.TryParse(date, out var parsedDate))
            {
                selectedDate = parsedDate.Date;
            }
            
            // Calculate week/month based on view type
            DateTime startOfWeek, endOfWeek, startOfMonth, endOfMonth;
            
            if (view == "month")
            {
                startOfMonth = new DateTime(selectedDate.Year, selectedDate.Month, 1);
                endOfMonth = startOfMonth.AddMonths(1);
                startOfWeek = startOfMonth;
                endOfWeek = endOfMonth;
            }
            else
            {
                startOfWeek = selectedDate.AddDays(-(int)selectedDate.DayOfWeek + (int)DayOfWeek.Monday);
                if (selectedDate.DayOfWeek == DayOfWeek.Sunday) startOfWeek = selectedDate.AddDays(-6);
                endOfWeek = startOfWeek.AddDays(7);
                startOfMonth = new DateTime(selectedDate.Year, selectedDate.Month, 1);
                endOfMonth = startOfMonth.AddMonths(1);
            }

            ViewBag.Doctor = doctor;
            ViewBag.Appointments = appointments;
            ViewBag.Today = today;
            ViewBag.SelectedDate = selectedDate;
            ViewBag.StartOfWeek = startOfWeek;
            ViewBag.EndOfWeek = endOfWeek;
            ViewBag.StartOfMonth = startOfMonth;
            ViewBag.EndOfMonth = endOfMonth;
            ViewBag.Now = now;
            ViewBag.View = view ?? "week";

            return View();
        }

        private void IncrementLoginAttempts(string sessionKey)
        {
            var attemptCount = Session[sessionKey] as int? ?? 0;
            Session[sessionKey] = attemptCount + 1;
        }
    }

    public class DoctorLoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public DoctorLoginModel()
        {
            Username = string.Empty;
            Password = string.Empty;
        }
    }
}

