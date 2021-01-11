using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

using UkrStanko.Models.App.Context;
using UkrStanko.Models.App.Interfaces;

namespace UkrStanko.Models.App.Repositories
{
    public class RequisitionRepository: IRequisitionRepository
    {
        private readonly AppDbContext _context;

        // Кол-во символов для поиска в имени машины
        private const int machineNameLength = 4; 

        public RequisitionRepository(AppDbContext context)
        {
            _context = context;
        } // RequisitionRepository

        /// <summary>
        /// Получение списка элементов
        /// </summary>
        public async Task<List<Requisition>> GetRequisitions() {
            var requisitions = await _context.Requisitions
                .Include(i => i.Machine.MachineType.ParentMachineType)
                .ToListAsync();

            // Коллекция фото пользователей
            var userImages = await _context.UserImages.ToListAsync();

            // Расчет приоритета для заявок
            var priorities = requisitions
                    .OrderBy(i => i.CreateDate)
                    .GroupBy(i => new { i.Phone, i.UserName })
                    .Select(i => new { Phone = i.Key.Phone.Substring(i.Key.Phone.Length - 7, 7), i.Key.UserName, CreateDate = i.Min(item => item.CreateDate) })
                    .ToList();
            foreach (var requisition in requisitions)
            {
                // Приоритет заявки
                var priority = priorities.FirstOrDefault(i => i.Phone == requisition.Phone.Substring(requisition.Phone.Length - 7, 7));
                requisition.UserPriority = priority.UserName != requisition.UserName ? new KeyValuePair<string, DateTime>(priority.UserName, priority.CreateDate) : null;

                // Путь к изображению пользователя
                requisition.UserImagePath = userImages.FirstOrDefault(i => i.UserName == requisition.UserName)?.Path;

                // Пользователи, имеющие доступные предложения по заявке
                var proposalUsers = await GetProposalPaths(requisition.Machine);
                if (proposalUsers.Count > 0)
                {
                    requisition.ProposalUserPaths = new Dictionary<string, string>();
                    foreach (var proposalUser in proposalUsers)
                    {
                        requisition.ProposalUserPaths.Add(proposalUser, userImages.FirstOrDefault(i => i.UserName == proposalUser)?.Path);
                    } // foreach
                } // if
            } // foreach

            return requisitions;
        } // GetRequisitions

        /// <summary>
        /// Получение списка элементов
        /// </summary>
        /// <param name="machine">Элемент, по которому необходимо отфильтровать</param>
        public async Task<List<Requisition>> GetRequisitions(Machine machine)
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

            // Коллекция фото пользователей
            var requisitionUsers = requisitions.GroupBy(i => i.UserName).Select(i => i.Key).ToList();

            var userImages = await _context.UserImages.Where(i => requisitionUsers.Contains(i.UserName)).ToListAsync();

            // Расчет приоритета для заявок
            var requisitionPhones = requisitions.GroupBy(item => item.Phone).Select(item => item.Key.Substring(item.Key.Length - 7, 7));
            var priorities = await _context.Requisitions
                .GroupBy(i => new { i.Phone, i.UserName })
                .Where(i => requisitionPhones.Contains(i.Key.Phone.Substring(i.Key.Phone.Length - 7, 7)))
                .Select(i => new { Phone = i.Key.Phone.Substring(i.Key.Phone.Length - 7, 7), i.Key.UserName, CreateDate = i.Min(item => item.CreateDate) })
                .OrderBy(i => i.CreateDate)
                .ToListAsync();
            foreach (var requisition in requisitions)
            {
                var priority = priorities.FirstOrDefault(i => i.Phone == requisition.Phone.Substring(requisition.Phone.Length - 7, 7));
                requisition.UserPriority = priority.UserName != requisition.UserName ? new KeyValuePair<string, DateTime>(priority.UserName, priority.CreateDate) : null;

                requisition.UserImagePath = userImages.FirstOrDefault(i => i.UserName == requisition.UserName)?.Path;
            } // foreach

            return requisitions.Distinct().ToList();
        } // GetRequisitions

        /// <summary>
        /// Получение списка элементов
        /// </summary>
        /// <param name="machine">Элемент, по которому необходимо отфильтровать</param>
        public async Task<List<string>> GetProposalPaths(Machine machine)
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

