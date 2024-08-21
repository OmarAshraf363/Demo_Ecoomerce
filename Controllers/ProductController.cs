using Azure.Core;
using Demo.Models;
using Demo.Repository.IRepository;
using Demo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Controllers
{
    public class ProductController : Controller
    {

        private readonly IunitOfWork unitOfWork;

        public ProductController(IunitOfWork unitOfWork)
        {


            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            ProductsViewModels model = new ProductsViewModels();
            return View(unitOfWork.ProductRepository.putAllInfoInProductViewModel(model));//check ProductsView Models
        }


        public IActionResult MobCat(int id)
        {
            return View(unitOfWork.ProductRepository.getAllProductsWithspacifsCategory(id)); //check Categoet Add to
        }
        public IActionResult ProductsCategory(int id)
        {
            return View(unitOfWork.ProductRepository.getAllProductsWithspacifsCategory(id));
        }
        public IActionResult Details(int id, ProductDetails model)
        {
            var item = unitOfWork.ProductRepository.GetOne(e => e.ProductId == id, e => e.Brand);
            model.Product = item;
            var Q = unitOfWork.StockRepository.GetOne(e => e.ProductId == id);
            if (Q != null)
            {
                model.Quantity = Q.Quantity;
            }
            else { model.Quantity = 0; }
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new ProductsViewModels
            {
                Categories = unitOfWork.CategoryRepository.Get().ToList(),
                Brands = unitOfWork.BrandRepository.Get().ToList()
            };
            return PartialView("_AddPartialView", model);
        }
        [HttpPost]
        public IActionResult Create(ProductsViewModels model)
        {
            if (ModelState.IsValid)
            {
                var productId = unitOfWork.ProductRepository.createFromViewModel(model);//create and return Id to complete anotherMethod


                var result = Check.Methods.CheckValidation(ModelState, Request, true);
                if (result != null) { return result; }

                return RedirectToAction("AddProductToStock", new { id = productId });
            }
            else
            {
                unitOfWork.ProductRepository.putAllInfoInProductViewModel(model);
                var result = Check.Methods.CheckValidation(ModelState, Request, false);
                if (result != null) { return result; }
                return View("Index", model);
            }
        }
        public IActionResult Delete(int id)
        {
            unitOfWork.ProductRepository.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product=unitOfWork.ProductRepository.GetOne(e=>e.ProductId==id);
            ViewBag.Categories = unitOfWork.CategoryRepository.Get().ToList();
            ViewBag.Brands = unitOfWork.BrandRepository.Get().ToList();
            return PartialView("_EditPartialView",product);
        }

        [HttpPost]
        public IActionResult Edit(ProductsViewModels model)
        {
            if (ModelState.IsValid)
            {
                var result=Check.Methods.CheckValidation(ModelState,Request, true);
                if(result != null) { return result; }
                unitOfWork.ProductRepository.editFromViewModel(model);
                return RedirectToAction("Index");
            }
            else
            {
                var result = Check.Methods.CheckValidation(ModelState, Request, false);
                if (result != null) { return result; }
                return View(model);
            }
        }
        public IActionResult AddProductToStock(int id)
        {

            Stock stock = new Stock()
            {
                StoreId = 1,
                ProductId = id,
                Quantity = 5

            };
            unitOfWork.StockRepository.Create(stock);
            return RedirectToAction("Index");
        }







    }
}
