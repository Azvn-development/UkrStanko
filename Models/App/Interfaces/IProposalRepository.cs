using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using UkrStanko.Models.App;

namespace UkrStanko.Models.App.Interfaces
{
    public interface IProposalRepository
    {
        /// <summary>
        /// Получение списка элементов
        /// </summary>
        Task<List<Proposal>> GetProposals();

        /// <summary>
        /// Получение списка элементов
        /// </summary>
        /// <param name="machine">Элемент, по которому необходимо отфильтровать</param>
        Task<List<Proposal>> GetProposals(Machine machine);

        /// <summary>
        /// Получение списка элементов
        /// </summary>
        /// <param name="machine">Элемент, по которому необходимо отфильтровать</param>
        Task<List<string>> GetRequisitionPaths(Machine machine);

        /// <summary>
        /// Получение отфильтрованного списка элементов
        /// </summary>
        /// <param name="searchString">Поисковая строка</param>
        /// <param name="startDate">Начальная дата</param>
        /// <param name="endDate">Конечная дата</param>
        /// <param name="ownRecords">Только собственные записи</param>
        /// <param name="userName">Текущий пользователь</param>
        Task<List<Proposal>> GetFilteredProposals(string searchString, DateTime? startDate, DateTime? endDate, bool ownRecords, string userName);

        /// <summary>
        /// Получение элемента
        /// </summary>
        /// <param name="id">ИД искомого элемента</param>
        Task<Proposal> GetProposal(int id);

        /// <summary>
        /// Добавление элемента
        /// </summary>
        /// <param name="element">Добавляемый элемент</param>
        Task AddProposal(Proposal element);

        /// <summary>
        /// Редактирование элемента
        /// </summary>
        /// <param name="element">Редактируемый элемент</param>
        Task EditProposal(Proposal element);

        /// <summary>
        /// Редактирование элементов
        /// </summary>
        /// <param name="oldUserName">Старое имя пользователя</param>
        /// <param name="newUserName">Новое имя пользователя</param>
        Task EditProposal(string oldUserName, string newUserName);

        /// <summary>
        /// Удаление элемента
        /// </summary>
        /// <param name="id">ИД удаляемого элемента</param>
        Task RemoveProposal(int id);

        /// <summary>
        /// Удаление элементов по пользователю
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        Task RemoveProposals(string userName);
    }
}