            return proposals.GroupBy(i => i.UserName).Select(i => i.Key).ToList();
        } // GetProposalPaths

        /// <summary>
        /// Получение отфильтрованного списка элементов
        /// </summary>
        /// <param name="searchString">Поисковая строка</param>
        public async Task<List<Requisition>> GetFilteredRequisitions(string searchString, DateTime? startDate, DateTime? endDate, bool ownRecords, string userName)
        {
            var requisitions =  await _context.Requisitions
                .Include(i => i.Machine.MachineType.ParentMachineType)
                .Where(i => (string.IsNullOrEmpty(searchString) ||
                    (i.Id + 
                    i.UserName + 
                    i.Phone +
                    i.ContactName + 
                    i.Location +
                    i.Comment +
                    i.Machine.Name).Contains(searchString)) &&
                    (startDate == null || i.CreateDate.Date >= startDate.Value.Date) &&
                    (endDate == null || i.CreateDate.Date <= endDate.Value.Date) &&
                    (!ownRecords || i.UserName == userName))
                .ToListAsync();

            // Коллекция фото пользователей
            var requisitionUsers = requisitions.GroupBy(i => i.UserName).Select(i => i.Key).ToList();

            var userImages = await _context.UserImages.ToListAsync();

            // Расчет приоритета для заявок
            var priorities = requisitions
                    .OrderBy(i => i.CreateDate)
                    .GroupBy(i => new { i.Phone, i.UserName })
                    .Select(i => new { Phone = i.Key.Phone.Substring(i.Key.Phone.Length - 7, 7), i.Key.UserName, CreateDate = i.Min(item => item.CreateDate) })
                    .ToList();
            foreach (var requisition in requisitions)
            {
                // Приоритет заявки
                var priority = priorities.FirstOrDefault(i => i.Phone == requisition.Phone.Substring(requisition.Phone.Length - 7, 7));
                requisition.UserPriority = priority.UserName != requisition.UserName ? new KeyValuePair<string, DateTime>(priority.UserName, priority.CreateDate) : null;

                // Путь к изображению пользователя
                requisition.UserImagePath = userImages.FirstOrDefault(i => i.UserName == requisition.UserName)?.Path;

                // Пользователи, имеющие доступные предложения по заявке
                var proposalUsers = await GetProposalPaths(requisition.Machine);
                if (proposalUsers.Count > 0)
                {
                    requisition.ProposalUserPaths = new Dictionary<string, string>();
                    foreach (var proposalUser in proposalUsers)
                    {
                        requisition.ProposalUserPaths.Add(proposalUser, userImages.FirstOrDefault(i => i.UserName == proposalUser)?.Path);
                    } // foreach
                } // if
            } // foreach

            return requisitions;
        } // GetFilteredRequisitions

        /// <summary>
        /// Получение элемента
        /// </summary>
        /// <param name="id">ИД искомого элемента</param>
        public async Task<Requisition> GetRequisition(int id)
        {
            return await _context.Requisitions
                .Include(i => i.Machine.MachineType.ParentMachineType)
                .FirstOrDefaultAsync(i => i.Id == id);
        } // GetRequisition

        /// <summary>
        /// Добавление элемента
        /// </summary>
        /// <param name="element">Добавляемый элемент</param>
        public async Task AddRequisition(Requisition element)
        {
            _context.Requisitions.Add(element);
            await _context.SaveChangesAsync();
        } // AddRequisition

        /// <summary>
        /// Редактирование элемента
        /// </summary>
        /// <param name="element">Редактируемый элемент</param>
        public async Task EditRequisition(Requisition element)
        {
            Regex regex = new Regex(@"\D+");
            element.Phone = regex.Replace(element.Phone, "");

            _context.Entry(element).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        } // EditRequisition

        /// <summary>
        /// Редактирование элементов
        /// </summary>
        /// <param name="oldUserName">Старое имя пользователя</param>
        /// <param name="newUserName">Новое имя пользователя</param>
        public async Task EditRequisitions(string oldUserName, string newUserName)
        {
            await _context.Requisitions.Where(i => i.UserName == oldUserName).ForEachAsync(i =>
            {
                i.UserName = newUserName;
            });

            await _context.SaveChangesAsync();
        } // EditRequisitions

        /// <summary>
        /// Удаление элемента
        /// </summary>
        /// <param name="id">ИД удаляемого элемента</param>
        public async Task RemoveRequisition(int id)
        {
            // Удаляемый элемент
            var element = await GetRequisition(id);

            // Удаление элемента из базы
            _context.Entry(element).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        } // RemoveRequisition

        /// <summary>
        /// Удаление элементов по пользователю
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        public async Task RemoveRequisitions(string userName)
        {
            var elements = await _context.Requisitions.Where(i => i.UserName == userName).ToListAsync();

            _context.Requisitions.RemoveRange(elements);
            await _context.SaveChangesAsync();
        } // RemoveRequisitions
    }
}
