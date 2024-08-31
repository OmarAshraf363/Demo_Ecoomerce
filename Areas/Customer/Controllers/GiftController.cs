using Demo.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class GiftController : Controller
    {

        private readonly IunitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public GiftController(IunitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var products = _unitOfWork.ProductRepository.Get(e => e.Discount.HasValue, e => e.Category, equals => equals.Brand);
            return View(products);
        }
      

    }
}
