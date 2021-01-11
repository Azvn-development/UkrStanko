using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using UkrStanko.Models.App;

namespace UkrStanko.Models.App.Interfaces
{
    public interface IMachineTypeRepository
    {
        /// <summary>
        /// Получение списка элементов
        /// </summary>
        Task<List<MachineType>> GetMachineTypes();

        /// <summary>
        /// Получение отфильтрованного списка элементов
        /// </summary>
        /// <param name="searchString">Поисковая строка</param>
        Task<List<MachineType>> GetFilteredMachineTypes(string searchString);

        /// <summary>
        /// Получение кол-ва заявок по станку
        /// </summary>
        /// <param name="id">Ид типа станка</param>
        Task<int> GetRequisitionsCount(int id);

        /// <summary>
        /// Получение кол-ва предложений по станку
        /// </summary>
        /// <param name="id">Ид типа станка</param>
        Task<int> GetProposalsCount(int id);

        /// <summary>
        /// Получение элемента
        /// </summary>
        /// <param name="id">ИД искомого элемента</param>
        Task<MachineType> GetMachineType(int id);

        /// <summary>
        /// Добавление элемента
        /// </summary>
        /// <param name="element">Добавляемый элемент</param>
        Task AddMachineType(MachineType element);

        /// <summary>
        /// Добавление элемента
        /// </summary>
        /// <param name="parentMachineTypeName">Группа, в которую добавляется элемент</param>
        Task<int> AddMachineType(string parentMachineTypeId);

        /// <summary>
        /// Редактирование элемента
        /// </summary>
        /// <param name="element">Редактируемый элемент</param>
        Task EditMachineType(MachineType element);

        /// <summary>
        /// Удаление элемента
        /// </summary>
        /// <param name="id">ИД удаляемого элемента</param>
        Task RemoveMachineType(int id);

        /// <summary>
        /// Проверка наличия пустых элементов
        /// </summary>
        Task CheckEmptyGroups();
    }
}
