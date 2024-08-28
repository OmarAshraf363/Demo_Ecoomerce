using Microsoft.AspNetCore.Mvc;

namespace Demo.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class GiftController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
      

    }
}
