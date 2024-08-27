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
            var item = unitOfWork.ProductRepository.GetOne(e => e.ProductId == id, e => e.Brand,expression=>expression.Category);
            model.Product = item;
            var Q = unitOfWork.StockRepository.GetOne(e => e.ProductId == id);
            if (Q != null)
            {
                model.Quantity = Q.Quantity;
            }
            else { model.Quantity = 0; }
            return View(model);
        }
        //[HttpPost]
        //public IActionResult Login(LoginViewModel  model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var customer = unitOfWork.CustomarRepository.GetOne(e => e.Phone == model.Phone && e.Email == model.Email);


        //        if (customer == null)
        //        {
        //            ModelState.AddModelError("", "Wrong Email or Phone");
        //            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        //            {
        //                return Json(new {isvalid=false , errors = "Wrong Email or Phone", type = "all" });
        //            }

        //            return View("Index", model); 
        //        }
        //        else
        //        {
        //            HttpContext.Session.SetInt32("UserID", customer.CustomerId);
        //            HttpContext.Session.SetString("UserName", $"{customer.FirstName} {customer.LastName}");
        //            HttpContext.Session.SetString("UserType", "Customer");
        //            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        //            {
        //                return Json(new { isvalid = true });
        //            }
        //            return RedirectToAction("Index");
        //        }
        //    }
        //    else
        //    {
        //        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        //        {

        //            return Json(new { isvalid = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage),type="one" });
        //        }
        //        return View("Index", model); 
        //    }
        //}
        //[HttpPost]
        //public IActionResult Register(HomeViewModels model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        if (Request.Headers["X-Requested-With"]=="XMLHttpRequest")
        //        {
        //            return Json(new { isvalid = false, errors = ModelState.Values
        //                .SelectMany(v => v.Errors).Select(e => e.ErrorMessage), type = "one" });
        //        }
        //        return View("Index",model);
        //    }
        //    else
        //    {
        //        var checkUser = unitOfWork.CustomarRepository.GetOne(e => e.Email == model.Customar.Email || e.Phone == model.Customar.Phone);

        //        if (checkUser == null)
        //        {
        //           unitOfWork.CustomarRepository.AddFromViewModel(model);
        //            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        //            {
        //                return Json(new { isvalid = true });
        //            }
        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        //            {
        //                return Json(new { isvalid = false, errors = "Wrong Email or Phone", type = "all" });
        //            }
        //            return View("Index",model);
        //        }
        //    }

        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
