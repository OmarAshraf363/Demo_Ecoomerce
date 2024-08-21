using Demo.Data;
using Demo.Models;
using Demo.Repository.IRepository;
using Demo.Repository.ModelsRepository.BrandModel;
using Demo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Controllers
{
    public class BrandController : Controller
    {
        private readonly IunitOfWork unitOfWork;


        public BrandController(IunitOfWork unitOfWork)
        {

            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            //var brands = unitOfWork.BrandRepository.GetAll().AsQueryable().Include(e => e.Products).ToList();
            var brands = unitOfWork.BrandRepository.Get(includeProperties: e => e.Products).ToList();
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
                    var brands = unitOfWork.BrandRepository.GetOne(expression:e=>e.BrandName==model.BrandName);
                    if (brands != null)
                    {
                        ModelState.AddModelError("", "Existed");
                        return Json(new { isvalid = false, errors = "Existed", type = "one" });
                    }
                    else
                    {
                        unitOfWork.BrandRepository.AddFromViewModel(model);
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
            unitOfWork.BrandRepository.Delete(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Edit(BrandViewModels model)
        {
            if (ModelState.IsValid)
            {
                var brand = unitOfWork.BrandRepository.GetOne(e=>e.BrandId==model.BrandId);
                if (brand != null)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        var brands = unitOfWork.BrandRepository.GetOne(expression: e => e.BrandName == model.BrandName);
                        if (brands == null)
                        {
                            brand.BrandName = model.BrandName;
                            unitOfWork.BrandRepository.Edit(brand);
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
