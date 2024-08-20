using Demo.Data;
using Demo.Models;
using Demo.Repository.ModelsRepository.CategoryModel;
using Demo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Controllers
{
    public class CategoryController : Controller
    {

       private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
           
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            var categories= _categoryRepository.GetAll().AsQueryable().Include(e=>e.Products).ToList();
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
                    var category=_categoryRepository.GetAll().Where(e=>e.CategoryName == model.CategoryName).SingleOrDefault();
                    if(category != null)
                    {
                        ModelState.AddModelError("", "Existed");
                        return Json(new { isvalid = false ,errors="Existed",type="one" });
                    }
                    else
                    {
                        _categoryRepository.AddFromViewModel(model);
                        return Json(new{isvalid = true});
                    }
                }
                return RedirectToAction("Index");
            }
            else
            {
                var categories = _categoryRepository.GetAll().AsQueryable().Include(e => e.Products).ToList();
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
                    var category=_categoryRepository.GetById(model.CategoryId);

                    var Existed=_categoryRepository.GetAll().Where(e=>e.CategoryName == model.CategoryName).SingleOrDefault();
                    if (Existed != null)
                    {
                        ModelState.AddModelError("", "Existed");
                        return Json(new { isvalid = false, errors = "Existed", type = "one" });
                    }
                    else
                    {
                        category.CategoryName=model.CategoryName;
                        _categoryRepository.Edit(category);
                    return Json(new { isvalid = true });
                    }
                }
                return RedirectToAction("Index");
            }
            else
            {

                var categories = _categoryRepository.GetAll().AsQueryable().Include(e => e.Products).ToList();
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
            _categoryRepository.Delete(id);
            return RedirectToAction("Index");
        
        }

    }
}
