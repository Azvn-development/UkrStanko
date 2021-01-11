using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;

using UkrStanko.Models;
using UkrStanko.Models.App.Interfaces;
using UkrStanko.Models.Hubs;
using UkrStanko.Models.Security;
using UkrStanko.ViewModels.Users;

namespace UkrStanko.Controllers
{
    
    public class UsersController : Controller
    {
        // Управление пользователями приложения
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        // Сервисы работы с базой данных
        private readonly IRequisitionRepository _requisitionRepository;
        private readonly IProposalRepository _proposalRepository;
        private readonly IUserImageRepository _userImageRepository;
        private readonly IReadedNoticeRepository _readedNoticeRepository;
        private readonly IHubContext<NotificationHub> _hub;

        // Веб среда
        private readonly IWebHostEnvironment _environment;

        // Push up уведомление
        private string _message;

        // Конструктор
        public UsersController(UserManager<AppUser> userManager, 
            RoleManager<AppRole> roleManager,
            SignInManager<AppUser> signInManager,
            IRequisitionRepository requisitionRepository,
            IProposalRepository proposalRepository,
            IUserImageRepository userImageRepository,
            IReadedNoticeRepository readedNoticeRepository,
            IWebHostEnvironment environment,
            IHubContext<NotificationHub> hub)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _requisitionRepository = requisitionRepository;
            _proposalRepository = proposalRepository;
            _userImageRepository = userImageRepository;
            _readedNoticeRepository = readedNoticeRepository;
            _environment = environment;
            _hub = hub;
        } // UsersController

        // Получение списка пользователей
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Index(bool ajaxLoad)
        {
            var viewModel = new IndexViewModel(await _userManager.Users.Where(i => i.UserName != "root").ToListAsync());

            if (!ajaxLoad) return View(viewModel);
            return PartialView(viewModel);
        } // Index

        // Добавление пользователя GET
        [Authorize(Roles = "Администратор")]
        [HttpGet]
        public async Task<ActionResult> Create(bool ajaxLoad)
        {
            var viewModel = new CreateViewModel(await _roleManager.Roles.ToListAsync());

            if (!ajaxLoad) return View(viewModel);
            return PartialView(viewModel);
        } // Create

