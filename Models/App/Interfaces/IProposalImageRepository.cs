using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

using UkrStanko.Models.App;

namespace UkrStanko.Models.App.Interfaces
{
    public interface IProposalImageRepository
    {
        /// <summary>
        /// Получение списка элементов
        /// </summary>
        /// <param name="proposalId">ИД элемента, к которому отностися изображение</param>
        Task<List<string>> GetProposalImagesPath(int proposalId);

        /// <summary>
        /// Добавление списка элементов
        /// </summary>
        /// <param name="proposalId">ИД элемента, к которому отностися изображение</param>
        /// <param name="newImages">Список изображений</param>
        /// <param name="hosting">Веб среда</param>
        Task AddProposalImages(int proposalId, List<string> newImages, IWebHostEnvironment hosting);

        /// <summary>
        /// Проверка списка элементов
        /// </summary>
        /// <param name="proposalId">ИД элемента, к которому отностися изображение</param>
        /// <param name="loadedImages">Список загруженных изображений</param>
        /// <param name="hosting">Веб среда</param>
        Task CheckProposalImages(int proposalId, List<string> loadedImages, IWebHostEnvironment hosting);

        /// <summary>
        /// Удаление списка элементов
        /// </summary>
        /// <param name="proposalId">ИД элемента, к которому отностися изображение</param>
        /// <param name="hosting">Веб среда</param>
        Task RemoveProposalImages(int proposalId, IWebHostEnvironment hosting);

        /// <summary>
        /// Удаление списка элементов
        /// </summary>
        /// <param name="deleteImages">Список картинок</param>
        /// <param name="hosting">Веб среда</param>
        Task RemoveProposalImages(IEnumerable<ProposalImage> deletedImages, IWebHostEnvironment hosting);
    }
}
