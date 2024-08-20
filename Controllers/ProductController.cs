using Demo.Data;
using Demo.Models;
using Demo.Repository.ModelsRepository.BrandModel;
using Demo.Repository.ModelsRepository.CategoryModel;
using Demo.Repository.ModelsRepository.ProductModel;
using Demo.Repository.ModelsRepository.StockModel;
using Demo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Controllers
{
    public class ProductController : Controller
    {
       
        IProductRepository _productRepository;
        IStockRepository _stockRepository;
        IBrandRepository _brandRepository;
        ICategoryRepository _categoryRepository;

        public ProductController(AppDbContext context, IProductRepository productRepository, IStockRepository stockRepository, IBrandRepository brandRepository, ICategoryRepository categoryRepository)
        {
           
            _productRepository = productRepository;
            _stockRepository = stockRepository;
            _brandRepository = brandRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            ProductsViewModels model = new ProductsViewModels();
            return View(_productRepository.putAllInfoInProductViewModel(model));//check ProductsView Models
        }

        
        public IActionResult MobCat(int id)
        {
            return View(_productRepository.getAllProductsWithspacifsCategory(id)); //check Categoet Add to
        }
        public IActionResult ProductsCategory( int id)
        {
            return View(_productRepository.getAllProductsWithspacifsCategory(id));
        }
        public IActionResult Details(int id, ProductDetails model)
        {
            var item =_productRepository.GetAll().AsQueryable().Include(e=>e.Brand).FirstOrDefault(e=>e.ProductId==id);
            model.Product = item;
            var Q =_stockRepository.GetAll().FirstOrDefault(e=>e.ProductId==id);
            if (Q != null)
            {
                model.Quantity = Q.Quantity;
            }
            else { model.Quantity = 0; }
            return View(model);
        }


        [HttpPost]
        public IActionResult Create(ProductsViewModels model)
        {
            if (ModelState.IsValid)
            {
              var productId= _productRepository.createFromViewModel(model);//create and return Id to complete anotherMethod
                if (Request.Headers["X-Requested-With"]== "XMLHttpRequest")
                {
                    return Json(new {isvalid=true});
                }
                
                return RedirectToAction("AddProductToStock", new {id= productId });
            }
            else
            {
                _productRepository.putAllInfoInProductViewModel(model);
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
            _productRepository.Delete(id);
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var product = _productRepository.GetAll().AsQueryable().Include(e=>e.Category).Include(e=>e.Brand).SingleOrDefault(e=>e.ProductId==id);
            ViewBag.brands = _brandRepository.GetAll();
            ViewBag.cats = _categoryRepository.GetAll();
            return View(product);
           
        }
        [HttpPost]
        public IActionResult Edit(ProductsViewModels model)
        {
            if (ModelState.IsValid)
            {
                _productRepository.editFromViewModel(model);
                 return RedirectToAction("Index"); 
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
           _stockRepository.Create(stock);
            return RedirectToAction("Index");
        }
        






    }
}
