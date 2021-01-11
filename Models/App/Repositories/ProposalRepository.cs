using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using UkrStanko.Models.App.Context;
using UkrStanko.Models.App.Interfaces;

namespace UkrStanko.Models.App.Repositories
{
    public class ProposalRepository: IProposalRepository
    {
        private readonly AppDbContext _context;

        // Кол-во символов для поиска в имени машины
        private const int machineNameLength = 4;

        public ProposalRepository(AppDbContext context)
        {
            _context = context;
        } // ProposalRepository

        /// <summary>
        /// Получение списка элементов
        /// </summary>
        public async Task<List<Proposal>> GetProposals() {
            var proposals = await _context.Proposals
                .Include(i => i.Machine.MachineType.ParentMachineType)
                .ToListAsync();

            // Коллекция фото пользователей
            var userImages = await _context.UserImages.ToListAsync();
            foreach (var proposal in proposals)
            {
                // Путь к изображению пользователя
                proposal.UserImagePath = (await _context.UserImages.FirstOrDefaultAsync(i => i.UserName == proposal.UserName))?.Path;

                // Пользователи, имеющие доступные предложения по заявке
                var requisitionUsers = await GetRequisitionPaths(proposal.Machine);
                if (requisitionUsers.Count > 0)
                {
                    proposal.RequisitionUserPaths = new Dictionary<string, string>();
                    foreach (var requisitionUser in requisitionUsers)
                    {
                        proposal.RequisitionUserPaths.Add(requisitionUser, userImages.FirstOrDefault(i => i.UserName == requisitionUser)?.Path);
                    } // foreach
                }
            } // foreach

            return proposals;
        } // GetProposals

        /// <summary>
        /// Получение списка элементов
        /// </summary>
        /// <param name="machine">Элемент, по которому необходимо отфильтровать</param>
        public async Task<List<Proposal>> GetProposals(Machine machine)
        {
            List<Machine> analogs = new List<Machine>(); // Список аналогов
            List<int> analogueGroups; // Список уникальных групп аналогов
            List<string> analogueCutedNames; // Список обрезанных имен аналогов (4 символа)

            if (machine.MachineType.ParentMachineType.Name == "Без группы" && machine.Name.Length >= machineNameLength)
            {
                // Поиск аналогов по имени
                analogs = await _context.Machines
                    .Include(i => i.MachineType.ParentMachineType)
                    .Where(i => i.Name.Length >= 4 &&
                        i.Name.Substring(0, machineNameLength) == machine.Name.Substring(0, machineNameLength))
                    .ToListAsync();

                // Для каждого аналога с группой ищем еще аналоги
                analogueGroups = analogs.Where(i => i.MachineType.ParentMachineType.Name != "Без группы")
                    .GroupBy(i => i.MachineTypeId)
                    .Select(i => i.Key)
                    .ToList();

                analogs.AddRange(await _context.Machines.Where(i => analogueGroups.Contains(i.MachineTypeId)).ToListAsync());
                analogs = analogs.Distinct().ToList();
            }
            else
            {
                // Поиск аналогов по группе
                analogs = await _context.Machines
                    .Where(i => i.MachineTypeId == machine.MachineTypeId)
                    .ToListAsync();
            } // if

            // Поиск предложений по аналогам
            analogueGroups = analogs
                .GroupBy(i => i.MachineTypeId)
                .Select(i => i.Key)
                .ToList();
            analogueCutedNames = analogs
                .Where(i => i.Name.Length >= machineNameLength)
                .GroupBy(i => i.Name.Substring(0, machineNameLength))
                .Select(i => i.Key)
                .ToList();

            var proposals = await _context.Proposals
                .Include(i => i.Machine.MachineType.ParentMachineType)
                .Where(i => analogueGroups.Contains(i.Machine.MachineTypeId) ||
                    (i.Machine.Name.Length >= machineNameLength && analogueCutedNames.Contains(i.Machine.Name.Substring(0, machineNameLength))))
                .ToListAsync();

            // Коллекция фото пользователей
            var proposalUsers = proposals.GroupBy(i => i.UserName).Select(i => i.Key).ToList();

            var userImages = await _context.UserImages.ToListAsync();
            foreach (var proposal in proposals)
            {
                proposal.UserImagePath = (await _context.UserImages.FirstOrDefaultAsync(i => i.UserName == proposal.UserName))?.Path;
            } // foreach

            return proposals.Distinct().ToList();
        } // GetProposals

