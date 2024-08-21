using Demo.Data;
using Demo.Models;
using Demo.Repository.IRepository;
using Demo.Repository.ModelsRepository.OrderItemRepository;
using Demo.Repository.ModelsRepository.OrderModel;
using Demo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Controllers
{
    public class OrderController : Controller
    {
        
        private readonly IunitOfWork unitOfWork;

        public OrderController( IunitOfWork unitOfWork)
        {

           
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserID");
            var order=unitOfWork.OrderRepository.GetUserOrder(userId);//get user order
            var orderItems = unitOfWork.OrderItemRepository.GetOrderItemsInSpacifcCart(order.OrderId);//get all order items that equal ioorder.orderid
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
           var order= unitOfWork.OrderRepository.CreateFirstOrderIfNotExisted(userId);
            var orderitems = unitOfWork.OrderItemRepository.Get().
                Where(e => e.ProductId == id && e.OrderId == order.OrderId)
                .SingleOrDefault();

          unitOfWork.OrderItemRepository.createOrderItemsIfNotExisted(id,order.OrderId,q);
            unitOfWork.OrderItemRepository.Save();
            TempData["success"] = "Successfully Added To Cart";

            return RedirectToAction("Index","Home");
        }
        public IActionResult Delete(int id)
        {
           unitOfWork.OrderItemRepository.spacifcDelete(id);
            
            TempData["success"] = "Product is Deleted Successfully";
            return RedirectToAction("Index");
       
        }
    }
}