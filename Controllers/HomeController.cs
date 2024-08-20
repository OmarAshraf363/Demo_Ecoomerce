using Demo.Data;
using Demo.Models;
using Demo.Repository.ModelsRepository.CategoryModel;
using Demo.Repository.ModelsRepository.CustomarModel;
using Demo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICustomarRepository _customarRepository;
        public HomeController(ILogger<HomeController> logger, ICategoryRepository categoryRepository, ICustomarRepository customarRepository)
        {
            _logger = logger;

            _categoryRepository = categoryRepository;
            _customarRepository = customarRepository;
        }

        public IActionResult Index(HomeViewModels model)
        {

            var categories =_categoryRepository.GetAll().AsQueryable().Include(e=>e.Products).ToList();
            model.Categories = categories;  
            return View(model);
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel  model)
        {
            if (ModelState.IsValid)
            {
                var customer = _customarRepository.GetAll()
                    .FirstOrDefault(e => e.Phone == model.Phone && e.Email == model.Email);

                if (customer == null)
                {
                    ModelState.AddModelError("", "Wrong Email or Phone");
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new {isvalid=false , errors = "Wrong Email or Phone", type = "all" });
                    }
                  
                    return View("Index", model); 
                }
                else
                {
                    HttpContext.Session.SetInt32("UserID", customer.CustomerId);
                    HttpContext.Session.SetString("UserName", $"{customer.FirstName} {customer.LastName}");
                    HttpContext.Session.SetString("UserType", "Customer");
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { isvalid = true });
                    }
                    return RedirectToAction("Index");
                }
            }
            else
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    
                    return Json(new { isvalid = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage),type="one" });
                }
                return View("Index", model); 
            }
        }
        [HttpPost]
        public IActionResult Register(HomeViewModels model)
        {
            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"]=="XMLHttpRequest")
                {
                    return Json(new { isvalid = false, errors = ModelState.Values
                        .SelectMany(v => v.Errors).Select(e => e.ErrorMessage), type = "one" });
                }
                return View("Index",model);
            }
            else
            {
                var checkUser = _customarRepository.GetAll()
                    .Where(e => e.Email == model.Customar.Email || e.Phone == model.Customar.Phone).FirstOrDefault();
                if (checkUser == null)
                {
                   _customarRepository.AddFromViewModel(model);
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { isvalid = true });
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { isvalid = false, errors = "Wrong Email or Phone", type = "all" });
                    }
                    return View("Index",model);
                }
            }
          
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
