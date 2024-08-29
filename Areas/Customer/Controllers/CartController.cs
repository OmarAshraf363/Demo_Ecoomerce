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


      


























        //public IActionResult AddToCart(HomeViewModels model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var productInStock = unitOfWork.StockRepository.GetOne(e => e.ProductId == model.productId);
        //        if (productInStock != null)
        //        {
        //            if (productInStock.Quantity > model.Quantity)
        //            {
        //                ModelState.AddModelError("quantity", "This Quantity Not Existed in The Stock");
        //                var result1 = Methods.CheckValidation(ModelState, Request, false);
        //                if (result1 != null) { return result1; }
        //            }
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("quantity", "Not Found This Product");
        //            var result1 = Methods.CheckValidation(ModelState, Request, false);
        //            if (result1 != null) { return result1; }
        //        }
        //        if (model.Quantity == 0)
        //        {
        //            model.Quantity = 1;
        //        }
        //        var userId = userManager.GetUserId(User);
        //        var order = unitOfWork.OrderRepository.CreateFirstOrderIfNotExisted(userId);
        //        var orderitems = unitOfWork.OrderItemRepository.Get().
        //            Where(e => e.ProductId == model.productId && e.OrderId == order.OrderId)
        //            .SingleOrDefault();
        //        var result = Methods.CheckValidation(ModelState, Request, true);
        //        if (result != null) { return result; }
        //        unitOfWork.OrderItemRepository.createOrderItemsIfNotExisted(model.productId, order.OrderId, model.Quantity);
        //        unitOfWork.OrderItemRepository.Save();
        //        TempData["success"] = "Successfully Added To Cart";

        //        return RedirectToAction("Index", "Home", new { Area = "Customer" });
        //    }
        //    else
        //    {
        //        var result = Methods.CheckValidation(ModelState, Request, false);
        //        if (result != null) { return result; }

        //    }
        //    return RedirectToAction("Index", "Home", new { Area = "Customer" });
        //}








        [HttpPost]
        public IActionResult UpdateQuantity(int id, int productId, int change)
        {
            var orderItem = unitOfWork.OrderItemRepository.Get(e => e.OrderId == id && e.ProductId == productId)?.SingleOrDefault();
            var prouductInStock=unitOfWork.StockRepository.GetOne(e=>e.ProductId == productId);
            if (orderItem != null)
            {
                orderItem.Quantity += change;
                if (orderItem.Quantity < 1)
                {
                    orderItem.Quantity = 1;
                }else if (orderItem.Quantity>prouductInStock.Quantity)
                {
                    orderItem.Quantity=prouductInStock.Quantity;
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
