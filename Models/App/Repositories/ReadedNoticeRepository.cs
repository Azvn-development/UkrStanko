using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.EntityFrameworkCore;

using UkrStanko.Models.App.Context;
using UkrStanko.Models.App.Interfaces;

namespace UkrStanko.Models.App.Repositories
{
    public class ReadedNoticeRepository: IReadedNoticeRepository
    {
        private readonly AppDbContext _context;

        public ReadedNoticeRepository(AppDbContext context)
        {
            _context = context;
        } // ReadedNoticeRepository

        /// <summary>
        /// Получение кол-ва прочитанных пользователем новостей
        /// </summary>
        /// <param name="userId">Имя пользователя</param>
        public async Task<int> GetUserReadedNoticesCounter(string userId)
        {
            var readedNotice = await _context.ReadedNotices.FirstOrDefaultAsync(i => i.UserId == userId);

            if (readedNotice != null) return readedNotice.ReadedNoticesCount;
            return 0;
        } // GetUserReadedNoticesCounter

        /// <summary>
        /// Добавление счетчика прочитанных новостей пользователем
        /// </summary>
        /// <param name="userId">Имя пользователя</param>
        public async Task AddUserReadedNoticesCounter(string userId)
        {
            _context.ReadedNotices.Add(new ReadedNotices { UserId = userId, ReadedNoticesCount = 0 });
            await _context.SaveChangesAsync();
        } // AddUserReadedNoticesCounter

        /// <summary>
        /// Изменение кол-ва прочитанных пользователями новостей
        /// </summary>
        /// <param name="userId">Имя пользователя</param>
        /// <param name="readedNoticesCount">Кол-во прочитанных пользователем новостей</param>
        public async Task EditUserReadedNoticesCounter(string userId, int readedNoticesCount)
        {
            var readedNotice = await _context.ReadedNotices.FirstOrDefaultAsync(i => i.UserId == userId);
            
            if(readedNotice != null && readedNoticesCount > 0)
            {
                readedNotice.ReadedNoticesCount = readedNoticesCount;
                await _context.SaveChangesAsync();
            } // if
        } // EditUserReadedNoticesCounter

        /// <summary>
        /// Изменение кол-ва прочитанных пользователями новостей на 1 пункт
        /// </summary>
        public async Task DecrementUsersReadedNoticesCounter()
        {
            await _context.ReadedNotices.ForEachAsync(i =>
            {
                if (i.ReadedNoticesCount > 0) i.ReadedNoticesCount--;
            });

            await _context.SaveChangesAsync();
        } // DecrementUsersReadedNoticesCounter

        /// <summary>
        /// Изменение кол-ва прочитанных пользователями новостей на N пунктов
        /// </summary>
        /// <param name="count">Кол-во пунктов</param>
        public async Task DecrementUsersReadedNoticesCounter(int count)
        {
            await _context.ReadedNotices.ForEachAsync(i =>
            {
                if (i.ReadedNoticesCount > 0) i.ReadedNoticesCount -= count;
            });

            await _context.SaveChangesAsync();
        } // DecrementUsersReadedNoticesCounter

        /// <summary>
        /// Удаление счетчика прочитанных новостей пользователем
        /// </summary>
        /// <param name="userId">Имя пользователя</param>
        public async Task RemoveUserReadedNoticesCounter(string userId)
        {
            var readedNotice = await _context.ReadedNotices.FirstOrDefaultAsync(i => i.UserId == userId);

            if(readedNotice != null)
            {
                _context.ReadedNotices.Remove(readedNotice);
                await _context.SaveChangesAsync();
            } // if
        } // RemoveUserReadedNoticesCounter
    }
}
