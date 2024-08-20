using Demo.Data;
using Demo.Models;
using Demo.Repository.ModelsRepository.BrandModel;
using Demo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Controllers
{
    public class BrandController : Controller
    {
        private readonly IBrandRepository brandRepository;

        public BrandController(IBrandRepository brandRepository)
        {
            this.brandRepository = brandRepository;
        }

        public IActionResult Index()
        {
            var brands = brandRepository.GetAll().AsQueryable().Include(e => e.Products).ToList();
            BrandViewModels model = new BrandViewModels()
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
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var brands = brandRepository.GetAll().Where(e => e.BrandName == model.BrandName).SingleOrDefault();
                    if (brands != null)
                    {
                        ModelState.AddModelError("", "Existed");
                        return Json(new { isvalid = false, errors = "Existed", type = "one" });
                    }
                    else
                    {
                        brandRepository.AddFromViewModel(model);
                        return Json(new { isvalid = true });
                    }
                }
                return RedirectToAction("Index");
            }
            else
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var errors = ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
                    return Json(new { isvalid = false, errors = errors, type = "one" });
                }
                return View("Index", model);
            }
        }
        public IActionResult Delete(int id)
        {
            brandRepository.Delete(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Edit(BrandViewModels model)
        {
            if (ModelState.IsValid)
            {
                var brand = brandRepository.GetById(model.BrandId);
                if (brand != null)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        var brands = brandRepository.GetAll().Where(e => e.BrandName == model.BrandName).SingleOrDefault();
                        if (brands == null)
                        {
                            brand.BrandName = model.BrandName;
                            brandRepository.Edit(brand);
                            return Json(new { isvalid = true });
                        }
                        else
                        {
                            ModelState.AddModelError("", "Existed");
                            return Json(new { isvalid = false, errors = "Existed", type = "one" });
                        }

                    }
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
                    var errors = ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage);
                    return Json(new { isvalid = false, errors = errors, typr = "one" });

                }
                return View("Index", model);
            }

        }
    }
}
