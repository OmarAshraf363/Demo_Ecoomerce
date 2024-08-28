using Demo.Models;
using Demo.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Stripe;
using System.Net.Mail;
using Newtonsoft.Json;
using Demo.ViewModels;
using Demo.Check;


namespace Demo.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IunitOfWork unitOfWork;
        private readonly UserManager<IdentityUser> userManager;

        public CartController(IunitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {


            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            var userId = userManager.GetUserId(User);
            var order = unitOfWork.OrderRepository.GetOne(e => e.AppUserId == userId&&e.OrderStatus==0);
            if(order == null)
            {
             
                    return View(new List<CartViewModel>());
                
            }
          //get user order
            var orderItems = unitOfWork.OrderItemRepository.GetOrderItemsInSpacifcCart(order.OrderId);//get all order items that equal ioorder.orderid
           
            TempData["shoppingCart"] = JsonConvert.SerializeObject(orderItems);
            return View(orderItems);
            
          
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int id, int productId, int change)
        {
            var orderItem = unitOfWork.OrderItemRepository.Get(e => e.OrderId == id && e.ProductId == productId)?.SingleOrDefault();

            if (orderItem != null)
            {
                orderItem.Quantity += change;
                if (orderItem.Quantity < 1)
                {
                    orderItem.Quantity = 1;
                }

                orderItem.TotalPrice = orderItem.Quantity * orderItem.ListPrice;
                unitOfWork.OrderItemRepository.Save();

                var grandTotal = unitOfWork.OrderItemRepository.Get(e => e.OrderId == orderItem.OrderId)?.Sum(e => e.TotalPrice);

                return Json(new
                {
                    newQuantity = orderItem.Quantity,
                    newTotalPrice = orderItem.TotalPrice,
                    grandTotal = grandTotal.ToString()
                });
            }

            return BadRequest();
        }
        public IActionResult Delete(int id)
        {
            var userId = userManager.GetUserId(User);
            var order = unitOfWork.OrderRepository.GetOne(e => e.AppUserId == userId&&e.OrderStatus==0);
            if (order != null)
            {
                var item = unitOfWork.OrderItemRepository.GetOne(e => e.ProductId == id && e.OrderId == order.OrderId);

                if (item != null)
                {
                    unitOfWork.OrderItemRepository.Delete(item);
                    unitOfWork.OrderItemRepository.Save();
                    return RedirectToAction("Index");
                }
                return BadRequest();
            }
            return BadRequest();

        }


        public IActionResult Pay()
        {
            var items = JsonConvert.DeserializeObject<IEnumerable<CartViewModel>>((string)TempData["shoppingCart"]);
            var order = unitOfWork.OrderRepository.Get(e => e.OrderId == items.FirstOrDefault().OrderId)?.FirstOrDefault();
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),

                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/{Methods.StaticData_CustomerRole}/checkout/success",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/{Methods.StaticData_CustomerRole}/checkout/cancel",
            };
            foreach (var model in items)
            {
                var result = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = model.ProductName,
                        },
                        UnitAmount = (long)model.ListPrice * 100,
                    },
                    Quantity = model.Quantity,
                };
                options.LineItems.Add(result);
            }
            var service = new SessionService();
            var session = service.Create(options);

            if (order != null)
            {
                order.StripeChargeId = session.Id; // Save the session ID or charge ID as needed
                unitOfWork.OrderRepository.Save();
            }
            return Redirect(session.Url);
        }


    }
}
