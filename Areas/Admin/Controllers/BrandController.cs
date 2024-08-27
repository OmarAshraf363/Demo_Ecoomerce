using Demo.Data;
using Demo.Models;
using Demo.Repository.IRepository;
using Demo.Repository.ModelsRepository.BrandModel;
using Demo.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly IunitOfWork unitOfWork;
        private readonly RoleManager<IdentityRole> role;

        public BrandController(IunitOfWork unitOfWork, RoleManager<IdentityRole> role)
        {

            this.unitOfWork = unitOfWork;
            this.role = role;
        }

        public IActionResult Index(string brandName)
        {
            var brands = unitOfWork.BrandRepository.Get(includeProperties: e => e.Products).ToList();
            if (!string.IsNullOrEmpty(brandName))
            {
                var lowerBrandName = brandName.ToLower();
                brands = brands.Where(e => e.BrandName.ToLower().IndexOf(brandName, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }
            BrandViewModels model = new BrandViewModels()
            {
                Brands = brands
            };
            ViewBag.CurrentFilterBrandName = brandName;
            return View(model);
        }
        [HttpPost]
        public IActionResult Create(BrandViewModels model)
        {
            if (ModelState.IsValid)
            {

                var brands = unitOfWork.BrandRepository.GetOne(expression: e => e.BrandName == model.Name);
                if (brands != null)
                {
                    ModelState.AddModelError("Name", "Existed");
                    var result = Check.Methods.CheckValidation(ModelState, Request, false);
                    if (result != null) { return result; }
                }
                else
                {
                    var result = Check.Methods.CheckValidation(ModelState, Request, true);
                    if (result != null) { return result; }
                    var brand = new Brand()
                    {
                        BrandName = model.Name,
                    };
                    unitOfWork.BrandRepository.Create(brand);
                }
                return RedirectToAction("Index");
            }
            else
            {
                var result = Check.Methods.CheckValidation(ModelState, Request, false);
                if (result != null) { return result; }
                return View("Index", model);
            }
        }
        public IActionResult Delete(int id)
        {
            var item = unitOfWork.BrandRepository.GetOne(e => e.BrandId == id);
            unitOfWork.BrandRepository.Delete(item);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var brand =unitOfWork.BrandRepository.GetOne(e=>e.BrandId==id);
            var model = new BrandViewModels()
            {
                BrandId = brand.BrandId,
                Name = brand.BrandName
            };
            return PartialView("_EditBrandPartial", model);
        }


        [HttpPost]
        public IActionResult Edit(BrandViewModels model)
        {
            if (ModelState.IsValid)
            {
                var brand = unitOfWork.BrandRepository.GetOne(e => e.BrandId == model.BrandId);
                if (brand != null)
                {
                    
                    var existedBrand = unitOfWork.BrandRepository.GetOne(expression: e => e.BrandName == model.Name);
                    if (existedBrand != null)
                    {
                        ModelState.AddModelError("Name", "Existed");
                        var result = Check.Methods.CheckValidation(ModelState, Request, false);
                        if (result != null) { return result; }

                    }
                    else
                    {
                        var result = Check.Methods.CheckValidation(ModelState, Request, true);
                        if (result != null) { return result; }
                        brand.BrandName = model.Name;
                        unitOfWork.BrandRepository.Edit(brand);
                      
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
                var result = Check.Methods.CheckValidation(ModelState, Request, false);
                if (result != null) { return result; }
                return View("Index", model);
            }

        }
        //public async Task<IActionResult> addroles()
        //{
        //   await role.CreateAsync(new IdentityRole("Admin"));
        //    await role.CreateAsync(new IdentityRole("Customer"));
        //    return Content("addedd");
        //}
    }
}
