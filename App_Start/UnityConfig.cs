using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using RandevuWeb.Services;
using RandevuWeb.Data;

namespace RandevuWeb
{
    public static class UnityConfig
    {
        public static IUnityContainer RegisterComponents()
        {
            var container = new UnityContainer();

            // Register DbContext
            container.RegisterType<ApplicationDbContext>(new HierarchicalLifetimeManager());

            // Register Services
            container.RegisterType<IDataService, SqlDataService>();
            container.RegisterType<IPasswordHasher, BCryptPasswordHasher>();
            container.RegisterType<IWhatsAppService, WhatsAppService>();

            // Register Controllers
            container.RegisterType<Controllers.AccountController>();
            container.RegisterType<Controllers.HomeController>();
            container.RegisterType<Controllers.AppointmentController>();
            container.RegisterType<Controllers.CalendarController>();
            container.RegisterType<Controllers.DoctorAuthController>();
            container.RegisterType<Controllers.DoctorController>();
            container.RegisterType<Controllers.NotificationsController>();
            container.RegisterType<Controllers.PatientController>();
            container.RegisterType<Controllers.ReportsController>();
            container.RegisterType<Controllers.SettingsController>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            return container;
        }
    }
}

