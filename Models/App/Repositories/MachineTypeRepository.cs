using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using UkrStanko.Models.App.Context;
using UkrStanko.Models.App.Interfaces;

namespace UkrStanko.Models.App.Repositories
{
    public class MachineTypeRepository: IMachineTypeRepository
    {
        private readonly AppDbContext _context;

        public MachineTypeRepository(AppDbContext context)
        {
            _context = context;
        } // MachineTypeRepository

        /// <summary>
        /// Получение списка элементов
        /// </summary>
        public async Task<List<MachineType>> GetMachineTypes() {
            return await _context.MachineTypes
                .Where(i => i.ParentMachineTypeId == null && i.Id != 1)
                .AsNoTracking()
                .ToListAsync();
        } // GetMachineTypes

        /// <summary>
        /// Получение кол-ва заявок по станку
        /// </summary>
        /// <param name="id">Ид типа станка</param>
        public async Task<int> GetRequisitionsCount(int id)
        {
            return await _context.Requisitions
                .Include(i => i.Machine.MachineType)
                .Where(i => i.Machine.MachineType.ParentMachineTypeId == id)
                .CountAsync();
        } // GetRequisitionsCount

        /// <summary>
        /// Получение кол-ва предложений по станку
        /// </summary>
        /// <param name="id">Ид типа станка</param>
        public async Task<int> GetProposalsCount(int id)
        {
            return await _context.Proposals
                .Include(i => i.Machine.MachineType)
                .Where(i => i.Machine.MachineType.ParentMachineTypeId == id)
                .CountAsync();
        } // GetProposalsCount

        /// <summary>
        /// Получение отфильтрованного списка элементов
        /// </summary>
        /// <param name="searchString">Поисковая строка</param>
        public async Task<List<MachineType>> GetFilteredMachineTypes(string searchString)
        {
            return await _context.MachineTypes
                .Where(i => (searchString == null || i.Name.Contains(searchString)) && i.ParentMachineTypeId == null && i.Id != 1)
                .AsNoTracking()
                .ToListAsync();
        } // GetFilteredMachineTypes

        /// <summary>
        /// Получение элемента
        /// </summary>
        /// <param name="id">ИД искомого элемента</param>
        public async Task<MachineType> GetMachineType(int id)
        {
            return await _context.MachineTypes.FirstOrDefaultAsync(i => i.Id == id);
        } // GetMachineType

        /// <summary>
        /// Добавление элемента
        /// </summary>
        /// <param name="element">Добавляемый элемент</param>
        public async Task AddMachineType(MachineType element)
        {
            _context.MachineTypes.Add(element);
            await _context.SaveChangesAsync();
        } // AddMachineType

        /// <summary>
        /// Добавление элемента
        /// </summary>
        /// <param name="parentMachineTypeName">Группа, в которую добавляется элемент</param>
        public async Task<int> AddMachineType(string parentMachineTypeName)
        {
            var parentMachineTypeId = (await _context.MachineTypes
                .FirstOrDefaultAsync(i => i.Name == parentMachineTypeName)).Id;

            int maxId = 0;
            try
            {
                maxId = await _context.MachineTypes
                    .Where(i => i.ParentMachineTypeId == parentMachineTypeId)
                    .MaxAsync(i => i.Id);
            } catch { }

            var element = new MachineType { Name = $"{parentMachineTypeName}_{maxId + 1}", ParentMachineTypeId = parentMachineTypeId };

            _context.MachineTypes.Add(element);
            await _context.SaveChangesAsync();

            return element.Id;
        } // AddMachineType

        /// <summary>
        /// Редактирование элемента
        /// </summary>
        /// <param name="element">Редактируемый элемент</param>
        public async Task EditMachineType(MachineType element)
        {
            _context.Entry(element).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        } // EditMachineType

        /// <summary>
        /// Удаление элемента
        /// </summary>
        /// <param name="id">ИД удаляемого элемента</param>
        public async Task RemoveMachineType(int id)
        {
            // Удаляемый элемент
            var element = await GetMachineType(id);

            // Удаление зависимых типов оборудования
            var childMachineTypes = await _context.MachineTypes.Where(i => i.ParentMachineTypeId == element.Id).ToListAsync();
            if(childMachineTypes.Count > 0)
            {
                _context.MachineTypes.RemoveRange(childMachineTypes);
                await _context.SaveChangesAsync();
            } // if

            // Удаление элемента из базы
            _context.Entry(element).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        } // RemoveMachineType

        /// <summary>
        /// Проверка наличия пустых элементов
        /// </summary>
        public async Task CheckEmptyGroups()
        {
            _context.RemoveRange(_context.MachineTypes
                .Where(i => 
                    !_context.Machines.GroupBy(i => i.MachineTypeId).Select(i => i.Key).Contains(i.Id) &&
                    i.ParentMachineTypeId != null));
            await _context.SaveChangesAsync();
        } // CheckEmptyGroups
    }
}
