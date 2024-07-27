using Demo.Data;
using Demo.Models;
using Demo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Controllers
{
    public class ProductController : Controller
    {
        AppDbContext context = new AppDbContext();
        public IActionResult Index(ProductsViewModels model)
        {
            var products = context.Products.Include(e => e.Category).Include(e => e.Brand).ToList();
            var categories = context.Categories.ToList();
            var brans = context.Brands.ToList();
            model.Products = products;
            model.Categories = categories;
            model.Brands = brans;
            return View(model);
        }
        public IActionResult MobCat(int id)
        {
            var AllMob = context.Products.Include(e => e.Category).Include(e=>e.Brand).Where(e => e.Category.CategoryId == id).ToList();
            var category = context.Categories.Find(id);
            var brans = context.Brands.ToList();
            CategoryAddToProduct model = new CategoryAddToProduct()
            {
                Products = AllMob,
                CategoryId = id,
                catname=category.CategoryName,
                Brands= brans
                

            };
            return View(model);
        }
        public IActionResult ProductsCategory( int id)
        {


            var AllMob = context.Products.Include(e => e.Category).Include(e => e.Brand).Where(e => e.Category.CategoryId == id).ToList();
            var category = context.Categories.Find(id);
            var brans = context.Brands.ToList();
            CategoryAddToProduct model = new CategoryAddToProduct()
            {
                Products = AllMob,
                CategoryId = id,
                catname = category.CategoryName,
                Brands = brans


            };
            return View(model);
        }
        public IActionResult Details(int id, ProductDetails product)
        {
            var item = context.Products.Include(e => e.Brand).SingleOrDefault(e => e.ProductId == id);
            product.Product = item;
            var Q = context.Stocks.Where(e => e.ProductId == id).SingleOrDefault();

            if (Q != null)
            {
                product.Quantity = Q.Quantity;
            }
            else { product.Quantity = 0; }
            return View(product);
        }


        [HttpPost]
        public IActionResult Create(ProductsViewModels model)
        {
            if (ModelState.IsValid)
            {
                Product product = new Product()
                {
                    ProductName = model.ProductName,
                    ProductDescription = model.ProductDescription,
                    Image = model.Image,
                    ListPrice = model.ListPrice,
                    BrandId = model.BrandId,
                    CategoryId = model.CategoryId,
                    ModelYear = model.ModelYear,
                    Rate = model.Rate,


                };
                if (Request.Headers["X-Requested-With"]== "XMLHttpRequest")
                {
                    return Json(new {isvalid=true});
                }
                context.Products.Add(product);
                context.SaveChanges();
                return RedirectToAction("AddProductToStock", new {id=product.ProductId});
            }
            else
            {
                var products = context.Products.Include(e => e.Category).Include(e => e.Brand).ToList();
                var categories = context.Categories.ToList();
                var brans = context.Brands.ToList();
                model.Products = products;
                model.Categories = categories;
                model.Brands = brans;
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    return Json(new { isvalid = false , errors= errors.LastOrDefault() });
                }
                return View("Index",model);
            }
        }
        public IActionResult Delete(int id)
        {
            var product=context.Products.Find(id);
            if (product != null)
            {
                context.Products.Remove(product);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return BadRequest();
            }
        }
        public IActionResult Edit(int id)
        {
            var product = context.Products.Include(e=>e.Category).Include(e=>e.Brand).SingleOrDefault(e=>e.ProductId==id);
            if (product != null)
            {
                  var model = new ProductsViewModels
                {
                   ProductId = id,
                    ProductName = product.ProductName,
                    ProductDescription = product.ProductDescription,
                    Image = product.Image,
                    ListPrice = product.ListPrice,
                    BrandId = product.BrandId,
                    CategoryId = product.CategoryId,
                    ModelYear = product.ModelYear,
                    Rate = product.Rate,
                    Categories = context.Categories.ToList(),
                    Brands = context.Brands.ToList()
                };
                return View(model);
            }
            { 
            return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult Edit(ProductsViewModels model)
        {
            if (ModelState.IsValid)
            {
                var product = context.Products.Where(e => e.ProductId == model.ProductId).SingleOrDefault();

                if (product != null)
                {
                    product.ProductName = model.ProductName;
                    product.ProductDescription = model.ProductDescription;
                    product.BrandId = model.BrandId;
                    product.CategoryId = model.CategoryId;
                    product.ModelYear = model.ModelYear;
                    product.Rate = model.Rate;
                    product.Image = model.Image;
                    product.ListPrice = model.ListPrice;
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }
        public IActionResult AddProductToStock(int id)
        {
           
            Stock stock = new Stock()
            {
                StoreId = 1,
                ProductId = id,
                Quantity=5
                
            };
            context.Stocks.Add(stock);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        






    }
}
