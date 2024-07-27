﻿using Demo.Data;
using Demo.Models;
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
        AppDbContext context=new AppDbContext();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            
        }

        public IActionResult Index(HomeViewModels model)
        {

            var categories = context.Categories.Include(e=>e.Products).ToList();
            model.Categories = categories;  
            return View(model);
        }
        [HttpPost]
        public IActionResult Login(HomeViewModels model)
        {
            if (ModelState.IsValid)
            {
                var customer = context.Customers
                    .FirstOrDefault(e => e.Phone == model.Phone && e.Email == model.Email);

                if (customer == null)
                {
                    ModelState.AddModelError("", "Wrong Email or Phone");
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new {isvalid=false , errors = "Wrong Email or Phone", type = "all" });
                    }
                  
                    return View("Index", model); // إعادة نفس الصفحة لعرض الأخطاء
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

                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    
                    return Json(new { isvalid = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage),type="one" });
                }
                return View("Index", model); // إعادة نفس الصفحة لعرض الأخطاء
            }
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}