using Demo.Data;
using Demo.Models;
using Demo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Controllers
{
    public class CategoryController : Controller
    {
        AppDbContext context=new AppDbContext();
        public IActionResult Index()
        {
            var categories=context.Categories.Include(e=>e.Products).ToList();
            CategoryViewModels models = new CategoryViewModels()
            {
                Categories = categories
            };
            return View(models);
        }
        [HttpPost]
        public IActionResult Create(CategoryViewModels model)
        {
            if (ModelState.IsValid)
            {
                Category category = new Category()
                {
                    CategoryName = model.CategoryName,
                };
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new{isvalid = true});
                }
                context.Categories.Add(category);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                var categories = context.Categories.Include(e => e.Products).ToList();
                model.Categories = categories;
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    return Json(new { isvalid = false, errors = errors });
                }
                return View("Index", model); 
            }
           
        }
        public IActionResult Edit(CategoryViewModels model)
        {
            if (ModelState.IsValid)
            {
                var cat = context.Categories.Find(model.CategoryId);
                cat.CategoryName= model.CategoryName;

                context.SaveChanges();
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { isvalid = true });
                }
                return RedirectToAction("Index");
            }
            else
            {

                var categories = context.Categories.Include(e => e.Products).ToList();
                model.Categories = categories;
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    return Json(new { isvalid = false, errors = errors });
                }
                return View("Index", model);
            }

        }
        public IActionResult Delete(int id) 
        {
            var item = context.Categories.Find(id);
            if (item == null)
            {
                return View("Error");
            }
            else
            {
                context.Categories.Remove(item);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
        }

    }
}
