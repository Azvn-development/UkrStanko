using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

using UkrStanko.Models;
using UkrStanko.Models.App;
using UkrStanko.Models.App.Interfaces;
using UkrStanko.Models.Hubs;
using UkrStanko.ViewModels.Requisitions;

namespace UkrStanko.Controllers
{
    [Authorize]
    public class RequisitionsController : Controller
    {
        private readonly IRequisitionRepository _requisitionRepository;
        private readonly IProposalRepository _proposalRepository;
        private readonly IMachineRepository _machineRepository;
        private readonly IMachineTypeRepository _machineTypeRepository;
        private readonly IReadedNoticeRepository _readedNoticeRepository;
        private readonly IHubContext<NotificationHub> _hub;

        // Push up уведомление
        private string _message;

        // Конструктор
        public RequisitionsController(IRequisitionRepository requisitionRepository, 
            IProposalRepository proposalRepository, 
            IMachineRepository machineRepository, 
            IMachineTypeRepository machineTypeRepository,
            IReadedNoticeRepository readedNoticeRepository,
            IHubContext<NotificationHub> hub)
        {
            _requisitionRepository = requisitionRepository;
            _proposalRepository = proposalRepository;
            _machineRepository = machineRepository;
            _machineTypeRepository = machineTypeRepository;
            _readedNoticeRepository = readedNoticeRepository;
            _hub = hub;
        } // RequisitionsController

        // Получение списка элементов
        public async Task<IActionResult> Index(bool ajaxLoad)
        {
            var viewModel = new IndexViewModel(await _requisitionRepository.GetRequisitions());

            if (!ajaxLoad) return View(viewModel);
            return PartialView(viewModel);
        } // Index

        // Получение детальной информации об элементе
        public async Task<IActionResult> Details(int id, bool ajaxLoad, string connectionId, string previousUrl)
        {
            try
            {
                var requisition = await _requisitionRepository.GetRequisition(id);
                if (requisition == null) throw new Exception();

                var viewModel = new DetailsViewModel(
                    requisition,
                    await _proposalRepository.GetProposals(requisition.Machine));

                if (!ajaxLoad) return View(viewModel);
                return PartialView(viewModel);
            }
            catch {
                await _hub.Clients.Client(connectionId).SendAsync("Error", Messages.downloadErrorMessage);

                if (previousUrl == null) return RedirectToAction("Index", new { ajaxLoad = true });
                return Redirect(previousUrl + "?ajaxLoad=true");
            } // try-catch
        } // Details

        // Добавление элемента GET
        [HttpGet]
        public async Task<IActionResult> Create(bool ajaxLoad, string phone, string contactName, string location)
        {
            var viewModel = new CreateViewModel(
                    phone,
                    contactName,
                    location,
                    await _machineRepository.GetMachines());

            if (!ajaxLoad) return View(viewModel);
            return PartialView(viewModel);
        } // Create

        // Добавление элемента POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateViewModel viewModel, string connectionId, bool more)
        {
            //Добавление элемента в базу
            try
            {
                // Проверка номера телефона на кол-во цифр
                Regex regex = new Regex(@"\D+");
                viewModel.Phone = regex.Replace(viewModel.Phone, "");
                if (viewModel.Phone.Length < 7) ModelState.AddModelError("Phone", Messages.phoneMinLengthErrorMessage);

                //Проверка введенных данных
                if (ModelState.IsValid)
                { 
                    // Добавление станка при отсутствии в базе
                    var machine = await _machineRepository.GetMachine(viewModel.Machine);
                    if (machine == null) machine = await _machineRepository.AddMachine(viewModel.Machine, await _machineTypeRepository.AddMachineType("Без группы"));

                    // Добавляемый элемент
                    var requisition = new Requisition
                    {
                        UserName = User.Identity.Name,
                        CreateDate = DateTime.Now,
                        Phone = viewModel.Phone,
                        ContactName = viewModel.ContactName,
                        Location = viewModel.Location,
                        MachineId = machine.Id,
                        Comment = viewModel.Comment
                    };

                    await _requisitionRepository.AddRequisition(requisition);
                    await _hub.Clients.Client(connectionId).SendAsync("Success", Messages.successCreateMessage);

                    // Добавление дополнительной записи
                    if(more)
                    {
                        return RedirectToAction("Create", new { 
                            ajaxLoad = true, 
                            phone = viewModel.Phone,
                            contactName = viewModel.ContactName, 
                            location = viewModel.Location });
                    } // if

                    return RedirectToAction("Details", new { id = requisition.Id, ajaxLoad = true });
                } //if
            }
            catch (DbUpdateException ex) { _message = DbErrorsInterpreter.GetDbUpdateExceptionMessage(ex); }
            catch { _message = Messages.createErrorMessage; } // try-catch

            viewModel.Machines = await _machineRepository.GetMachines();

            if (_message != null) await _hub.Clients.Client(connectionId).SendAsync("Error", _message);
            return PartialView(viewModel);
        } // Create

