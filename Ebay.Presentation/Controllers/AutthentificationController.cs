using Ebay.Presentation.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ebay.Presentation.Controllers
{
    public class AutthentificationController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        public AutthentificationController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return RedirectToAction(nameof(LogIn));
        }

        public IActionResult LogIn(string? errorMessage = null)
        {
            AppUserViewModel appUser = new AppUserViewModel() { ErrorMessage = errorMessage};
            return View(appUser);
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(string username, string password)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(username);
                var userRole = await _userManager.IsInRoleAsync(user, "Admin");

                if(userRole == true)
                {
                    if (user != null)
                    {
                        var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);
                        if (signInResult.Succeeded)
                        {
                            if (signInResult.Succeeded)
                            {
                                return RedirectToAction(nameof(Index), nameof(AdminController));
                            }
                        }
                    }
                }
                else
                {
                    return RedirectToAction(nameof(LogIn), "You do not have admin rights to enter the system");
                }
            }
            return RedirectToAction(nameof(LogIn));
        }
    }
}
