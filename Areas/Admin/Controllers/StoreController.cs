using Demo.Models;
using Demo.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StoreController : Controller
    {
        private readonly IunitOfWork _unitOfWork;

        public StoreController(IunitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            // Get all stores and pass them to the view
            var stores = _unitOfWork.StoreRepository.Get(includeProperties:e=>e.Stocks).ToList();
            return View(stores);
        }

        public IActionResult Create()
        {
            // Create a new Store model and pass it to the view
            return PartialView("_CreateStorePartialView");
        }

        [HttpPost]
        public IActionResult Create(Store store)
        {
            if (ModelState.IsValid)
            {
                var result = Check.Methods.CheckValidation(ModelState, Request, true);
                if (result!=null) { return result; }
                _unitOfWork.StoreRepository.Create(store);
               
                return RedirectToAction("Index");
            }
            else
            {

            var result = Check.Methods.CheckValidation(ModelState, Request, false);
            if (result != null) { return result; }

            return View("Index", store);//posibale erorr but check validation handel it 
            }
        }

        public IActionResult Edit(int id)
        {
            // Find the store by id and pass it to the edit view
            var store = _unitOfWork.StoreRepository.GetOne(s => s.StoreId == id);
            if (store == null)
            {
                return NotFound();
            }
            return PartialView("_EditStorePartialView", store);
        }

        [HttpPost]
        public IActionResult Edit(Store store)
        {
            if (ModelState.IsValid)
            {
                var result = Check.Methods.CheckValidation(ModelState, Request, true);
                if (result != null) { return result; }
                _unitOfWork.StoreRepository.Edit(store);
               
                return RedirectToAction("Index");
            }
            else
            {
                var result = Check.Methods.CheckValidation(ModelState, Request, false);
                if (result != null) { return result; }
                return PartialView("_EditStorePartialView", store);
            }
        }

        public IActionResult Delete(int id)
        {
            var store = _unitOfWork.StoreRepository.GetOne(s => s.StoreId == id);
            if (store == null)
            {
                return NotFound();
            }

            _unitOfWork.StoreRepository.Delete(store.StoreId);
           
            return RedirectToAction("Index");
        }
    }
}
