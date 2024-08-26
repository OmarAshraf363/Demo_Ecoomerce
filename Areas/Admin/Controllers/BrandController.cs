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
        private readonly RoleManager<IdentityRole> role ;

        public BrandController(IunitOfWork unitOfWork, RoleManager<IdentityRole> role)
        {

            this.unitOfWork = unitOfWork;
            this.role = role;
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
                    var brands = unitOfWork.BrandRepository.GetOne(expression: e => e.BrandName == model.BrandName);
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
                    return Json(new { isvalid = false, errors, type = "one" });
                }
                return View("Index", model);
            }
        }
        public IActionResult Delete(int id)
        {
            var item=unitOfWork.BrandRepository.GetOne(e=>e.BrandId == id);
            unitOfWork.BrandRepository.Delete(item);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Edit(BrandViewModels model)
        {
            if (ModelState.IsValid)
            {
                var brand = unitOfWork.BrandRepository.GetOne(e => e.BrandId == model.BrandId);
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
                    return Json(new { isvalid = false, errors, typr = "one" });

                }
                return View("Index", model);
            }

        }
        public async Task<IActionResult> addroles()
        {
           await role.CreateAsync(new IdentityRole("Admin"));
            await role.CreateAsync(new IdentityRole("Customer"));
            return Content("addedd");
        }
    }
}
