using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using UkrStanko.Models.App.Context;
using UkrStanko.Models.App.Interfaces;
using UkrStanko.Models.Security;

namespace UkrStanko.Models.App.Repositories
{
    public class MessageRepository: IMessageRepository
    {
        private readonly AppDbContext _context;

        public MessageRepository(AppDbContext context)
        {
            _context = context;
        } // MessageRepository

        /// <summary>
        /// Получение списка элементов
        /// </summary>
        public async Task<List<Message>> GetMessages(UserManager<AppUser> userManager) {
            var messages = await _context.Messages.AsNoTracking().ToListAsync();

            // Коллекция фото пользователей
            var userImages = await _context.UserImages.ToListAsync();

            foreach (var message in messages)
            {
                var messageUser = await userManager.FindByIdAsync(message.UserId);
                message.UserName = messageUser.UserName;

                // Поиск ответов по сообщению
                if (message.Response)
                {
                    var requisitionResponse = await _context.RequisitionResponses.Include(i => i.Requisition.Machine).FirstOrDefaultAsync(i => i.MessageId == message.Id);
                    var proposalResponse = await _context.ProposalResponses.Include(i => i.Proposal.Machine).FirstOrDefaultAsync(i => i.MessageId == message.Id);
                    var messageResponse = await _context.MessageResponses.Include(i => i.ResponseMessage).FirstOrDefaultAsync(i => i.MessageId == message.Id);

                    if (requisitionResponse != null) message.ResponseRequisition = requisitionResponse.Requisition;
                    if (proposalResponse != null) message.ResponseProposal = proposalResponse.Proposal;
                    if (messageResponse != null)
                    {
                        message.ResponseMessage = messageResponse.ResponseMessage;

                        if (message.ResponseMessage != null)
                        {
                            var responseUser = await userManager.FindByIdAsync(message.ResponseMessage.UserId);

                            if (responseUser == null) message.ResponseMessage.UserName = "Пользователь удален";
                            else message.ResponseMessage.UserName = responseUser.UserName;
                        } // if
                    } // if
                } // if

                message.UserImagePath = (await _context.UserImages.FirstOrDefaultAsync(i => i.UserName == message.UserName))?.Path;
            } // foreach

            return messages;
        } // GetMessages

        /// <summary>
        /// Получение списка элементов
        /// </summary>
        /// <param name="searchString">Поисковая строка</param>
        public async Task<List<Message>> GetFilteredMessages(UserManager<AppUser> userManager, string searchString)
        {
            var messages = await _context.Messages
                .Where(i => i.Text.Contains(searchString))
                .AsNoTracking()
                .ToListAsync();

            // Коллекция фото пользователей
            var userImages = await _context.UserImages.ToListAsync();

            foreach (var message in messages)
            {
                var messageUser = await userManager.FindByIdAsync(message.UserId);
                message.UserName = messageUser.UserName;

                // Поиск ответов по сообщению
                if (message.Response)
                {
                    var requisitionResponse = await _context.RequisitionResponses.Include(i => i.Requisition.Machine).FirstOrDefaultAsync(i => i.MessageId == message.Id);
                    var proposalResponse = await _context.ProposalResponses.Include(i => i.Proposal.Machine).FirstOrDefaultAsync(i => i.MessageId == message.Id);
                    var messageResponse = await _context.MessageResponses.Include(i => i.ResponseMessage).FirstOrDefaultAsync(i => i.MessageId == message.Id);

                    if (requisitionResponse != null) message.ResponseRequisition = requisitionResponse.Requisition;
                    if (proposalResponse != null) message.ResponseProposal = proposalResponse.Proposal;
                    if (messageResponse != null)
                    {
                        message.ResponseMessage = messageResponse.ResponseMessage;

                        if (message.ResponseMessage != null)
                        {
                            var responseUser = await userManager.FindByIdAsync(message.ResponseMessage.UserId);

                            if (responseUser == null) message.ResponseMessage.UserName = "Пользователь удален";
                            else message.ResponseMessage.UserName = responseUser.UserName;
                        } // if
                    } // if
                } // if

                message.UserImagePath = (await _context.UserImages.FirstOrDefaultAsync(i => i.UserName == message.UserName))?.Path;
            } // foreach

            return messages;
        } // GetFilteredMessages

        /// <summary>
        /// Добавление элемента
        /// </summary>
        /// <param name="element">Добавляемый элемент</param>
        /// <param name="responseId">Ид ответа сообщения</param>
        /// <param name="responseType">Тип ответа сообщения</param>
        public async Task AddMessage(Message element, int responseId, string responseType)
        {
            // Добавление сообщения
            _context.Messages.Add(element);
            await _context.SaveChangesAsync();

            // Добавление информации об ответе
            switch (responseType)
            {
                case "Requisition":
                    {
                        _context.RequisitionResponses.Add(new RequisitionResponse
                        {
                            MessageId = element.Id,
                            RequisitionId = responseId
                        });
                        break;
                    }

                case "Proposal":
                    {
                        _context.ProposalResponses.Add(new ProposalResponse
                        {
                            MessageId = element.Id,
                            ProposalId = responseId
                        });
                        break;
                    }

                case "Message":
                    {
                        _context.MessageResponses.Add(new MessageResponse
                        {
                            MessageId = element.Id,
                            ResponseMessageId = responseId
                        });
                        break;
                    }
            } // switch
            
            await _context.SaveChangesAsync();
        } // AddMessage

        /// <summary>
        /// Удаление элемента
        /// </summary>
        /// <param name="id">ИД удаляемого элемента</param>
        public async Task RemoveMessage(int id)
        {
            // Удаляемый элемент
            var element = await _context.Messages.FirstOrDefaultAsync(i => i.Id == id);

            if (element != null)
            {
                // Удаление элемента из базы
                _context.Entry(element).State = EntityState.Deleted;

                //Удаление записи в ответах
                var response = await _context.MessageResponses.FirstOrDefaultAsync(i => i.MessageId == id);
                if (response != null) _context.Entry(response).State = EntityState.Deleted;

                await _context.SaveChangesAsync();
            } // if
        } // RemoveMessage
    }
}
