using Demo.Data;
using Demo.Models;
using Demo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Controllers
{
    public class OrderController : Controller
    {
        AppDbContext context = new AppDbContext();
        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserID");
            var order=context.Orders.Where(e=>e.CustomerId == userId && e.OrderStatus==0).SingleOrDefault();
            var orderItems=context.OrderItems.Where(e=>e.OrderId==order.OrderId).Include(e=>e.Product)
                .Select(e => new CartViewModel
                {
                    ProductId = e.ProductId,
                    ProductName = e.Product.ProductName,
                    Quantity = e.Quantity,
                    ListPrice = e.ListPrice,
                    TotalPrice = e.Quantity * e.ListPrice
                })
        .ToList();
            return View(orderItems);
        }
        [HttpPost]
        public IActionResult AddToCart(int id,int q)
        {
            
            //var product=context.Products.Find(productId);
            int? userId = HttpContext.Session.GetInt32("UserID");
            var order = context.Orders.Where(e => e.CustomerId == userId && e.OrderStatus == 0).SingleOrDefault();
            if (order == null)
            {
                order  = new Order()
                {
                    OrderStatus = 0,
                    OrderDate = DateOnly.FromDateTime(DateTime.Now),
                    CustomerId = userId,
                };
                context.Orders.Add(order);
                context.SaveChanges();
            }
            var orderitems = context.OrderItems.
                Where(e => e.ProductId == id && e.OrderId == order.OrderId)
                .SingleOrDefault();
            if (q == 0)
            {
                q = 1;
            }

            if (orderitems == null)
            {
                orderitems = new OrderItem()
                {
                    ItemId = id,
                    OrderId = order.OrderId,
                    ProductId = id,
                    Quantity = q,
                    ListPrice = context.Products.First(p => p.ProductId == id).ListPrice,
                    Discount = 0
                };
                context.OrderItems.Add(orderitems);
                context.SaveChanges();
            }
            else
            {
                orderitems.Quantity += q;
            }
            context.SaveChanges();
            TempData["success"] = "Successfully Added To Cart";

            return RedirectToAction("Index","Home");
        }
        public IActionResult Delete(int id)
        {
            var orderitem = context.OrderItems.FirstOrDefault(e=>e.ProductId==id);

            if (orderitem == null)
            {
                return View("Error");
            }
            else
            {
                context.OrderItems.Remove(orderitem);
                context.SaveChanges();
                TempData["success"] = "Product is Deleted Successfully";
                return RedirectToAction("Index");
            }
        }
    }
}