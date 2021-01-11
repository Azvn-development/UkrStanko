using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using UkrStanko.Models.App;

namespace UkrStanko.Models.App.Interfaces
{
    public interface IMachineRepository
    {
        /// <summary>
        /// Получение списка элементов
        /// </summary>
        /// <param name="pageCount">Кол-во элементов для отображения</param>
        Task<List<Machine>> GetMachines(int itemsCount = 0);

        /// <summary>
        /// Получение списка элементов
        /// </summary>
        /// <param name="machines">Элементы для которых требуются аналоги</param>
        Task<List<Machine>> GetAnalogs(IEnumerable<Machine> machines);

        /// <summary>
        /// Получение кол-ва заявок по станку
        /// </summary>
        /// <param name="id">Ид станка</param>
        Task<int> GetRequisitionsCount(int id);

        /// <summary>
        /// Получение кол-ва предложений по станку
        /// </summary>
        /// <param name="id">Ид станка</param>
        Task<int> GetProposalsCount(int id);

        /// <summary>
        /// Получение отфильтрованного списка элементов
        /// </summary>
        /// <param name="searchString">Поисковая строка</param>
        Task<List<Machine>> GetFilteredMachines(string searchString);

        /// <summary>
        /// Получение элемента
        /// </summary>
        /// <param name="id">ИД искомого элемента</param>
        Task<Machine> GetMachine(int id);

        /// <summary>
        /// Получение элемента
        /// </summary>
        /// <param name="name">Имя искомого элемента</param>
        Task<Machine> GetMachine(string name);

        /// <summary>
        /// Получение элемента
        /// </summary>
        /// <param name="machine">Элемент, для которого требуется аналог</param>
        Task<string> GetAnalogueName(Machine machine);

        /// <summary>
        /// Добавление элемента
        /// </summary>
        /// <param name="element">Добавляемый элемент</param>
        Task AddMachine(Machine element);

        /// <summary>
        /// Добавление элемента
        /// </summary>
        /// <param name="name">Имя добавляемого элемента</param>
        /// <param name="machineTypeId">Группа, в которую добавляется элемент</param>
        Task<Machine> AddMachine(string name, int machineTypeId);

        /// <summary>
        /// Редактирование элемента
        /// </summary>
        /// <param name="element">Редактируемый элемент</param>
        Task EditMachine(Machine element);

        /// <summary>
        /// Удаление элемента
        /// </summary>
        /// <param name="id">ИД удаляемого элемента</param>
        Task RemoveMachine(int id);
    }
}
