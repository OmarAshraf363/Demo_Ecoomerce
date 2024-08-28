using Microsoft.AspNetCore.Mvc;

namespace Demo.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
    }
}
