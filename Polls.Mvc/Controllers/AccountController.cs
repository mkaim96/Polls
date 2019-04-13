using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Polls.Infrastructure.Ef;
using Polls.Mvc.Models;

namespace Polls.Mvc.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = string.IsNullOrEmpty(model.UserName) ? model.Email : model.UserName,
                    Email = model.Email,
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.PasswordSignInAsync(user, model.Password, true, false);

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);

            }
            // executes if registration failed, displays registration form again 
            return View(model);
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.ToString());
            }
        }

        #endregion
    }
}