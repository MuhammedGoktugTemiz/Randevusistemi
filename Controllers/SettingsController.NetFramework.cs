using System.Web.Mvc;

namespace RandevuWeb.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}

