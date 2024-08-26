using Demo.Data;
using Demo.Models;
using Demo.Repository.IRepository;
using Demo.Repository.ModelsRepository.CategoryModel;
using Demo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace Demo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {

        private readonly IunitOfWork unitOfWork;


        public CategoryController(IunitOfWork unitOfWork)
        {

            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index(string categoryName)
        {
            var categories = unitOfWork.CategoryRepository.Get(includeProperties: e => e.Products).ToList();
            if (!string.IsNullOrEmpty(categoryName))
            {
                var lowerCategoryName = categoryName.ToLower();
                categories = categories.Where(e => e.CategoryName.ToLower().IndexOf(lowerCategoryName, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                ViewBag.CurrentFilterCcategoryName=categoryName;
            }
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

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var category = unitOfWork.CategoryRepository.GetOne(expression: e => e.CategoryName == model.CategoryName);
                    if (category != null)
                    {
                        ModelState.AddModelError("", "Existed");
                        return Json(new { isvalid = false, errors = "Existed", type = "one" });
                    }
                    else
                    {
                        unitOfWork.CategoryRepository.AddFromViewModel(model);
                        return Json(new { isvalid = true });
                    }
                }
                return RedirectToAction("Index");
            }
            else
            {
                var categories = unitOfWork.CategoryRepository.Get(includeProperties: e => e.Products).ToList();
                model.Categories = categories;
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    return Json(new { isvalid = false, errors });
                }
                return View("Index", model);
            }

        }
        public IActionResult Edit(CategoryViewModels model)
        {
            if (ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var category = unitOfWork.CategoryRepository.GetOne(e => e.CategoryId == model.CategoryId);

                    var Existed = unitOfWork.CategoryRepository.GetOne(expression: e => e.CategoryName == model.CategoryName);
                    if (Existed != null)
                    {
                        ModelState.AddModelError("", "Existed");
                        return Json(new { isvalid = false, errors = "Existed", type = "one" });
                    }
                    else
                    {
                        category.CategoryName = model.CategoryName;
                        unitOfWork.CategoryRepository.Edit(category);
                        return Json(new { isvalid = true });
                    }
                }
                return RedirectToAction("Index");
            }
            else
            {

                var categories = unitOfWork.CategoryRepository.Get(includeProperties: e => e.Products).ToList();
                model.Categories = categories;
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    return Json(new { isvalid = false, errors });
                }
                return View("Index", model);
            }
        }
        public IActionResult Delete(int id)
        {
            var item=unitOfWork.CategoryRepository.GetOne(e=>e.CategoryId == id);
            unitOfWork.CategoryRepository.Delete(item);
            return RedirectToAction("Index");

        }

    }
}
