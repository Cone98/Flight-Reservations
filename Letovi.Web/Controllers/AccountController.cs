using Letovi.Web.Data;
using Letovi.Web.Models.Domain;
using Letovi.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Letovi.Web.Controllers
{
    
    public class AccountController : Controller
    {      
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(SignInManager<IdentityUser> signInManager)
        {          
            this.signInManager = signInManager;
        }
        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            var signInResult = await signInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password, false, false);
            if (signInResult.Succeeded && signInResult != null)
            {
                
                return RedirectToAction("Index","Home");
            }
            else
            {
                ViewData["ErrorMessage"] = "Niste uneli ispravne podatke, probajte ponovo!";
                return View(loginViewModel);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
