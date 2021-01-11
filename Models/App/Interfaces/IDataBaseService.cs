using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using UkrStanko.Models.App;

namespace UkrStanko.Models.App.Interfaces
{
    public interface IDataBaseService
    {
        /// <summary>
        /// Проверка изображений на сервере без записи в БД
        /// </summary>
        Task CheckImages();

        /// <summary>
        /// Проверка заявок с датой создания более 3-х месяцев
        /// </summary>
        Task CheckRequisitions();

        /// <summary>
        /// Проверка предложений с датой создания более года
        /// </summary>
        Task CheckProposals();

        /// <summary>
        /// Проверка сообщений с датой создания более года
        /// </summary>
        Task CheckMessages();
    }
}
