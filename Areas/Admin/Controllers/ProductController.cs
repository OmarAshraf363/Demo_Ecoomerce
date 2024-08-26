﻿using Azure.Core;
using Demo.Models;
using Demo.Repository.IRepository;
using Demo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {

        private readonly IunitOfWork unitOfWork;

        public ProductController(IunitOfWork unitOfWork)
        {


            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index(string productName)
        {
            ProductsViewModels model = new ProductsViewModels();
           model= unitOfWork.ProductRepository.putAllInfoInProductViewModel(model);
            if (!string.IsNullOrEmpty(productName))
            {
                var loweerProductName = productName.ToLower();
                model.Products=model.Products.Where(e=>e.ProductName.ToLower().IndexOf(loweerProductName, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }
            ViewBag.CurrentFilterProductName=productName;
            return View(model);//check ProductsView Models
        }


        public IActionResult MobCat(int id) => View(unitOfWork.ProductRepository.getAllProductsWithspacifsCategory(id)); //check Categoet Add to

       
        public IActionResult Details(int id, ProductDetails model)
        {
            var item = unitOfWork.ProductRepository.GetOne(e => e.ProductId == id, e => e.Brand,expression=>expression.Category);
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
                Brands = unitOfWork.BrandRepository.Get().ToList(),
                Stores=unitOfWork.StoreRepository.Get().ToList(),
            };
            return PartialView("_AddPartialView", model);
        }
        [HttpPost]
        public IActionResult Create(ProductsViewModels model)
        {
            if (ModelState.IsValid)
            {
                var result = Check.Methods.CheckValidation(ModelState, Request, true);
                if (result != null) { return result; }
                var productId = unitOfWork.ProductRepository.createFromViewModel(model);//create and return Id to complete anotherMethod



                return RedirectToAction("AddProductToStock", new { id = productId , quantity=model.Quantity , storeId = model.StoreId});
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
            var item = unitOfWork.ProductRepository.GetOne(e => e.ProductId == id);
            
            unitOfWork.ProductRepository.Delete(item);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = unitOfWork.ProductRepository.GetOne(e => e.ProductId == id);
            ViewBag.Categories = unitOfWork.CategoryRepository.Get().ToList();
            ViewBag.Brands = unitOfWork.BrandRepository.Get().ToList();

            return PartialView("_EditPartialView", product);
        }

        [HttpPost]
        public IActionResult Edit(ProductsViewModels model)
        {
            if (ModelState.IsValid)
            {
                var result = Check.Methods.CheckValidation(ModelState, Request, true);
                if (result != null) { return result; }
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
        public IActionResult AddProductToStock(int id,int quantity,int storeId)
        {

            var stock = new Stock()
            {
                StoreId =storeId ,
                ProductId = id,
                Quantity = quantity

            };
            unitOfWork.StockRepository.Create(stock);
            return RedirectToAction("Index");
        }







    }
}