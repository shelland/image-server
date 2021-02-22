using Microsoft.AspNetCore.Mvc;

namespace ImageServerDemo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
