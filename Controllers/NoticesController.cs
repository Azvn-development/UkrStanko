using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using System.IO;

using UkrStanko.Models;
using UkrStanko.Models.App;
using UkrStanko.Models.App.Interfaces;
using UkrStanko.Models.Security;
using UkrStanko.Models.Hubs;
using UkrStanko.ViewModels.Notices;

namespace UkrStanko.Controllers
{
    [Authorize]
    public class NoticesController : Controller
    {
        // Репозитории
        private readonly IProposalRepository _proposalRepository;
        private readonly IRequisitionRepository _requisitionRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IReadedNoticeRepository _readedNoticeRepository;
        private readonly IHubContext<NotificationHub> _hub;

        // Управление пользователями
        private readonly UserManager<AppUser> _userManager;

        // Конструктор
        public NoticesController(IProposalRepository proposalRepository, 
            IRequisitionRepository requisitionRepository,
            IMessageRepository messageRepository,
            IReadedNoticeRepository readedNoticeRepository,
            UserManager<AppUser> userManager,
            IHubContext<NotificationHub> hub)
        {
            _proposalRepository = proposalRepository;
            _requisitionRepository = requisitionRepository;
            _messageRepository = messageRepository;
            _readedNoticeRepository = readedNoticeRepository;
            _hub = hub;

            _userManager = userManager;
        } // NoticesController

        // Получение списка элементов
        public async Task<IActionResult> Index(bool ajaxLoad)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var viewModel = new IndexViewModel(
                await _proposalRepository.GetProposals(),
                await _requisitionRepository.GetRequisitions(),
                await _messageRepository.GetMessages(_userManager),
                await _readedNoticeRepository.GetUserReadedNoticesCounter(user.Id));

            if (!ajaxLoad) return View(viewModel);
            return PartialView(viewModel);
        } // Index

        // Чтение элемента AJAX
        public async Task<int> ReadNoticesAJAX(int readedNoticeCouter)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            await _readedNoticeRepository.EditUserReadedNoticesCounter(user.Id, readedNoticeCouter);

            return readedNoticeCouter;
        } // ReadNoticesAJAX

        // Отправка сообщения AJAX
        public async Task<ActionResult> SendAJAX(string text, int responseId, string responseType, string connectionId)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            try
            {
                // Добавление сообщения
                var message = new Message { CreateDate = DateTime.Now, Text = text, UserId = user.Id, Response = responseId != 0 };
                await _messageRepository.AddMessage(message, responseId, responseType);

                await _hub.Clients.All.SendAsync("Success", Messages.successCreateMessage);
            } catch { await _hub.Clients.Client(connectionId).SendAsync("Error", Messages.createErrorMessage); }

            var viewModel = new IndexViewModel(
                await _proposalRepository.GetProposals(),
                await _requisitionRepository.GetRequisitions(),
                await _messageRepository.GetMessages(_userManager),
                await _readedNoticeRepository.GetUserReadedNoticesCounter(user.Id));

            return PartialView("_Table", viewModel);
        } // SearchAJAX

        // Удаление сообщения AJAX
        public async Task<bool> DeleteAJAX(int id, string connectionId)
        {
            try
            {
                // Удаление сообщения
                await _messageRepository.RemoveMessage(id);
                await _hub.Clients.All.SendAsync("Success", Messages.successDeleteMessage);

                // Декрементирование счетчика новостей для всех пользователей
                await _readedNoticeRepository.DecrementUsersReadedNoticesCounter();

                return true;
            } catch { 
                await _hub.Clients.Client(connectionId).SendAsync("Error", Messages.deleteErrorMessage);
                return false;
            } // try-catch
        } // DeleteAJAX

        // Поиск элемента AJAX
        public async Task<ActionResult> SearchAJAX(string searchString)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var viewModel = new IndexViewModel(
                    string.IsNullOrEmpty(searchString) ? await _proposalRepository.GetProposals() : await _proposalRepository.GetFilteredProposals(searchString, null, null, false, null),
                    string.IsNullOrEmpty(searchString) ? await _requisitionRepository.GetRequisitions() : await _requisitionRepository.GetFilteredRequisitions(searchString, null, null, false, null),
                    string.IsNullOrEmpty(searchString) ? await _messageRepository.GetMessages(_userManager) : await _messageRepository.GetFilteredMessages(_userManager, searchString),
                    await _readedNoticeRepository.GetUserReadedNoticesCounter(user.Id),
                    string.IsNullOrEmpty(searchString) ? 0 : -1) ;
            ViewBag.Download = string.IsNullOrEmpty(searchString);

            return PartialView("_Table", viewModel);
        } // SearchAJAX

        // Подгрузка данных AJAX
        public async Task<ActionResult> DownloadAJAX(int downloadedCount)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var viewModel = new IndexViewModel(
                await _proposalRepository.GetProposals(),
                await _requisitionRepository.GetRequisitions(),
                await _messageRepository.GetMessages(_userManager),
                await _readedNoticeRepository.GetUserReadedNoticesCounter(user.Id),
                downloadedCount);

            return PartialView("_TBody", viewModel);
        } // DownloadAJAX
    }
}
