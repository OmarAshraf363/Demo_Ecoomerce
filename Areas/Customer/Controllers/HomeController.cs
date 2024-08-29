using Demo.Check;
using Demo.Data;
using Demo.Models;
using Demo.Repository.IRepository;
using Demo.Repository.ModelsRepository.CategoryModel;
using Demo.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Demo.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IunitOfWork unitOfWork;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        //private readonly ICustomarRepository _customarRepository;
        public HomeController(ILogger<HomeController> logger, IunitOfWork unitOfWork, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            this.unitOfWork = unitOfWork;
            this._roleManager = roleManager;
            _userManager = userManager;
        }

        public  IActionResult Index(HomeViewModels model)
        {
          
            if (User.IsInRole(Check.Methods.StaticData_AdminRole))
            {
                return RedirectToAction("Index", "Product", new { Area = "Admin" });
            }


            var categories = unitOfWork.CategoryRepository.Get(includeProperties: e => e.Products).ToList();
            
            model.Categories = categories;
            model.Stocks = unitOfWork.StockRepository.Get().ToList();

            return View(model);
        }
        public IActionResult ProductsCategory(int id)  => View(unitOfWork.ProductRepository.getAllProductsWithspacifsCategoryOrBrand(id,brand:false));
        
        public IActionResult Details(int id, ProductDetails model)
        {
            model.realQuantity= int.Parse(model.Quantity);
            var item = unitOfWork.ProductRepository.GetOne(e => e.ProductId == id, e => e.Brand,expression=>expression.Category);
            model.Product = item;
           
            var productInStock= unitOfWork.StockRepository.GetOne(e => e.ProductId == id);
            if (productInStock != null)
            {
                model.realQuantity = productInStock.Quantity;
            }
            else { model.realQuantity = 0; }
            model.Products = unitOfWork.ProductRepository.Get(e => e.CategoryId == item.CategoryId);
            
            return View(model);
        }


        [HttpPost]


        public IActionResult AddToCart  ([Bind("productId", "Quantity")] HomeViewModels model)
        {
            var quantityAsIntger = int.Parse(model.Quantity);

            if (!ModelState.IsValid)
            {
                var result = Methods.CheckValidation(ModelState, Request, false);
                if (result != null) { return result; }   
            }

            var productInStock = unitOfWork.StockRepository.GetOne(e => e.ProductId == model.productId);
            if (productInStock == null)
            {
                ModelState.AddModelError("Quantity", "This product was not found in stock.");

                var result = Methods.CheckValidation(ModelState, Request, false);
                if (result != null) { return result; }

            }

            if (productInStock.Quantity < quantityAsIntger || quantityAsIntger <= 0)
            {
                ModelState.AddModelError("Quantity", "The requested quantity is not available.");
                var result = Methods.CheckValidation(ModelState, Request, false);
                if (result != null) { return result; }

            }

            var userId = _userManager.GetUserId(User);
            var order = unitOfWork.OrderRepository.CreateFirstOrderIfNotExisted(userId);
            var existingOrderItem = unitOfWork.OrderItemRepository
                .Get()
                .SingleOrDefault(e => e.ProductId == model.productId && e.OrderId == order.OrderId);

            if (existingOrderItem == null)
            {
                var result = Methods.CheckValidation(ModelState, Request, true);
                if (result != null) { return result; }
                unitOfWork.OrderItemRepository.createOrderItemsIfNotExisted(model.productId, order.OrderId,quantityAsIntger);
                unitOfWork.OrderItemRepository.Save();
            }
            else
            {
                existingOrderItem.Quantity +=quantityAsIntger;
                unitOfWork.OrderItemRepository.Save();
            }

            TempData["success"] = "Successfully added to cart.";
            return RedirectToAction("Index");
        }









        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
