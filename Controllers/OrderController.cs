using Demo.Data;
using Demo.Models;
using Demo.Repository.ModelsRepository.OrderItemRepository;
using Demo.Repository.ModelsRepository.OrderModel;
using Demo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Controllers
{
    public class OrderController : Controller
    {
        
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderItemRepository orderItemRepository, IOrderRepository orderRepository)
        {

            _orderItemRepository = orderItemRepository;
            _orderRepository = orderRepository;
        }

        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserID");
            var order=_orderRepository.GetUserOrder(userId);//get user order
            var orderItems = _orderItemRepository.GetOrderItemsInSpacifcCart(order.OrderId);//get all order items that equal ioorder.orderid
            return View(orderItems);
        }
        [HttpPost]
        public IActionResult AddToCart(int id,int q)
        {
            
            if (q == 0)
            {
                q = 1;
            }
            int? userId = HttpContext.Session.GetInt32("UserID");
           var order= _orderRepository.CreateFirstOrderIfNotExisted(userId);
            var orderitems = _orderItemRepository.GetAll().
                Where(e => e.ProductId == id && e.OrderId == order.OrderId)
                .SingleOrDefault();

          _orderItemRepository.createOrderItemsIfNotExisted(id,order.OrderId,q);
            _orderItemRepository.Save();
            TempData["success"] = "Successfully Added To Cart";

            return RedirectToAction("Index","Home");
        }
        public IActionResult Delete(int id)
        {
           _orderItemRepository.spacifcDelete(id);
            
            TempData["success"] = "Product is Deleted Successfully";
            return RedirectToAction("Index");
       
        }
    }
}