        // Редактирование элемента GET
        [HttpGet]
        public async Task<IActionResult> Edit(int id, bool ajaxLoad, string connectionId, string previousUrl)
        {
            try
            {
                var requisition = await _requisitionRepository.GetRequisition(id);
                if (requisition == null) throw new Exception();

                var viewModel = new EditViewModel(
                    requisition,
                    await _machineRepository.GetMachines());

                if (!ajaxLoad) return View(viewModel);
                return PartialView(viewModel);
            }
            catch { _message = Messages.downloadErrorMessage; } // try-catch

            if (_message != null) await _hub.Clients.Client(connectionId).SendAsync("Error", _message);

            if (previousUrl == null) return RedirectToAction("Index", new { ajaxLoad = true });
            return Redirect(previousUrl + "?ajaxLoad=true");
            
        } // Edit

        // Редактирование элемента POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditViewModel viewModel, string connectionId)
        {
            //Редактирование элемента в базе
            try
            {
                // Проверка номера телефона на кол-во цифр
                Regex regex = new Regex(@"\D+");
                viewModel.Phone = regex.Replace(viewModel.Phone, "");
                if (viewModel.Phone.Length < 7) ModelState.AddModelError("Phone", Messages.phoneMinLengthErrorMessage);

                //Проверка введенных данных
                if (ModelState.IsValid)
                {
                    // Добавление станка при отсутствии в базе
                    var machine = await _machineRepository.GetMachine(viewModel.Machine);
                    if (machine == null) machine = await _machineRepository.AddMachine(viewModel.Machine, await _machineTypeRepository.AddMachineType("Без группы"));

                    // Редактируемый элемент
                    var requisition = await _requisitionRepository.GetRequisition(viewModel.Id);
                    requisition.Phone = viewModel.Phone;
                    requisition.ContactName = viewModel.ContactName;
                    requisition.Location = viewModel.Location;
                    requisition.MachineId = machine.Id;
                    requisition.Comment = viewModel.Comment;

                    await _requisitionRepository.EditRequisition(requisition);
                    await _hub.Clients.Client(connectionId).SendAsync("Success", Messages.successUpdateMessage);

                    return RedirectToAction("Details", new { id = requisition.Id, ajaxLoad = true });
                } //if
            }
            catch (DbUpdateException ex) { _message = DbErrorsInterpreter.GetDbUpdateExceptionMessage(ex); }
            catch { _message = Messages.updateErrorMessage; } // try-catch

            viewModel.Machines = await _machineRepository.GetMachines();

            if (_message != null) await _hub.Clients.Client(connectionId).SendAsync("Error", _message);

            return PartialView(viewModel);
        } // Edit

        // Удаление элемента AJAX
        public async Task<ActionResult> DeleteAJAX(int id, bool details, string connectionId, string searchString, DateTime? startDate, DateTime? endDate, bool ownRecords)
        {
            // Удаление элемента из базы
            try
            {
                await _requisitionRepository.RemoveRequisition(id);

                // Декрементирование счетчика новостей для всех пользователей
                await _readedNoticeRepository.DecrementUsersReadedNoticesCounter();

                await _hub.Clients.Client(connectionId).SendAsync("Success", Messages.successDeleteMessage);
            } 
            catch { await _hub.Clients.Client(connectionId).SendAsync("Error", Messages.deleteErrorMessage); } // try-catch

            if (details) return RedirectToAction("Index", new { ajaxLoad = true });
            return PartialView("_Table", await _requisitionRepository.GetFilteredRequisitions(searchString, startDate, endDate, ownRecords, User.Identity.Name));
        } // DeleteAJAX

        // Поиск элемента AJAX
        public async Task<ActionResult> SearchAJAX(string searchString, DateTime? startDate, DateTime? endDate, bool ownRecords)
        {
            return PartialView("_Table", await _requisitionRepository.GetFilteredRequisitions(searchString, startDate, endDate, ownRecords, User.Identity.Name));
        } // SearchAJAX
    }
}
