using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

using UkrStanko.Models.App.Context;
using UkrStanko.Models.App.Interfaces;

namespace UkrStanko.Models.App.Repositories
{
    public class UserImageRepository: IUserImageRepository
    {
        private readonly AppDbContext _context;

        // Максимальная ширина фото
        private readonly int maxWidth = 300;

        public UserImageRepository(AppDbContext context)
        {
            _context = context;
        } // UserImageRepository

        /// <summary>
        /// Получение списка элементов
        /// </summary>
        public async Task<List<UserImage>> GetUserImages()
        {
            return await _context.UserImages.ToListAsync();
        } // GetUserImages

        /// <summary>
        /// Получение списка элементов
        /// </summary>
        /// <param name="userName">Имя элемента, к которому отностися картинка</param>
        public async Task<string> GetUserImagePath(string userName)
        {
            return await _context.UserImages
                .Where(i => i.UserName == userName)
                .Select(i => i.Path)
                .FirstOrDefaultAsync();
        } // GetUserImagePath

        /// <summary>
        /// Добавление элемента
        /// </summary>
        /// <param name="userName">Имя элемента, к которому отностися картинка</param>
        /// <param name="newImage">Изображение</param>
        /// <param name="hosting">Веб среда</param>
        public async Task AddUserImage(string userName, string image, IWebHostEnvironment hosting)
        {
            // Максимальный ID записи на сервере
            int maxId = 0;
            try
            {
                maxId = await _context.UserImages
                    .MaxAsync(i => i.Id);
            }
            catch { }

            // Загрузка фото на сервер
            if (!Directory.Exists(hosting.WebRootPath + "/images")) Directory.CreateDirectory(hosting.WebRootPath + "/images");
            if (!Directory.Exists(hosting.WebRootPath + "/images/users")) Directory.CreateDirectory(hosting.WebRootPath + "/images/users");

            if (image != null)
            {
                string name = $"{maxId + 1}.jpg";
                string path = $"/images/users/{name}";

                var newImage = Image.Load(Convert.FromBase64String(image[(image.IndexOf(',') + 1)..]));
                newImage.Mutate(action => {
                    action.Resize(new ResizeOptions { Mode = ResizeMode.Min, Size = new Size(maxWidth) })
                        .Crop(new Rectangle((newImage.Width - maxWidth) / 2, (newImage.Height - maxWidth) / 2, maxWidth, maxWidth));
                });
                await newImage.SaveAsync(hosting.WebRootPath + path);

                _context.UserImages.Add(new UserImage { Path = path, Name = name, UserName = userName });
            } // if

            await _context.SaveChangesAsync();
        } // AddUserImage

        /// <summary>
        /// Добавление элемента
        /// </summary>
        /// <param name="oldUserName">Старое имя пользователя</param>
        /// <param name="newUserName">Новое имя пользователя</param>
        /// <param name="hosting">Веб среда</param>
        public async Task EditUserImage(string oldUserName, string newUserName, IWebHostEnvironment hosting)
        {
            var userImage = await _context.UserImages.FirstOrDefaultAsync(i => i.UserName == oldUserName);
            if(userImage != null)
            {
                userImage.UserName = newUserName;
                await _context.SaveChangesAsync();
            } // if
        } // EditUserImage

        /// <summary>
        /// Удаление элемента
        /// </summary>
        /// <param name="userName">Имя элемента, к которому отностися картинка</param>
        /// <param name="hosting">Веб среда</param>
        public async Task RemoveUserImage(string userName, IWebHostEnvironment hosting)
        {
            var image = await _context.UserImages.FirstOrDefaultAsync(i => i.UserName == userName);

            if (image != null)
            {
                _context.UserImages.Remove(image);
                await _context.SaveChangesAsync();

                string path = hosting.WebRootPath + image.Path;
                if (File.Exists(path))
                {
                    File.Delete(path);
                } // if
            } // if
        } // RemoveUserImage
    }
}
