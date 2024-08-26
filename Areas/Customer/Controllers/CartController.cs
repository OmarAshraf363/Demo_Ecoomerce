﻿using Demo.Models;
using Demo.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
            var order = unitOfWork.OrderRepository.GetUserOrder(userId);//get user order
            var orderItems = unitOfWork.OrderItemRepository.GetOrderItemsInSpacifcCart(order.OrderId);//get all order items that equal ioorder.orderid
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
            var order = unitOfWork.OrderRepository.GetOne(e => e.AppUserId == userId);
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
    }
}