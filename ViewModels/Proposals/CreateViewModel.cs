using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using UkrStanko.Models;
using UkrStanko.Models.App;

namespace UkrStanko.ViewModels.Proposals
{
    public class CreateViewModel
    {
        [Display(Name = "Локация")]
        [Required(ErrorMessage = Messages.requiredErrorMessage)]
        public string Location { get; set; } // локация станка

        [Display(Name = "Станок")]
        [Required(ErrorMessage = Messages.requiredErrorMessage)]
        public string Machine { get; set; } // имя станка

        [Display(Name = "Цена покупки")]
        [Required(ErrorMessage = Messages.requiredErrorMessage)]
        [RegularExpression(@"[0-9]+", ErrorMessage = Messages.priceErrorMessage)]
        [Range(0, int.MaxValue, ErrorMessage = Messages.rangeErrorMessage)]
        public int? PurshasePrice { get; set; } // цена покупки

        [Display(Name = "Цена продажи")]
        [Required(ErrorMessage = Messages.requiredErrorMessage)]
        [RegularExpression(@"[0-9]+", ErrorMessage = Messages.priceErrorMessage)]
        [Range(0, int.MaxValue, ErrorMessage = Messages.rangeErrorMessage)]
        public int? SellingPrice { get; set; } // цена продажи

        [Display(Name = "Комментарий")]
        public string Comment { get; set; } // комментарий

        public List<string> Images { get; set; } // Список base64 строк загружаемых изображений

        // Конструктор
        public CreateViewModel() { } // CreateViewModel
        public CreateViewModel(string location)
        {
            Location = location;
        } // CreateViewModel
    }
}
