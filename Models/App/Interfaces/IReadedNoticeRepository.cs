using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UkrStanko.Models.App.Interfaces
{
    public interface IReadedNoticeRepository
    {
        /// <summary>
        /// Получение кол-ва прочитанных пользователем новостей
        /// </summary>
        /// <param name="userId">Имя пользователя</param>
        Task<int> GetUserReadedNoticesCounter(string userId);

        /// <summary>
        /// Добавление счетчика прочитанных новостей пользователем
        /// </summary>
        /// <param name="userId">Имя пользователя</param>
        Task AddUserReadedNoticesCounter(string userId);

        /// <summary>
        /// Изменение кол-ва прочитанных пользователями новостей
        /// </summary>
        /// <param name="userId">Имя пользователя</param>
        /// <param name="readedNoticesCount">Кол-во прочитанных пользователем новостей</param>
        Task EditUserReadedNoticesCounter(string userId, int readedNoticesCount);

        /// <summary>
        /// Изменение кол-ва прочитанных пользователями новостей на 1 пункт
        /// </summary>
        Task DecrementUsersReadedNoticesCounter();

        /// <summary>
        /// Изменение кол-ва прочитанных пользователями новостей на N пунктов
        /// </summary>
        /// <param name="count">Кол-во пунктов</param>
        Task DecrementUsersReadedNoticesCounter(int count);

        /// <summary>
        /// Удаление счетчика прочитанных новостей пользователем
        /// </summary>
        /// <param name="userId">Имя пользователя</param>
        Task RemoveUserReadedNoticesCounter(string userId);
    }
}
