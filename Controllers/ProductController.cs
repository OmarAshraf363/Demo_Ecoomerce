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

        public ProductController( IunitOfWork unitOfWork)
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
        public IActionResult ProductsCategory( int id)
        {
            return View(unitOfWork.ProductRepository.getAllProductsWithspacifsCategory(id));
        }
        public IActionResult Details(int id, ProductDetails model)
        {
            var item =unitOfWork.ProductRepository.GetOne(e=>e.ProductId==id,e=>e.Brand);
            model.Product = item;
            var Q =unitOfWork.StockRepository.GetOne(e=>e.ProductId==id);
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
              var productId= unitOfWork.ProductRepository.createFromViewModel(model);//create and return Id to complete anotherMethod
                if (Request.Headers["X-Requested-With"]== "XMLHttpRequest")
                {
                    return Json(new {isvalid=true});
                }
                
                return RedirectToAction("AddProductToStock", new {id= productId });
            }
            else
            {
                unitOfWork.ProductRepository.putAllInfoInProductViewModel(model);
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
            unitOfWork.ProductRepository.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            
            return View();
        }





        //public IActionResult Edit(int id)
        //{
        //    var product = unitOfWork.ProductRepository.GetAll().AsQueryable().Include(e=>e.Category).Include(e=>e.Brand).SingleOrDefault(e=>e.ProductId==id);
        //    ViewBag.brands = unitOfWork.BrandRepository.GetAll();
        //    ViewBag.cats = unitOfWork.CategoryRepository.GetAll();
        //    return View(product);
           
        //}
        //[HttpPost]
        //public IActionResult Edit(ProductsViewModels model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        unitOfWork.ProductRepository.editFromViewModel(model);
        //         return RedirectToAction("Index"); 
        //    }
        //    else
        //    {
        //        return View(model);
        //    }
        //}
        public IActionResult AddProductToStock(int id)
        {
 
            Stock stock = new Stock()
            {
                StoreId = 1,
                ProductId = id,
                Quantity=5
                
            };
           unitOfWork.StockRepository.Create(stock);
            return RedirectToAction("Index");
        }
        






    }
}
