using Demo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index(string userName)
        {
           
            List<AppUser> appusers = new List<AppUser>();
           var users=_userManager.Users.ToList();
            if (users.Any())
            {
                foreach (var user in users)
                {

                    appusers.Add(user as AppUser);
                }

                if (!string.IsNullOrEmpty(userName))
                {
                    var lowerUserName=userName.ToLower();
                    appusers=appusers.Where(e=>e.UserName.ToLower().IndexOf(lowerUserName,StringComparison.OrdinalIgnoreCase)>=0).ToList();
                    ViewBag.CurrentFilterUserName=userName;
                return View(appusers);
                }
            }
            return View(appusers);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        public async Task<IActionResult> LockUser(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Lock out the user until a specific date
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> UnlockUser(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Unlock the user
            await _userManager.SetLockoutEndDateAsync(user, null);

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return NotFound();
                }
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
