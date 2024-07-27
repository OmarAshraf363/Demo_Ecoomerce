using Demo.Data;
using Demo.Models;
using Demo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Controllers
{
    public class BrandController : Controller
    {
        AppDbContext context=new AppDbContext();
        public IActionResult Index()
        {
            var brands = context.Brands.Include(e=>e.Products).ToList();
            BrandViewModels model=new BrandViewModels()
            {
                Brands = brands
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult Create(BrandViewModels model)
        {
            if (ModelState.IsValid)
            {
                Brand brand = new Brand()
                {
                    BrandName = model.BrandName,
                };
                if (Request.Headers["X-Requested-With"]== "XMLHttpRequest")
                {
                    return Json(new { isvalid = true });

                }
                context.Brands.Add(brand);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                if (Request.Headers["X-Requested-With"]== "XMLHttpRequest")
                {
                    var errors = ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
                    return Json(new { isvalid = false, errors = errors, type = "one" });
                }
                return View("Index", model);
            }
        }
        public IActionResult Delete(int id)
        {
            var brand = context.Brands.Find(id);
            if (brand != null)
            {
                context.Brands.Remove(brand);
                context.SaveChanges();
            return RedirectToAction("Index");
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult Edit(BrandViewModels model)
        {
            if (ModelState.IsValid)
            {
                var brand=context.Brands.Find(model.BrandId);
                if (brand != null)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { isvalid = true });

                    }
                    brand.BrandName=model.BrandName;
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var errors=ModelState.Values.SelectMany(x => x.Errors).Select(e=>e.ErrorMessage);
                    return Json(new { isvalid = false, errors= errors,typr="one" });

                }
                return View("Index",model);
            }

        }
    }
}
