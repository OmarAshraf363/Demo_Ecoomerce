using Demo.Data;
using Demo.Models;
using Demo.Repository.IRepository;
using Demo.Repository.ModelsRepository.OrderItemRepository;
using Demo.Repository.ModelsRepository.OrderModel;
using Demo.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            unitOfWork.OrderItemRepository.spacifcDelete(id);

            TempData["success"] = "Product is Deleted Successfully";
            return RedirectToAction("Index");

        }
    }
}