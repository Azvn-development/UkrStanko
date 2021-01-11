using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

using UkrStanko.Models.App.Context;
using UkrStanko.Models.App.Interfaces;

namespace UkrStanko.Models.App.Repositories
{
    public class ProposalImageRepository: IProposalImageRepository
    {
        private readonly AppDbContext _context;

        public ProposalImageRepository(AppDbContext context)
        {
            _context = context;
        } // ProposalImageRepository

        /// <summary>
        /// Получение списка элементов
        /// </summary>
        /// <param name="proposalId">ИД элемента, к которому отностися картинка</param>
        public async Task<List<string>> GetProposalImagesPath(int proposalId)
        {
            return await _context.ProposalImages
                .Where(i => i.ProposalId == proposalId)
                .Select(i => i.Path)
                .ToListAsync();
        } // GetProposalImagesPath

        /// <summary>
        /// Добавление списка элементов
        /// </summary>
        /// <param name="proposalId">ИД элемента, к которому отностися картинка</param>
        /// <param name="newImages">Список картинок</param>
        /// <param name="hosting">Веб среда</param>
        public async Task AddProposalImages(int proposalId, List<string> newImages, IWebHostEnvironment hosting)
        {
            // Максимальный ID записи на сервере
            int maxId = 0;
            try
            {
                maxId = await _context.ProposalImages
                    .MaxAsync(i => i.Id);
            }
            catch { }

            // Загрузка фото на сервер
            if(!Directory.Exists(hosting.WebRootPath + "/images")) Directory.CreateDirectory(hosting.WebRootPath + "/images");
            if(!Directory.Exists(hosting.WebRootPath + "/images/proposals")) Directory.CreateDirectory(hosting.WebRootPath + "/images/proposals");

            if (newImages != null)
            {
                foreach(var image in newImages)
                {
                    string name = $"{maxId + 1}.jpg";
                    string path = $"/images/proposals/{name}";

                    var newImage = Image.Load(Convert.FromBase64String(image[(image.IndexOf(',') + 1)..]));
                    newImage.Mutate();
                    await newImage.SaveAsync(hosting.WebRootPath + path);

                    _context.ProposalImages.Add(new ProposalImage { Path = path, Name = name, ProposalId = proposalId });
                    maxId++;
                } // for
            } // if

            await _context.SaveChangesAsync();
        } // AddProposalImages

        /// <summary>
        /// Проверка списка элементов
        /// </summary>
        /// <param name="proposalId">ИД элемента, к которому отностися картинка</param>
        /// <param name="loadedImages">Список загруженных картинок</param>
        /// <param name="hosting">Веб среда</param>
        public async Task CheckProposalImages(int proposalId, List<string> loadedImages, IWebHostEnvironment hosting)
        {
            var allImages = await _context.ProposalImages.Where(i => i.ProposalId == proposalId).ToListAsync();

            if(allImages.Any())
            {
                List<ProposalImage> deleteImages;
                if (loadedImages == null) deleteImages = allImages;
                else deleteImages = allImages.Where(i => !loadedImages.Contains(i.Path)).ToList();

                await RemoveProposalImages(deleteImages, hosting);
            } // if
        } // CheckProposalImages

        /// <summary>
        /// Удаление списка элементов
        /// </summary>
        /// <param name="proposalId">ИД элемента, к которому отностися картинка</param>
        /// <param name="newImages">Список картинок</param>
        /// <param name="hosting">Веб среда</param>
        public async Task RemoveProposalImages(int proposalId, IWebHostEnvironment hosting)
        {
            var images = await _context.ProposalImages.Where(i => i.ProposalId == proposalId).ToListAsync();
            foreach (var item in images)
            {
                string path = hosting.WebRootPath + item.Path;
                if(File.Exists(path))
                {
                    File.Delete(path);
                } // if
            } // foreach
        } // RemoveProposalImages

        /// <summary>
        /// Удаление списка элементов
        /// </summary>
        /// <param name="deleteImages">Список картинок</param>
        /// <param name="hosting">Веб среда</param>
        public async Task RemoveProposalImages(IEnumerable<ProposalImage> deletedImages, IWebHostEnvironment hosting)
        {
            _context.ProposalImages.RemoveRange(deletedImages);

            foreach (var item in deletedImages)
            {
                string path = hosting.WebRootPath + item.Path;
                if (File.Exists(path))
                {
                    File.Delete(path);
                } // if
            } // foreach

            await _context.SaveChangesAsync();
        } // RemoveImages
    }
}
