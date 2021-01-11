using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using System.IO;

using UkrStanko.Models;
using UkrStanko.Models.App;
using UkrStanko.Models.App.Interfaces;
using UkrStanko.Models.Hubs;
using UkrStanko.ViewModels.Proposals;

namespace UkrStanko.Controllers
{

    public class ProposalsController : Controller
    {
        // Репозитории
        private readonly IProposalRepository _proposalRepository;
        private readonly IRequisitionRepository _requisitionRepository;
        private readonly IProposalImageRepository _proposalImageRepository;
        private readonly IMachineRepository _machineRepository;
        private readonly IMachineTypeRepository _machineTypeRepository;
        private readonly IReadedNoticeRepository _readedNoticeRepository;
        private readonly IHubContext<NotificationHub> _hub;

        // Веб среда
        private readonly IWebHostEnvironment _environment;

        // Push up уведомление
        private string _message;

        // Конструктор
        public ProposalsController(IProposalRepository proposalRepository, 
            IRequisitionRepository requisitionRepository,
            IProposalImageRepository proposalImageRepository,
            IMachineRepository machineRepository, 
            IMachineTypeRepository machineTypeRepository,
            IReadedNoticeRepository readedNoticeRepository,
            IWebHostEnvironment environment,
            IHubContext<NotificationHub> hub)
        {
            _proposalRepository = proposalRepository;
            _requisitionRepository = requisitionRepository;
            _proposalImageRepository = proposalImageRepository;
            _machineRepository = machineRepository;
            _machineTypeRepository = machineTypeRepository;
            _readedNoticeRepository = readedNoticeRepository;
            _environment = environment;
            _hub = hub;
        } // ProposalsController

        // Получение списка элементов
        [Authorize]
        public async Task<IActionResult> Index(bool ajaxLoad)
        {
            var viewModel = new IndexViewModel(await _proposalRepository.GetProposals());

            if (!ajaxLoad) return View(viewModel);
            return PartialView(viewModel);
        } // Index

        // Получение детальной информации об элементе
        public async Task<IActionResult> Details(int id, bool ajaxLoad, string connectionId, string previousUrl)
        {
            try
            {
                var proposal = await _proposalRepository.GetProposal(id);
                if (proposal == null) throw new Exception();

                var viewModel = new DetailsViewModel(
                    proposal,
                    await _proposalImageRepository.GetProposalImagesPath(id),
                    await _requisitionRepository.GetRequisitions(proposal.Machine));

                if (!ajaxLoad) return View(viewModel);
                return PartialView(viewModel);
            }
            catch { _message = Messages.downloadErrorMessage; } // try-catch

            await _hub.Clients.Client(connectionId).SendAsync("Error", _message);

            if (previousUrl == null) return RedirectToAction("Index", new { ajaxLoad = true });
            return Redirect(previousUrl + "?ajaxLoad=true");
        } // Details

        // Добавление элемента GET
        [Authorize]
        [HttpGet]
        public IActionResult Create(string location, bool ajaxLoad)
        {
            var viewModel = new CreateViewModel(location);

            if (!ajaxLoad) return View(viewModel);
            return PartialView(viewModel);
        } // Create

        // Добавление элемента POST
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateViewModel viewModel, string connectionId, bool more)
        {
            //Добавление элемента в базу
            try
            {
                // Проверка количества загруженных фото
                if(viewModel.Images != null && viewModel.Images.Count > 7) ModelState.AddModelError("", "Максимальное кол-во фото в предложении - 7 шт.");

                //Проверка введенных данных
                if (ModelState.IsValid)
                {
                    // Добавление станка при отсутствии в базе
                    var machine = await _machineRepository.GetMachine(viewModel.Machine);
                    if (machine == null) machine = await _machineRepository.AddMachine(viewModel.Machine, await _machineTypeRepository.AddMachineType("Без группы"));

                    // Добавляемый элемент
                    var proposal = new Proposal
                    {
                        UserName = User.Identity.Name,
                        CreateDate = DateTime.Now,
                        Location = viewModel.Location,
                        MachineId = machine.Id,
                        PurshasePrice = (int)viewModel.PurshasePrice,
                        SellingPrice = (int)viewModel.SellingPrice,
                        Comment = viewModel.Comment
                    };

                    await _proposalRepository.AddProposal(proposal);

                    try
                    {
                        await _proposalImageRepository.AddProposalImages(proposal.Id, viewModel.Images, _environment);
                    } catch { }

                    await _hub.Clients.Client(connectionId).SendAsync("Success", Messages.successCreateMessage);

                    // Добавление дополнительной записи
                    if(more)
                    {
                        return RedirectToAction("Create", new {
                            ajaxLoad = true,
                            location = viewModel.Location
                        });
                    } // if

                    return RedirectToAction("Details", new { id = proposal.Id, ajaxLoad = true });
                } //if
            }
            catch (DbUpdateException ex) { _message = DbErrorsInterpreter.GetDbUpdateExceptionMessage(ex); }
            catch { _message = Messages.createErrorMessage; } // try-catch

            if(_message != null) if (_message != null) await _hub.Clients.Client(connectionId).SendAsync("Error", _message);
            return PartialView(viewModel);
        } // Create

