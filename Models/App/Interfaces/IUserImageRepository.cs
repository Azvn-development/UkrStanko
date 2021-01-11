using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

using UkrStanko.Models.App;

namespace UkrStanko.Models.App.Interfaces
{
    public interface IUserImageRepository
    {
        /// <summary>
        /// Получение списка элементов
        /// </summary>
        Task<List<UserImage>> GetUserImages();

        /// <summary>
        /// Получение списка элементов
        /// </summary>
        /// <param name="userName">Имя элемента, к которому отностися изображение</param>
        Task<string> GetUserImagePath(string userName);

        /// <summary>
        /// Добавление элемента
        /// </summary>
        /// <param name="userName">ИД элемента, к которому отностися изображение</param>
        /// <param name="image">Изображение</param>
        /// <param name="hosting">Веб среда</param>
        Task AddUserImage(string userName, string image, IWebHostEnvironment hosting);

        /// <summary>
        /// Добавление элемента
        /// </summary>
        /// <param name="oldUserName">Старое имя пользователя</param>
        /// <param name="newUserName">Новое имя пользователя</param>
        /// <param name="hosting">Веб среда</param>
        Task EditUserImage(string oldUserName, string newUserName, IWebHostEnvironment hosting);

        /// <summary>
        /// Удаление элемента
        /// </summary>
        /// <param name="userName">ИД элемента, к которому отностися изображение</param>
        /// <param name="hosting">Веб среда</param>
        Task RemoveUserImage(string userName, IWebHostEnvironment hosting);
    }
}
