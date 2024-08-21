using Demo.Data;
using Demo.Repository.IRepository;
using Demo.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    public class AccountController : Controller
    {
        private readonly IunitOfWork unitOfWork;


        public AccountController(IunitOfWork unitOfWork)
        {

            this.unitOfWork = unitOfWork;
        }

        //public IActionResult Login(HomeViewModels model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var customar = context.Customers
        //          .FirstOrDefault(e => e.Phone == model.Phone && e.Email == model.Email);
        //        if (customar == null)
        //        {
        //            ModelState.AddModelError("", "Wrong Email or Phone");
        //            return RedirectToAction("Index", "Home", model);

        //        }
        //        else
        //        {
        //            HttpContext.Session.SetInt32("UserID", customar.CustomerId);
        //            HttpContext.Session.SetString("UserName", $"{customar.FirstName} {customar.LastName}");
        //            HttpContext.Session.SetString("UserType", "Customar");
        //            return RedirectToAction("Index", "Home");
        //        }
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index", "Home", model);
        //    }
        //}

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
