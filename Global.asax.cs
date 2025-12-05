using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity;
using Unity.Mvc5;

namespace RandevuWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Unity Dependency Injection
            var container = UnityConfig.RegisterComponents();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            // Initialize database - Production'da sadece connection test et
            try
            {
                using (var context = new Data.ApplicationDbContext())
                {
                    // Sadece connection test et, database otomatik oluşturma
                    // Database zaten SQL Server'da oluşturulmuş olmalı
                    var canConnect = context.Database.Exists();
                    if (!canConnect)
                    {
                        // Log warning but don't prevent app from starting
                        System.Diagnostics.Debug.WriteLine("Warning: Database connection failed. Please check connection string in web.config");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error but don't prevent app from starting
                // Production'da uygulama başlamalı, database hatası sonra düzeltilebilir
                System.Diagnostics.Debug.WriteLine($"Database connection test error: {ex.Message}");
            }
        }
    }
}

