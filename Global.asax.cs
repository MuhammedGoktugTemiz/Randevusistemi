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

            // Initialize database
            try
            {
                using (var context = new Data.ApplicationDbContext())
                {
                    // Test connection
                    if (!context.Database.Exists())
                    {
                        context.Database.Create();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error but don't prevent app from starting
                System.Diagnostics.Debug.WriteLine($"Database initialization error: {ex.Message}");
            }
        }
    }
}

