using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using Microsoft.Extensions.FileProviders;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;

using UkrStanko.Models.App.Context;
using UkrStanko.Models.App.Interfaces;

namespace UkrStanko.Models.App.Services
{
    public class DataBaseService: IDataBaseService
    {
        private readonly AppDbContext _context;

        // Веб среда
        private readonly IWebHostEnvironment _environment;

        // Конструктор
        public DataBaseService(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        } // DataBaseService

        /// <summary>
        /// Проверка изображений на сервере без записи в БД
        /// </summary>
        public async Task CheckImages()
        {
            var proposalImagesOnServer = _environment.ContentRootFileProvider
                .GetDirectoryContents("/wwwroot/images/proposals/")
                .ToList();
            var userImagesOnServer = _environment.ContentRootFileProvider
                .GetDirectoryContents("/wwwroot/images/users/")
                .ToList();

            // Сопоставление изображений предложений на сервере и записей в базе
            await _context.ProposalImages.ForEachAsync(item =>
            {
                if (proposalImagesOnServer.FirstOrDefault(i => i.Name == item.Name) == null) _context.ProposalImages.Remove(item);
                else proposalImagesOnServer.RemoveAll(i => i.Name == item.Name);
            });

            // Удаление изображений предложений без записи в БД из базы
            proposalImagesOnServer.ForEach(i =>
            {
                File.Delete(i.PhysicalPath);
            });

            // Сопоставление изображений пользователей на сервере и записей в базе
            await _context.UserImages.ForEachAsync(item =>
            {
                if (userImagesOnServer.FirstOrDefault(i => i.Name == item.Name) == null) _context.UserImages.Remove(item);
                else userImagesOnServer.RemoveAll(i => i.Name == item.Name);
            });

            // Удаление изображений предложений без записи в БД из базы
            userImagesOnServer.ForEach(i =>
            {
                File.Delete(i.PhysicalPath);
            });

            await _context.SaveChangesAsync();
        } // CheckImages

        /// <summary>
        /// Проверка заявок с датой создания более 3-х месяцев
        /// </summary>
        public async Task CheckRequisitions()
        {
            var requisitions = await _context.Requisitions.Where(i => i.CreateDate <= DateTime.Now.AddDays(-90)).ToListAsync();

            _context.Requisitions.RemoveRange(requisitions);
            await _context.SaveChangesAsync();
        } // CheckRequisitions

        /// <summary>
        /// Проверка предложений с датой создания более года
        /// </summary>
        public async Task CheckProposals()
        {
            var proposals = await _context.Proposals.Where(i => i.CreateDate <= DateTime.Now.AddDays(-365)).ToListAsync();

            _context.Proposals.RemoveRange(proposals);
            await _context.SaveChangesAsync();
        } // CheckProposals

        /// <summary>
        /// Проверка сообщений с датой создания более года
        /// </summary>
        public async Task CheckMessages()
        {
            var messages = await _context.Messages.Where(i => i.CreateDate <= DateTime.Now.AddDays(-365)).ToListAsync();
            _context.Messages.RemoveRange(messages);

            foreach(var message in messages)
            {
                var messageResponses = _context.MessageResponses.Where(i => i.MessageId == message.Id);
                _context.MessageResponses.RemoveRange(messageResponses);
            } // foreach

            await _context.SaveChangesAsync();
        } // CheckMessages
    }
}