        // Добавление роли пользователя POST
        [Authorize(Roles = "Администратор")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateViewModel viewModel, string connectionId)
        {
            //Добавление элемента в базу
            try
            {
                //Проверка введенных данных
                if (ModelState.IsValid)
                {
                    // Добавление нового пользователя в базу
                    var user = new AppUser { 
                        UserName = viewModel.UserName, 
                        Email = viewModel.Email, 
                        Role = viewModel.Role };
                    var result = await _userManager.CreateAsync(user, viewModel.Password);
                    if(result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, user.Role);
                        await _userImageRepository.AddUserImage(user.UserName, viewModel.Image, _environment);

                        // Добавление счетчика прочитанных новостей
                        await _readedNoticeRepository.AddUserReadedNoticesCounter(user.Id);

                        return RedirectToAction("Index", new { ajaxLoad = true });
                    } else
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        } // foreach

                        // Подгрузка вспомогательных коллекций
                        viewModel.Roles = await _roleManager.Roles.ToListAsync();

                        return PartialView(viewModel);
                    } // if
                } //if
            }
            catch (DbUpdateException ex) { _message = DbErrorsInterpreter.GetDbUpdateExceptionMessage(ex); }
            catch { _message = Messages.createErrorMessage; } // try-catch

            // Подгрузка вспомогательных коллекций
            viewModel.Roles = await _roleManager.Roles.ToListAsync();

            // Отправка push up уведомления при наличии исключения
            if(_message != null) if (_message != null) await _hub.Clients.Client(connectionId).SendAsync("Error", _message);

            return PartialView(viewModel);
        } // Create

        // Редактирование пользователя GET
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Edit(string name, bool ajaxLoad, string connectionId, string previousUrl)
        {
            try
            {
                if (name == "own") name = User.Identity.Name;

                var user = await _userManager.FindByNameAsync(name);
                if (user == null) throw new Exception();

                var viewModel = new EditViewModel(
                    user,
                    await _userImageRepository.GetUserImagePath(user.UserName),
                    await _roleManager.Roles.ToListAsync());

                if (!ajaxLoad) return View(viewModel);
                return PartialView(viewModel);
            }
            catch { _message = Messages.downloadErrorMessage; } // try-catch

            if (_message != null) await _hub.Clients.Client(connectionId).SendAsync("Error", _message);
            return Redirect(previousUrl + "?ajaxLoad=true");
        } // Edit

        // Редактирование пользователя POST
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditViewModel viewModel, string connectionId, string previousUrl)
        {
            //Редактирование элемента в базе
            try
            {
                //Проверка введенных данных
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByIdAsync(viewModel.Id);

                    var oldUserName = user.UserName; // старое имя пользователя

                    user.UserName = viewModel.UserName;
                    user.Email = viewModel.Email;
                    user.Role = viewModel.Role;

                    // Обновление пользователя
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        // Роли в которых состоял пользователь
                        var roles = await _userManager.GetRolesAsync(user);
                        if (roles[0] != user.Role)
                        {
                            await _userManager.RemoveFromRolesAsync(user, roles);
                            await _userManager.AddToRoleAsync(user, user.Role);
                        } // if

                        // Проверка замены фотографии пользователя
                        if(viewModel.ImagePath == null)
                        {
                            // Удаление загруженного изображения пользователя
                            try
                            {
                                if(oldUserName != user.UserName) await _userImageRepository.RemoveUserImage(oldUserName, _environment);
                                else await _userImageRepository.RemoveUserImage(user.UserName, _environment);
                            } catch { }

                            // Загрузка нового изображения пользователя
                            if(viewModel.Image != null)
                            {
                                await _userImageRepository.AddUserImage(user.UserName, viewModel.Image, _environment);
                            } // if
                        } // if

                        // Изменение заявок и преложений при редактировании имени пользователя
                        if (oldUserName != user.UserName)
                        {
                            await _requisitionRepository.EditRequisitions(oldUserName, user.UserName);
                            await _proposalRepository.EditProposal(oldUserName, user.UserName);
                            await _userImageRepository.EditUserImage(oldUserName, user.UserName, _environment);
                        } // if
                    } else
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        } // foreach

                        // Подгрузка вспомогательных коллекций
                        viewModel.Roles = await _roleManager.Roles.ToListAsync();
                        ViewBag.PreviousUrl = previousUrl;

                        return PartialView(viewModel);
                    } // if

                    if(User.Identity.Name == oldUserName)
                    {
                         await _signInManager.SignOutAsync();
                         await _signInManager.SignInAsync(user, true);
                    } // if

                    await _hub.Clients.Client(connectionId).SendAsync("Success", Messages.successUpdateMessage);
                    return Redirect(previousUrl + $"?ajaxLoad=true");
                } // if
            }
            catch (DbUpdateException ex) { _message = DbErrorsInterpreter.GetDbUpdateExceptionMessage(ex); }
            catch { _message = Messages.updateErrorMessage; } // try-catch

            // Подгрузка вспомогательных коллекций
            viewModel.Roles = await _roleManager.Roles.ToListAsync();
            ViewBag.PreviousUrl = previousUrl;

            if (_message != null) await _hub.Clients.Client(connectionId).SendAsync("Error", _message);
            return PartialView(viewModel);
        } // Edit

        // Удаление пользователя AJAX
        [Authorize(Roles = "Администратор")]
        public async Task<ActionResult> DeleteAJAX(string id, string connectionId)
        {
            try
            {
                // Удаляемый пользователь
                var user = await _userManager.FindByIdAsync(id);
                var result = await _userManager.DeleteAsync(user);

                if(result.Succeeded)
                {
                    // Удаление заявок и предложений, созданных пользователем
                    await _proposalRepository.RemoveProposals(user.UserName);
                    await _requisitionRepository.RemoveRequisitions(user.UserName);

                    // Удаление фото пользователя из базы
                    await _userImageRepository.RemoveUserImage(user.UserName, _environment);

                    // Удаление счетчика прочитанных новостей
                    await _readedNoticeRepository.RemoveUserReadedNoticesCounter(user.Id);

                    await _hub.Clients.Client(connectionId).SendAsync("Success", Messages.successDeleteMessage);
                    return PartialView("_Table", await _userManager.Users.Where(i => i.UserName != "root").ToListAsync());
                } else
                {
                    throw new Exception();
                } // if
            }
            catch
            {
                await _hub.Clients.Client(connectionId).SendAsync("Error", Messages.deleteErrorMessage);
                return PartialView("_Table", await _userManager.Users.Where(i => i.UserName != "root").ToListAsync());
            } //try-catch
        } // DeleteAJAX

        // Поиск типа оборудования AJAX
        [Authorize(Roles = "Администратор")]
        public async Task<ActionResult> SearchAJAX(string searchString)
        {
            var users = await _userManager.Users
                .Where(i => (searchString == null || (i.UserName + i.Email + i.Role).Contains(searchString)) && i.UserName != "root")
                .ToListAsync();

            return PartialView("_Table", users);
        } // SearchAJAX
    }
}