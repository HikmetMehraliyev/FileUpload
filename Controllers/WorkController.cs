using Microsoft.AspNetCore.Mvc;

namespace WebFrontToBack.Controllers
{
    public class WorkController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
