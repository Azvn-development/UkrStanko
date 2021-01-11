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
using UkrStanko.ViewModels.Machines;

namespace UkrStanko.Controllers
{
    [Authorize(Roles = "Администратор")]
    public class MachinesController : Controller
    {
        private readonly IMachineRepository _machineRepository;
        private readonly IMachineTypeRepository _machineTypeRepository;
        private readonly IReadedNoticeRepository _readedNoticeRepository;
        private readonly IHubContext<NotificationHub> _hub;

        // Push up уведомление
        private string _message;

        // Конструктор
        public MachinesController(IMachineRepository machineRepository,
            IMachineTypeRepository machineTypeRepository,
            IReadedNoticeRepository readedNoticeRepository,
            IHubContext<NotificationHub> hub)
        {
            _machineRepository = machineRepository;
            _machineTypeRepository = machineTypeRepository;
            _readedNoticeRepository = readedNoticeRepository;
            _hub = hub;
        } // MachinesController

        // Получение списка элементов
        public async Task<IActionResult> Index(bool ajaxLoad)
        {
            var viewModel = new IndexViewModel(await _machineRepository.GetMachines());

            if (!ajaxLoad) return View(viewModel);
            return PartialView(viewModel);
        } // Index

        // Добавление элемента GET
        [HttpGet]
        public async Task<IActionResult> Create(bool ajaxLoad)
        {
            var viewModel = new CreateViewModel(
                await _machineRepository.GetMachines(),
                await _machineTypeRepository.GetMachineTypes());

            if (!ajaxLoad) return View(viewModel);
            return PartialView(viewModel);
        } // Create

        // Добавление элемента POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateViewModel viewModel, string connectionId)
        {
            // Индикатор добавления группы для станка
            bool groupAdded = false;

            //Добавление элемента в базу
            try
            {
                //Проверка введенных данных
                if (ModelState.IsValid)
                {
                    // Добавление группы станков при отсутствии аналога
                    if (viewModel.MachineTypeId == 0)
                    {
                        viewModel.MachineTypeId = await _machineTypeRepository.AddMachineType(viewModel.MachineType);
                        groupAdded = true;
                    } // if

                    await _machineRepository.AddMachine(new Machine { Name = viewModel.Name, MachineTypeId = viewModel.MachineTypeId });

                    await _hub.Clients.Client(connectionId).SendAsync("Success", Messages.successCreateMessage);

                    return RedirectToAction("Index", new { ajaxLoad = true });
                } //if
            } 
            catch (DbUpdateException ex) { _message = DbErrorsInterpreter.GetDbUpdateExceptionMessage(ex); } 
            catch { _message = Messages.createErrorMessage; } // try-catch

            // Удаление созданной группы для станка
            if (groupAdded) await _machineTypeRepository.RemoveMachineType(viewModel.MachineTypeId);

            // Подгрузка вспомогательных коллекция
            viewModel.Machines = await _machineRepository.GetMachines();
            viewModel.MachineTypes = await _machineTypeRepository.GetMachineTypes();

            if (_message != null) await _hub.Clients.Client(connectionId).SendAsync("Error", _message);
            return PartialView(viewModel);
        } // Create

        // Редактирование элемента GET
        [HttpGet]
        public async Task<IActionResult> Edit(int id, bool ajaxLoad, string connectionId)
        {
            try
            {
                var machine = await _machineRepository.GetMachine(id);
                if (machine == null) throw new Exception();

                var viewModel = new EditViewModel(
                    machine,
                    await _machineRepository.GetAnalogueName(machine),
                    await _machineRepository.GetMachines(),
                    await _machineTypeRepository.GetMachineTypes());

                if (!ajaxLoad) return View(viewModel);
                return PartialView(viewModel);
            }
            catch { _message = Messages.downloadErrorMessage; } // try-catch

            if (_message != null) await _hub.Clients.Client(connectionId).SendAsync("Error", _message);
            return RedirectToAction("Index", new { ajaxLoad = true });
        } // Edit

        // Редактирование элемента POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditViewModel viewModel, string connectionId)
        {
            // Индикатор добавления группы для станка
            bool groupAdded = false;

            //Редактирование элемента в базе
            try
            {
                //Проверка введенных данных
                if (ModelState.IsValid)
                {
                    // Добавление группы станков при отсутствии аналога или уже созданной группы
                    if (viewModel.MachineTypeId == 0)
                    {
                        viewModel.MachineTypeId = await _machineTypeRepository.AddMachineType(viewModel.MachineType);
                        groupAdded = true;
                    } // if

                    // Изменение станка
                    var machine = await _machineRepository.GetMachine(viewModel.Id);
                    machine.Name = viewModel.Name;
                    machine.MachineTypeId = viewModel.MachineTypeId;

                    await _machineRepository.EditMachine(machine);
                    await _machineTypeRepository.CheckEmptyGroups();

                    await _hub.Clients.Client(connectionId).SendAsync("Success", Messages.successUpdateMessage);
                    return RedirectToAction("Index", new { ajaxLoad = true });
                } //if
            }
            catch (DbUpdateException ex) { _message = DbErrorsInterpreter.GetDbUpdateExceptionMessage(ex); }
            catch { _message = Messages.updateErrorMessage; } // try-catch

            // Удаление созданной группы для станка
            if (groupAdded) await _machineTypeRepository.RemoveMachineType(viewModel.MachineTypeId);

            viewModel.Machines = await _machineRepository.GetMachines();
            viewModel.MachineTypes = await _machineTypeRepository.GetMachineTypes();

            if (_message != null) await _hub.Clients.Client(connectionId).SendAsync("Error", _message);
            return PartialView(viewModel);
        } // Edit

        // Удаление элемента AJAX
        public async Task<bool> DeleteAJAX(int id, string connectionId)
        {
            // Удаление элемента из базы
            try
            {
                // Кол-во заявок и предложений по удаляемому станку
                var requisitionsCount = await _machineRepository.GetRequisitionsCount(id);
                var proposalsCount = await _machineRepository.GetProposalsCount(id);

                await _machineRepository.RemoveMachine(id);

                // Уменьшение кол-ва прочитанных новостей
                await _readedNoticeRepository.DecrementUsersReadedNoticesCounter(requisitionsCount + proposalsCount);

                // Проверка зависимых элементов
                await _machineTypeRepository.CheckEmptyGroups();

                await _hub.Clients.All.SendAsync("Success", Messages.successDeleteMessage);

                return true;
            } 
            catch { 
                await _hub.Clients.Client(connectionId).SendAsync("Error", Messages.deleteErrorMessage);

                return false;
            } // try-catch
        } // DeleteAJAX

        // Поиск элемента AJAX
        public async Task<ActionResult> SearchAJAX(string searchString)
        {
            List<Machine> machines;

            if (string.IsNullOrEmpty(searchString)) machines = await _machineRepository.GetMachines();
            else
            {
                machines = await _machineRepository.GetFilteredMachines(searchString);
                ViewBag.Download = false;
            } // if

            return PartialView("_Table", machines);
        } // SearchAJAX

        // Подгрузка элементов AJAX
        public async Task<ActionResult> DownloadAJAX(int downloadedCount)
        {
            var machines = await _machineRepository.GetMachines(downloadedCount);
            ViewBag.DownloadedCount = downloadedCount + machines.Count;

            return PartialView("_TBody", machines);
        } // DownloadAJAX
    }
}
