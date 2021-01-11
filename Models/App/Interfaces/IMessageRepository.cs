using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

using UkrStanko.Models.Security;

namespace UkrStanko.Models.App.Interfaces
{
    public interface IMessageRepository
    {
        /// <summary>
        /// Получение списка элементов
        /// </summary>
        Task<List<Message>> GetMessages(UserManager<AppUser> userManager);

        /// <summary>
        /// Получение списка элементов
        /// </summary>
        /// <param name="searchString">Поисковая строка</param>
        Task<List<Message>> GetFilteredMessages(UserManager<AppUser> userManager, string searchString);

        /// <summary>
        /// Добавление элемента
        /// </summary>
        /// <param name="element">Добавляемый элемент</param>
        /// <param name="responseId">Ид ответа сообщения</param>
        /// <param name="responseType">Тип ответа сообщения</param>
        Task AddMessage(Message element, int responseId, string responseType);

        /// <summary>
        /// Удаление элемента
        /// </summary>
        /// <param name="id">ИД удаляемого элемента</param>
        Task RemoveMessage(int id);
    }
}