        // Редактирование элемента GET
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int id, bool ajaxLoad, string connectionId, string previousUrl)
        {
            try
            {
                var proposal = await _proposalRepository.GetProposal(id);
                if (proposal == null) throw new Exception();

                var viewModel = new EditViewModel(
                    proposal,
                    await _proposalImageRepository.GetProposalImagesPath(id));

                if (!ajaxLoad) return View(viewModel);
                return PartialView(viewModel);
            }
            catch { _message = Messages.downloadErrorMessage; } // try-catch

            if (_message != null) await _hub.Clients.Client(connectionId).SendAsync("Error", _message);

            if (previousUrl == null) return RedirectToAction("Index", new { ajaxLoad = true });
            return Redirect(previousUrl + "?ajaxLoad=true");
        } // Edit

        // Редактирование элемента POST
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditViewModel viewModel, string connectionId)
        {
            //Редактирование элемента в базе
            try
            {
                // Проверка количества загруженных фото
                if ((viewModel.Images != null ? viewModel.Images.Count : 0) + 
                    (viewModel.ProposalImages != null ? viewModel.ProposalImages.Count : 0) > 7) 
                    ModelState.AddModelError("", "Максимальное кол-во фото в предложении - 7 шт.");

                //Проверка введенных данных
                if (ModelState.IsValid)
                {
                    // Добавление станка при отсутствии в базе
                    var machine = await _machineRepository.GetMachine(viewModel.Machine);
                    if (machine == null) machine = await _machineRepository.AddMachine(viewModel.Machine, await _machineTypeRepository.AddMachineType("Без группы"));

                    // Добавляемый элемент
                    var proposal = await _proposalRepository.GetProposal(viewModel.Id);
                    proposal.Location = viewModel.Location;
                    proposal.MachineId = machine.Id;
                    proposal.PurshasePrice = (int)viewModel.PurshasePrice;
                    proposal.SellingPrice = (int)viewModel.SellingPrice;
                    proposal.Comment = viewModel.Comment;

                    await _proposalRepository.EditProposal(proposal);

                    try
                    {
                        await _proposalImageRepository.CheckProposalImages(proposal.Id, viewModel.ProposalImages, _environment);
                        await _proposalImageRepository.AddProposalImages(proposal.Id, viewModel.Images, _environment);
                    }
                    catch { }

                    await _hub.Clients.Client(connectionId).SendAsync("Success", Messages.successUpdateMessage);

                    return RedirectToAction("Details", new { id = proposal.Id, ajaxLoad = true });
                } //if
            }
            catch (DbUpdateException ex) { _message = DbErrorsInterpreter.GetDbUpdateExceptionMessage(ex); }
            catch { _message = Messages.updateErrorMessage; } // try-catch

            if (_message != null) await _hub.Clients.Client(connectionId).SendAsync("Error", _message);
            return PartialView(viewModel);
        } // Edit

        // Удаление элемента AJAX
        [Authorize]
        public async Task<ActionResult> DeleteAJAX(int id, bool details, string connectionId, string searchString, DateTime? startDate, DateTime? endDate, bool ownRecords)
        {
            // Удаление элемента из базы
            try
            {
                await _proposalImageRepository.RemoveProposalImages(id, _environment);
                await _proposalRepository.RemoveProposal(id);

                // Декрементирование счетчика новостей для всех пользователей
                await _readedNoticeRepository.DecrementUsersReadedNoticesCounter();

                await _hub.Clients.Client(connectionId).SendAsync("Success", Messages.successDeleteMessage);
            } 
            catch { await _hub.Clients.Client(connectionId).SendAsync("Error", Messages.deleteErrorMessage); } // try-catch

            if (details) return RedirectToAction("Index", new { ajaxLoad = true });
            return PartialView("_Table", await _proposalRepository.GetFilteredProposals(searchString, startDate, endDate, ownRecords, User.Identity.Name));
        } // DeleteAJAX

        // Поиск элемента AJAX
        [Authorize]
        public async Task<ActionResult> SearchAJAX(string searchString, DateTime? startDate, DateTime? endDate, bool ownRecords)
        {
            return PartialView("_Table", await _proposalRepository.GetFilteredProposals(searchString, startDate, endDate, ownRecords, User.Identity.Name));
        } // SearchAJAX
    }
}