        /// <summary>
        /// Получение списка элементов
        /// </summary>
        /// <param name="machine">Элемент, по которому необходимо отфильтровать</param>
        public async Task<List<string>> GetRequisitionPaths(Machine machine)
        {
            List<Machine> analogs = new List<Machine>(); // Список аналогов
            List<int> analogueGroups; // Список уникальных групп аналогов
            List<string> analogueCutedNames; // Список обрезанных имен аналогов (4 символа)

            if (machine.MachineType.ParentMachineType.Name == "Без группы" && machine.Name.Length >= machineNameLength)
            {
                // Поиск аналогов по имени
                analogs = await _context.Machines
                    .Include(i => i.MachineType.ParentMachineType)
                    .Where(i => i.Name.Length >= 4 &&
                        i.Name.Substring(0, machineNameLength) == machine.Name.Substring(0, machineNameLength))
                    .ToListAsync();

                // Для каждого аналога с группой ищем еще аналоги
                analogueGroups = analogs.Where(i => i.MachineType.ParentMachineType.Name != "Без группы")
                    .GroupBy(i => i.MachineTypeId)
                    .Select(i => i.Key)
                    .ToList();

                analogs.AddRange(await _context.Machines.Where(i => analogueGroups.Contains(i.MachineTypeId)).ToListAsync());
                analogs = analogs.Distinct().ToList();
            }
            else
            {
                // Поиск аналогов по группе
                analogs = await _context.Machines
                    .Where(i => i.MachineTypeId == machine.MachineTypeId)
                    .ToListAsync();
            } // if

            // Поиск предложений по аналогам
            analogueGroups = analogs
                .GroupBy(i => i.MachineTypeId)
                .Select(i => i.Key)
                .ToList();
            analogueCutedNames = analogs
                .Where(i => i.Name.Length >= machineNameLength)
                .GroupBy(i => i.Name.Substring(0, machineNameLength))
                .Select(i => i.Key)
                .ToList();

            var requisitions = await _context.Requisitions
                .Include(i => i.Machine.MachineType.ParentMachineType)
                .Where(i => analogueGroups.Contains(i.Machine.MachineTypeId) ||
                    (i.Machine.Name.Length >= machineNameLength && analogueCutedNames.Contains(i.Machine.Name.Substring(0, machineNameLength))))
                .ToListAsync();

            return requisitions.GroupBy(i => i.UserName).Select(i => i.Key).ToList();
        } // GetRequisitionPaths

        /// <summary>
        /// Получение отфильтрованного списка элементов
        /// </summary>
        /// <param name="searchString">Поисковая строка</param>
        public async Task<List<Proposal>> GetFilteredProposals(string searchString, DateTime? startDate, DateTime? endDate, bool ownRecords, string userName)
        {
            var proposals = await _context.Proposals
                .Include(i => i.Machine.MachineType.ParentMachineType)
                .Where(i => (string.IsNullOrEmpty(searchString) ||
                    (i.Id + 
                    i.UserName + 
                    i.Location +
                    i.PurshasePrice + 
                    i.SellingPrice +
                    i.Comment +
                    i.Machine.Name).Contains(searchString)) &&
                    (startDate == null || i.CreateDate.Date >= startDate.Value.Date) &&
                    (endDate == null || i.CreateDate.Date <= endDate.Value.Date) &&
                    (!ownRecords || i.UserName == userName))
                .ToListAsync();

            // Коллекция фото пользователей
            var userImages = await _context.UserImages.ToListAsync();
            foreach (var proposal in proposals)
            {
                // Путь к изображению пользователя
                proposal.UserImagePath = (await _context.UserImages.FirstOrDefaultAsync(i => i.UserName == proposal.UserName))?.Path;

                // Пользователи, имеющие доступные предложения по заявке
                var requisitionUsers = await GetRequisitionPaths(proposal.Machine);
                if (requisitionUsers.Count > 0)
                {
                    proposal.RequisitionUserPaths = new Dictionary<string, string>();
                    foreach (var requisitionUser in requisitionUsers)
                    {
                        proposal.RequisitionUserPaths.Add(requisitionUser, userImages.FirstOrDefault(i => i.UserName == requisitionUser)?.Path);
                    } // foreach
                }
            } // foreach

            return proposals;
        } // GetFilteredProposals

        /// <summary>
        /// Получение элемента
        /// </summary>
        /// <param name="id">ИД искомого элемента</param>
        public async Task<Proposal> GetProposal(int id)
        {
            return await _context.Proposals
                .Include(i => i.Machine.MachineType.ParentMachineType)
                .FirstOrDefaultAsync(i => i.Id == id);
        } // GetProposal

        /// <summary>
        /// Добавление элемента
        /// </summary>
        /// <param name="element">Добавляемый элемент</param>
        public async Task AddProposal(Proposal element)
        {
            _context.Proposals.Add(element);
            await _context.SaveChangesAsync();
        } // AddProposal

        /// <summary>
        /// Редактирование элемента
        /// </summary>
        /// <param name="element">Редактируемый элемент</param>
        public async Task EditProposal(Proposal element)
        {
            _context.Entry(element).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        } // EditProposal

        /// <summary>
        /// Редактирование элементов
        /// </summary>
        /// <param name="oldUserName">Старое имя пользователя</param>
        /// <param name="newUserName">Новое имя пользователя</param>
        public async Task EditProposal(string oldUserName, string newUserName)
        {
            await _context.Proposals.Where(i => i.UserName == oldUserName).ForEachAsync(i =>
            {
                i.UserName = newUserName;
            });

            await _context.SaveChangesAsync();
        } // EditProposal

        /// <summary>
        /// Удаление элемента
        /// </summary>
        /// <param name="id">ИД удаляемого элемента</param>
        public async Task RemoveProposal(int id)
        {
            // Удаляемый элемент
            var element = await GetProposal(id);

            // Удаление элемента из базы
            _context.Entry(element).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        } // RemoveProposal

        /// <summary>
        /// Удаление элементов по пользователю
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        public async Task RemoveProposals(string userName)
        {
            var elements = await _context.Proposals.Where(i => i.UserName == userName).ToListAsync();

            _context.Proposals.RemoveRange(elements);
            await _context.SaveChangesAsync();
        } // RemoveProposals
    }
}
