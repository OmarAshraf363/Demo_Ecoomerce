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
                ViewBag.CurrentFilterCcategoryName = categoryName;
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


                var category = unitOfWork.CategoryRepository.GetOne(expression: e => e.CategoryName == model.Name);
                if (category != null)
                {
                    ModelState.AddModelError("Name", "Existed");
                    var result = Check.Methods.CheckValidation(ModelState, Request, false);
                    if (result != null) { return result; }
                }
                else
                {
                    var result = Check.Methods.CheckValidation(ModelState, Request, true);
                    if (result != null) { return result; }
                    unitOfWork.CategoryRepository.AddFromViewModel(model);

                }

                return RedirectToAction("Index");
            }
            else
            {
                var categories = unitOfWork.CategoryRepository.Get(includeProperties: e => e.Products).ToList();
                model.Categories = categories;
                var result = Check.Methods.CheckValidation(ModelState, Request, false);
                if (result != null) { return result; }
                return View("Index", model);
            }

        }



        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category=unitOfWork.CategoryRepository.GetOne(e=>e.CategoryId == id);
            CategoryViewModels model = new()
            {
                CategoryId = category.CategoryId ,
                Name = category.CategoryName
            };
            return PartialView("_EditCategoryPartial",model);

        }
        [HttpPost]
        public IActionResult Edit(CategoryViewModels model)
        {
            if (ModelState.IsValid)
            {

                var category = unitOfWork.CategoryRepository.GetOne(e => e.CategoryId == model.CategoryId);

                var Existed = unitOfWork.CategoryRepository.GetOne(expression: e => e.CategoryName == model.Name);
                if (Existed != null)
                {
                    ModelState.AddModelError("Name", "Existed");
                    var result = Check.Methods.CheckValidation(ModelState, Request, false);
                    if (result != null) { return result; }
                }
                else
                {
                    var result = Check.Methods.CheckValidation(ModelState, Request, true);
                    if (result != null) { return result; }
                    category.CategoryName = model.Name;
                    unitOfWork.CategoryRepository.Edit(category);
                    
                }

                return RedirectToAction("Index");
            }
            else
            {

                var categories = unitOfWork.CategoryRepository.Get(includeProperties: e => e.Products).ToList();
                model.Categories = categories;
                var result = Check.Methods.CheckValidation(ModelState, Request, false);
                if (result != null) { return result; }
                return View("Index", model);
            }
        }
        public IActionResult Delete(int id)
        {
            var item = unitOfWork.CategoryRepository.GetOne(e => e.CategoryId == id);
            unitOfWork.CategoryRepository.Delete(item);
            return RedirectToAction("Index");

        }

    }
}
