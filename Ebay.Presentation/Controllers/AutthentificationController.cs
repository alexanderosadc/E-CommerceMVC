﻿using Ebay.Domain.Entities;
using Ebay.Infrastructure.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ebay.Presentation.Controllers
{
    public class AutthentificationController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        public AutthentificationController(
            SignInManager<User> signInManager,
            UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return RedirectToAction(nameof(LogIn));
        }

        public IActionResult LogIn()
        {
            AppUserLogInDTO appUser = new AppUserLogInDTO();
            return View(appUser);
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(string username, string password)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(username);
                
                if (user != null)
                {
                    var isAdmin = await _userManager.IsInRoleAsync(user, "admin");
                    var isModerator = await _userManager.IsInRoleAsync(user, "moderator");
                    if (isAdmin == true || isModerator == true)
                    {
                        var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);
                        if (signInResult.Succeeded)
                        {
                            return RedirectToAction(nameof(Index), "Admin");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "You do not have admin rights to enter the system");
                    }
                } 
            }
            return RedirectToAction(nameof(LogIn));
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
