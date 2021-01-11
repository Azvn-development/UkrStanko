using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;

using UkrStanko.Models.Security;
using UkrStanko.ViewModels.Home;

namespace UkrStanko.Controllers
{
    public class HomeController : Controller
    {
        // Управление пользователями приложения
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        // Конструктор
        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        } // HomeController

        //GET: Страница входа
        public IActionResult Login()
        {
            return View();
        } //Login

        //POST: Страница входа
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(viewModel.Login, viewModel.Password, true, false);
                if (!result.Succeeded) { ModelState.AddModelError("", "Неправильный логин или пароль!"); }
                else { return RedirectToAction("Index", "Notices"); } //if
            } //if

            return View();
        } //Login

        // GET: Страница смены пароля
        public async Task<IActionResult> Password()
        {
            if(User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                var viewModel = new PasswordViewModel(user.UserName, user.Email);
                return View(viewModel);
            } else
            {
                var viewModel = new PasswordViewModel(null, null);
                return View(viewModel);
            } // if
        } // Password

        // POST: Страница смены пароля
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Password(PasswordViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(viewModel.Login);

                if(user == null || user.Email != viewModel.Email)
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и e-mail не существует!");

                    return View(viewModel);
                } // if

                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, viewModel.Password);

                var result = await _userManager.UpdateAsync(user);
                if(result.Succeeded) return RedirectToAction("Index", "Notices");
            } // if

            return View(viewModel);
        } // Password

        //GET: Выход из приложения
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        } //Logout
    }
}