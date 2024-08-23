using Demo.Models;
using Demo.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StockController : Controller
    {
        private readonly IunitOfWork _unitOfWork;

        public StockController(IunitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(string storeName ,string productName)
        {
            // Retrieve all stocks with related Store and Product data
            var stocks = _unitOfWork.StockRepository.Get(null, e=>e.Store,e=>e.Product).ToList();

            // Apply filters if they are provided
            if (!string.IsNullOrEmpty(storeName))
            {
                stocks = stocks.Where(s => s.Store.StoreName.Contains(storeName)).ToList();
            }

            if (!string.IsNullOrEmpty(productName))
            {
                stocks = stocks.Where(s => s.Product.ProductName.Contains(productName)).ToList();
            }

            // Pass the current filter values to the view for display
            ViewBag.CurrentFilterStoreName = storeName;
            ViewBag.CurrentFilterProductName = productName;

            return View(stocks);
        }

        //public IActionResult Create()
        //{
            
        //    return PartialView("_CreateStockPartialView");
        //}

        //[HttpPost]
        //public IActionResult Create(Stock stock)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result=Check.Methods.CheckValidation(ModelState,Request,true);
        //        if (result != null) { return result; }
        //        _unitOfWork.StockRepository.Create(stock);
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        var result = Check.Methods.CheckValidation(ModelState, Request, false);
        //        if (result != null) { return result; }
        //        return PartialView("_CreateStockPartialView", stock);
        //    }
        //}

        public IActionResult Edit(int storeId, int productId)
        {
            var stock = _unitOfWork.StockRepository.GetOne(s => s.StoreId == storeId && s.ProductId == productId);
            if (stock == null)
            {
                return NotFound();
            }
            return PartialView("_EditStockPartialView", stock);
        }

        [HttpPost]
        public IActionResult Edit(Stock stock)
        {
            if (ModelState.IsValid)
            {
                var result = Check.Methods.CheckValidation(ModelState, Request, true);
                if (result != null) { return result; }
                _unitOfWork.StockRepository.Edit(stock);
                return RedirectToAction("Index");
            }
            else
            {
                var result = Check.Methods.CheckValidation(ModelState, Request, false);
                if (result != null) { return result; }
            return PartialView("_EditStockPartialView", stock);
            }
        }

        public IActionResult Delete(int storeId, int productId)
        {
            var stock = _unitOfWork.StockRepository.GetOne(s => s.StoreId == storeId && s.ProductId == productId);
            if (stock == null)
            {
                return NotFound();
            }

            _unitOfWork.StockRepository.Delete(stock.StoreId);
            return RedirectToAction("Index");
        }
    }
}
