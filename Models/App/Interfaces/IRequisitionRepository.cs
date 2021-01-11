using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using UkrStanko.Models.App;

namespace UkrStanko.Models.App.Interfaces
{
    public interface IRequisitionRepository
    {
        /// <summary>
        /// Получение списка элементов
        /// </summary>
        Task<List<Requisition>> GetRequisitions();

        /// <summary>
        /// Получение списка элементов
        /// </summary>
        /// <param name="machine">Элемент, по которому необходимо отфильтровать</param>
        Task<List<Requisition>> GetRequisitions(Machine machine);

        /// <summary>
        /// Получение списка элементов
        /// </summary>
        /// <param name="machine">Элемент, по которому необходимо отфильтровать</param>
        Task<List<string>> GetProposalPaths(Machine machine);

        /// <summary>
        /// Получение отфильтрованного списка элементов
        /// </summary>
        /// <param name="searchString">Поисковая строка</param>
        /// <param name="startDate">Начальная дата</param>
        /// <param name="endDate">Конечная дата</param>
        /// <param name="ownRecords">Только собственные записи</param>
        /// <param name="userName">Текущий пользователь</param>
        Task<List<Requisition>> GetFilteredRequisitions(string searchString, DateTime? startDate, DateTime? endDate, bool ownRecords, string userName);

        /// <summary>
        /// Получение элемента
        /// </summary>
        /// <param name="id">ИД искомого элемента</param>
        Task<Requisition> GetRequisition(int id);

        /// <summary>
        /// Добавление элемента
        /// </summary>
        /// <param name="element">Добавляемый элемент</param>
        Task AddRequisition(Requisition element);

        /// <summary>
        /// Редактирование элемента
        /// </summary>
        /// <param name="element">Редактируемый элемент</param>
        Task EditRequisition(Requisition element);

        /// <summary>
        /// Редактирование элементов
        /// </summary>
        /// <param name="oldUserName">Старое имя пользователя</param>
        /// <param name="newUserName">Новое имя пользователя</param>
        Task EditRequisitions(string oldUserName, string newUserName);

        /// <summary>
        /// Удаление элемента
        /// </summary>
        /// <param name="id">ИД удаляемого элемента</param>
        Task RemoveRequisition(int id);

        /// <summary>
        /// Удаление элементов по пользователю
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        Task RemoveRequisitions(string userName);
    }
}
