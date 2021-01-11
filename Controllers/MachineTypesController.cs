using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;

using UkrStanko.Models;
using UkrStanko.Models.App;
using UkrStanko.Models.App.Interfaces;
using UkrStanko.Models.Hubs;
using UkrStanko.ViewModels.MachineTypes;

namespace UkrStanko.Controllers
{
    [Authorize(Roles = "Администратор")]
    public class MachineTypesController : Controller
    {
        private readonly IMachineTypeRepository _machineTypesRepository;
        private readonly IReadedNoticeRepository _readedNoticeRepository;
        private readonly IHubContext<NotificationHub> _hub;

        // Push up уведомление
        private string _message;

        // Конструктор
        public MachineTypesController(IMachineTypeRepository machineTypesRepository,
            IReadedNoticeRepository readedNoticeRepository,
            IHubContext<NotificationHub> hub)
        {
            _machineTypesRepository = machineTypesRepository;
            _readedNoticeRepository = readedNoticeRepository;
            _hub = hub;
        } // MachineTypesController

        // Получение списка элементов
        public async Task<IActionResult> Index(bool ajaxLoad)
        {
            var viewModel = new IndexViewModel(await _machineTypesRepository.GetMachineTypes());

            if (!ajaxLoad) return View(viewModel);
            return PartialView(viewModel);
        } // Index

        // Добавление элемента GET
        [HttpGet]
        public IActionResult Create(bool ajaxLoad)
        {
            var viewModel = new CreateViewModel();

            if (!ajaxLoad) return View(viewModel);
            return PartialView(viewModel);
        } // Create

        // Добавление элемента POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateViewModel viewModel, string connectionId)
        {
            //Добавление элемента в базу
            try
            {
                //Проверка введенных данных
                if (ModelState.IsValid)
                {
                    await _machineTypesRepository.AddMachineType(new MachineType { Name = viewModel.Name });
                    await _hub.Clients.Client(connectionId).SendAsync("Success", Messages.successCreateMessage);

                    return RedirectToAction("Index", new { ajaxLoad = true });
                } //if
            }
            catch (DbUpdateException ex) { _message = DbErrorsInterpreter.GetDbUpdateExceptionMessage(ex); }
            catch { _message = Messages.createErrorMessage; } // try-catch

            if (_message != null) await _hub.Clients.Client(connectionId).SendAsync("Error", _message);
            return PartialView(viewModel);
        } // Create

        // Редактирование элемента GET
        [HttpGet]
        public async Task<IActionResult> Edit(int id, bool ajaxLoad, string connectionId)
        {
            try
            {
                var machineType = await _machineTypesRepository.GetMachineType(id);
                if (machineType == null) throw new Exception();

                var viewModel = new EditViewModel(machineType);

                if (!ajaxLoad) return View(viewModel);
                return PartialView(viewModel);
            }
            catch { _message = Messages.downloadErrorMessage; } // try-catch

            if (_message != null) await _hub.Clients.Client(connectionId).SendAsync("Error", _message);
            return RedirectToAction("Index", new { ajaxLoad = true});
        } // Edit

        // Редактирование элемента POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditViewModel viewModel, string connectionId)
        {
            //Редактирование элемента в базе
            try
            {
                //Проверка введенных данных
                if (ModelState.IsValid)
                {
                    var machineType = await _machineTypesRepository.GetMachineType(viewModel.Id);
                    machineType.Name = viewModel.Name;

                    await _machineTypesRepository.EditMachineType(machineType);
                    await _hub.Clients.Client(connectionId).SendAsync("Success", Messages.successUpdateMessage);

                    return RedirectToAction("Index", new { ajaxLoad = true });
                } //if
            }
            catch (DbUpdateException ex) { _message = DbErrorsInterpreter.GetDbUpdateExceptionMessage(ex); }
            catch { _message = Messages.updateErrorMessage; } // try-catch

            if (_message != null) await _hub.Clients.Client(connectionId).SendAsync("Error", _message);
            return PartialView(viewModel);
        } // Edit

        // Удаление элемента AJAX
        public async Task<ActionResult> DeleteAJAX(int id, string searchString, string connectionId)
        {
            // Удаление элемента из базы
            try
            {
                // Получение кол-ва заявок и предложений по удаляемой группе
                var requisitionsCount = await _machineTypesRepository.GetRequisitionsCount(id);
                var proposalsCount = await _machineTypesRepository.GetProposalsCount(id);

                await _machineTypesRepository.RemoveMachineType(id);

                // Уменьшение кол-ва прочитанных новостей
                await _readedNoticeRepository.DecrementUsersReadedNoticesCounter(requisitionsCount + proposalsCount);

                await _hub.Clients.Client(connectionId).SendAsync("Success", Messages.successDeleteMessage);
            } 
            catch { await _hub.Clients.Client(connectionId).SendAsync("Error", Messages.deleteErrorMessage); } // try-catch

            return PartialView("_Table", await _machineTypesRepository.GetFilteredMachineTypes(searchString));
        } // DeleteAJAX

        // Поиск элемента AJAX
        public async Task<ActionResult> SearchAJAX(string searchString)
        {
            return PartialView("_Table", await _machineTypesRepository.GetFilteredMachineTypes(searchString));
        } // SearchAJAX
    }
}
