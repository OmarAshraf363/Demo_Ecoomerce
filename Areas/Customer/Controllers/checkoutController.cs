using Demo.Models;
using Demo.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Demo.Check;
using Stripe.Checkout;

namespace Demo.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class checkoutController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IunitOfWork unitOfWork;

        public checkoutController(UserManager<IdentityUser> userManager, IunitOfWork unitOfWork)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult success()
        {

            var userId = userManager.GetUserId(User);
            var userOrder = unitOfWork.OrderRepository.Get(e => e.AppUserId == userId && e.OrderStatus == 0)?.FirstOrDefault();
            if (userOrder != null)
            {
                var orderItem = unitOfWork.OrderItemRepository.GetOne(e => e.OrderId == userOrder.OrderId);
                orderItem.TotalPrice=orderItem.ListPrice*orderItem.Quantity;
                userOrder.OrderStatus = 1;
                userOrder.RequiredDate = DateOnly.FromDateTime(DateTime.Now);
                userOrder.PaymentStatus = Check.Methods.StaticDataInProcessPayment;
                unitOfWork.OrderRepository.Save();
            }
            else
            {
                return View("NotFound");
            }

            ViewBag.Message = "Your payment was successful! Thank you for your purchase.";
            return View();
        }
        public IActionResult cancel()
        {


            ViewBag.Message = "Your payment was canceled. Please try again.";
            return View();
        }
  
    }
}
