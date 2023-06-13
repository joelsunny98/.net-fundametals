using Microsoft.AspNetCore.Mvc;

namespace RetailStore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
