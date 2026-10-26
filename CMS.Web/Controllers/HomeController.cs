
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CMS.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.UserType = "Administrator";
            ViewBag.fullName = "Test User";
            return View();
        }

       
    }
}
