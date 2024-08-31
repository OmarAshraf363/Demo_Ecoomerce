using Demo.Models;
using Demo.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Demo.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class WishListController : Controller
    {
        private readonly IunitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public WishListController(IunitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            var wishList = _unitOfWork.WishListRepository.GetOne(e => e.AppUserId == userId);
            if (wishList==null)
            {
                wishList = new WishList() { AppUserId = userId, CreatedAt = DateTime.Now };
                _unitOfWork.WishListRepository.Create(wishList);
            }
            var wishListItems = _unitOfWork.WishListItemsRepository.Get(e => e.WishListId == wishList.Id, e => e.Product);
            if (wishListItems == null)
            {
                return View( new List<WishListItems>() );
            }
            return View(wishListItems);
        }

        public  IActionResult AddToWishList(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var wishList = _unitOfWork.WishListRepository.GetOne(e => e.AppUserId == userId, e => e.WishListItems);


            if (wishList == null)
            {
                wishList = new WishList { AppUserId = userId };

                 _unitOfWork.WishListRepository.Create(wishList);
            }

            if (!wishList.WishListItems.Any(wi => wi.ProductId == productId))
            {
                var wishListItem = new WishListItems
                {
                    WishListId = wishList.Id,
                    ProductId = productId
                };
                _unitOfWork.WishListItemsRepository.Create(wishListItem);
               
            }

            return RedirectToAction("Index","Home");
        }
        public  IActionResult RemoveFromWishList(int id , string? returnUrl)
        {
            var wishListItem = _unitOfWork.WishListItemsRepository.GetOne(e => e.ProductId == id);
            if (wishListItem != null)
            {
               _unitOfWork.WishListItemsRepository.Delete(wishListItem);
            }
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult ToggleWishList(int id)
        {
            var wishList = _unitOfWork.WishListRepository.GetOne(e => e.AppUserId == _userManager.GetUserId(User));
            if (wishList == null)
            {
                wishList = new WishList()
                {
                    AppUserId = _userManager.GetUserId(User),
                    CreatedAt = DateTime.Now,

                };
                _unitOfWork.WishListRepository.Create(wishList);
                var wishListItem= _unitOfWork.WishListItemsRepository.GetOne(e => e.ProductId == id);
                if(wishListItem != null)
                {
                    _unitOfWork.WishListItemsRepository.Delete(wishListItem);

                       return Json(new { isInWishList = false });
                }
                else
                {
                    _unitOfWork.WishListItemsRepository.Create(new WishListItems { WishListId = wishList.Id, ProductId = id });

                       return Json(new { isInWishList = true });
                }

            }
            else
            {
                var wishListItem = _unitOfWork.WishListItemsRepository.GetOne(e => e.ProductId == id);

                if (wishListItem != null)
                {
                    _unitOfWork.WishListItemsRepository.Delete(wishListItem);

                    return Json(new { isInWishList = false });
                }
                else
                {

                    _unitOfWork.WishListItemsRepository.Create(new WishListItems { WishListId = wishList.Id, ProductId = id });

                    return Json(new { isInWishList = true });
                }
            }




        }

    }
}
