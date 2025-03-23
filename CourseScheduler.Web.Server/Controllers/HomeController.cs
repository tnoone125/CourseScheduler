using Microsoft.AspNetCore.Mvc;

namespace CourseScheduler.Web.Server.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index() { return View(); }
    }
}
