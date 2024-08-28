using Demo.Check;
using Demo.Data;
using Demo.Models;
using Demo.Repository.IRepository;
using Demo.Repository.ModelsRepository.OrderItemRepository;
using Demo.Repository.ModelsRepository.OrderModel;
using Demo.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;

namespace Demo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {

        private readonly IunitOfWork unitOfWork;
        private readonly UserManager<IdentityUser> userManager;

        public OrderController(IunitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {


            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            var orders = unitOfWork.OrderRepository.Get(null,e=>e.AppUser,expression=>expression.OrderItems);//get user order
            //get all order items that equal ioorder.orderid
            return View(orders);
        }
        [HttpPost]
        public IActionResult AddToCart(int id, int q)
        {

            if (q == 0)
            {
                q = 1;
            }
            var userId = userManager.GetUserId(User);
            var order = unitOfWork.OrderRepository.CreateFirstOrderIfNotExisted(userId);
            var orderitems = unitOfWork.OrderItemRepository.Get().
                Where(e => e.ProductId == id && e.OrderId == order.OrderId)
                .SingleOrDefault();

            unitOfWork.OrderItemRepository.createOrderItemsIfNotExisted(id, order.OrderId, q);
            unitOfWork.OrderItemRepository.Save();
            TempData["success"] = "Successfully Added To Cart";

            return RedirectToAction("Index", "Home", new { Area = "Customer" });
        }
        public IActionResult Delete(int id)
        {
           var order=unitOfWork.OrderRepository.GetOne(e=>e.OrderId==id);
            if (order == null) { return NotFound(); }
            unitOfWork.OrderRepository.Delete(order);

            TempData["success?"] = "Order is Deleted Successfully";
            return RedirectToAction("Index");

        }
        public IActionResult RejectOrder(int id)//refund
        {
            //get the order
            var order = unitOfWork.OrderRepository.Get(e => e.OrderId == id)?.FirstOrDefault();
            if (order != null)
            {
                if (order.OrderStatus == 1 && order.PaymentStatus != Methods.StaticDataSuccessPayment)
                {
                    var service = new SessionService();
                    var session = service.Get(order.StripeChargeId);

                    var refundOptions = new RefundCreateOptions
                    {
                        PaymentIntent = session.PaymentIntentId,
                        Amount = session.AmountTotal,
                        Reason = RefundReasons.RequestedByCustomer
                    };
                    var refundService = new RefundService();
                    var refund = refundService.Create(refundOptions);
                    order.OrderStatus = 0;
                    order.PaymentStatus = Methods.StaticDataRefundedPayment;
                    unitOfWork.OrderRepository.Save();
                    TempData["success?"] = "The Order is Rejected Successfully";
                    return RedirectToAction("Index", "Order");
                }
                else
                {
                    TempData["success?"] = "Sorry,Can not Cancle this Order!!";
                    return RedirectToAction("Index", "Order");
                }
            }
            else
            {
                TempData["success?"] = "Faild To Reject";
                return RedirectToAction("Index", "Order");
            }

        }
        public IActionResult ConfirmOrder(int id)
        {
            var order = unitOfWork.OrderRepository.Get(e => e.OrderId == id, e => e.AppUser)?.FirstOrDefault();
            if (order != null)
            {
                var user = order.AppUser.Email;
                var result = Methods.SendConfirmationEmail(user, order);
                order.ShippedDate = DateOnly.FromDateTime(DateTime.Now);
                if (result)
                {
                    TempData["success?"] = "Payment completed successfully. A confirmation email has been sent to your email address.";
                    order.PaymentStatus = Methods.StaticDataSuccessPayment;
                    unitOfWork.OrderRepository.Save();
                }
                else { TempData["success?"] = "Faild To Send Email"; }
                return RedirectToAction("Index", "Order");
            }
            else
            {
                return View("NotFound");
            }
        }
    }
}