using Demo.Data;
using Demo.Models;
using Demo.Repository.IRepository;
using Demo.Repository.ModelsRepository.CategoryModel;
using Demo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Controllers
{
    public class CategoryController : Controller
    {

        private readonly IunitOfWork unitOfWork;


        public CategoryController(IunitOfWork unitOfWork)
        {

            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var categories= unitOfWork.CategoryRepository.Get(includeProperties:e=>e.Products).ToList();
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
                    var category=unitOfWork.CategoryRepository.GetOne(expression: e => e.CategoryName == model.CategoryName);
                    if(category != null)
                    {
                        ModelState.AddModelError("", "Existed");
                        return Json(new { isvalid = false ,errors="Existed",type="one" });
                    }
                    else
                    {
                        unitOfWork.CategoryRepository.AddFromViewModel(model);
                        return Json(new{isvalid = true});
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
                    return Json(new { isvalid = false, errors = errors });
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
                    var category=unitOfWork.CategoryRepository.GetOne(e=>e.CategoryId==model.CategoryId);

                    var Existed=unitOfWork.CategoryRepository.GetOne(expression: e => e.CategoryName == model.CategoryName);
                    if (Existed != null)
                    {
                        ModelState.AddModelError("", "Existed");
                        return Json(new { isvalid = false, errors = "Existed", type = "one" });
                    }
                    else
                    {
                        category.CategoryName=model.CategoryName;
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
                    return Json(new { isvalid = false, errors = errors });
                }
                return View("Index", model);
            }
        }
        public IActionResult Delete(int id) 
        {
            unitOfWork.CategoryRepository.Delete(id);
            return RedirectToAction("Index");
        
        }

    }
}
