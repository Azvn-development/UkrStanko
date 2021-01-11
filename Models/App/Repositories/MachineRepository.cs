using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using UkrStanko.Models.App.Context;
using UkrStanko.Models.App.Interfaces;

namespace UkrStanko.Models.App.Repositories
{
    public class MachineRepository: IMachineRepository
    {
        private readonly AppDbContext _context;

        public MachineRepository(AppDbContext context)
        {
            _context = context;
        } // MachineRepository

        /// <summary>
        /// Получение списка элементов
        /// </summary>
        /// <param name="pageCount">Кол-во элементов для отображения</param>
        public async Task<List<Machine>> GetMachines(int itemsCount = 0) {
            var machines = await _context.Machines
                .Include(i => i.MachineType.ParentMachineType)
                .OrderBy(i => i.MachineType.ParentMachineType.Name)
                .ThenBy(i => i.MachineType.Name)
                .ThenBy(i => i.Name)
                .Skip(itemsCount)
                .Take(50)
                .ToListAsync();

            var analogues = await GetAnalogs(machines);
            foreach (var machine in machines)
            {
                machine.Analogue = analogues.FirstOrDefault(i => i.MachineTypeId == machine.MachineTypeId && i.Id != machine.Id)?.Name;
            } // foreach

            return machines;
        } // GetMachines

        /// <summary>
        /// Получение списка элементов
        /// </summary>
        /// <param name="machines">Элементы для которых требуются аналоги</param>
        public async Task<List<Machine>> GetAnalogs(IEnumerable<Machine> machines)
        {
            var machineTypesId = machines.GroupBy(i => i.MachineTypeId).Select(i => i.Key).ToList();

            return await _context.Machines
                .Where(i => machineTypesId.Contains(i.MachineTypeId))
                .AsNoTracking()
                .ToListAsync();
        } // GetAnalogs

        /// <summary>
        /// Получение кол-ва заявок по станку
        /// </summary>
        /// <param name="id">Ид станка</param>
        public async Task<int> GetRequisitionsCount(int id)
        {
            return await _context.Requisitions.Where(i => i.MachineId == id).CountAsync();
        } // GetRequisitionsCount

        /// <summary>
        /// Получение кол-ва предложений по станку
        /// </summary>
        /// <param name="id">Ид станка</param>
        public async Task<int> GetProposalsCount(int id)
        {
            return await _context.Proposals.Where(i => i.MachineId == id).CountAsync();
        } // GetProposalsCount

        /// <summary>
        /// Получение отфильтрованного списка элементов
        /// </summary>
        /// <param name="searchString">Поисковая строка</param>
        public async Task<List<Machine>> GetFilteredMachines(string searchString)
        {
            var machines = await _context.Machines
                .Include(i => i.MachineType.ParentMachineType)
                .Where(i => searchString == null || (i.Name + i.MachineType.ParentMachineType.Name).Contains(searchString))
                .ToListAsync();

            var analogues = await GetAnalogs(machines);
            foreach(var machine in machines)
            {
                machine.Analogue = analogues.FirstOrDefault(i => i.MachineTypeId == machine.MachineTypeId && i.Id != machine.Id)?.Name;
            } // foreach

            return machines;
        } // GetFilteredMachines

        /// <summary>
        /// Получение элемента
        /// </summary>
        /// <param name="id">ИД искомого элемента</param>
        public async Task<Machine> GetMachine(int id)
        {
            return await _context.Machines
                .Include(i => i.MachineType.ParentMachineType)
                .FirstOrDefaultAsync(i => i.Id == id);
        } // GetMachine

        /// <summary>
        /// Получение элемента
        /// </summary>
        /// <param name="name">Имя искомого элемента</param>
        public async Task<Machine> GetMachine(string name)
        {
            return await _context.Machines
                .Include(i => i.MachineType.ParentMachineType)
                .FirstOrDefaultAsync(i => i.Name == name);
        } // GetMachine

        /// <summary>
        /// Получение элемента
        /// </summary>
        /// <param name="id">ИД элемента, для которого требуется аналог</param>
        public async Task<string> GetAnalogueName(Machine machine)
        {
            return await _context.Machines
                .Where(i => i.MachineTypeId == machine.MachineTypeId && i.Id != machine.Id)
                .Select(i => i.Name)
                .FirstOrDefaultAsync();
        } // GetAnalogueName

        /// <summary>
        /// Добавление элемента
        /// </summary>
        /// <param name="element">Добавляемый элемент</param>
        public async Task AddMachine(Machine element)
        {
            element.Name = element.Name.ToUpper();

            _context.Machines.Add(element);
            await _context.SaveChangesAsync();
        } // AddMachine

        /// <summary>
        /// Добавление элемента
        /// </summary>
        /// <param name="name">Имя добавляемого элемента</param>
        public async Task<Machine> AddMachine(string name, int machineTypeId)
        {
            // Добавление элемента
            var element = new Machine { Name = name.ToUpper(),  MachineTypeId = machineTypeId};

            _context.Machines.Add(element);
            await _context.SaveChangesAsync();

            return element;
        } // AddMachine

        /// <summary>
        /// Редактирование элемента
        /// </summary>
        /// <param name="element">Редактируемый элемент</param>
        public async Task EditMachine(Machine element)
        {
            element.Name = element.Name.ToUpper();

            _context.Entry(element).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        } // EditMachine

        /// <summary>
        /// Удаление элемента
        /// </summary>
        /// <param name="id">ИД удаляемого элемента</param>
        public async Task RemoveMachine(int id)
        {
            // Удаляемый элемент
            var element = await GetMachine(id);

            // Удаление элемента из базы
            _context.Entry(element).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        } // RemoveMachine
    }
}
