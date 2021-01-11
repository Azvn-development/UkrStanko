using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

using UkrStanko.Models;
using UkrStanko.Models.App;

namespace UkrStanko.ViewModels.Proposals
{
    public class EditViewModel
    {
        public int Id { get; set; } // ид предложения

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
        [Range(0.01, int.MaxValue, ErrorMessage = Messages.rangeErrorMessage)]
        public int? SellingPrice { get; set; } // цена продажи

        [Display(Name = "Комментарий")]
        public string Comment { get; set; } // комментарий

        public List<string> ProposalImages { get; set; } // пути к изображениям просматриваемого предложения

        public List<string> Images { get; set; } // Список base64 строк загружаемых изображений

        // Конструктор
        public EditViewModel() { } // EditViewModel
        public EditViewModel(Proposal proposal, List<string> proposalImages)
        {
            Id = proposal.Id;
            Location = proposal.Location;
            Machine = proposal.Machine.Name;
            PurshasePrice = proposal.PurshasePrice;
            SellingPrice = proposal.SellingPrice;
            Comment = proposal.Comment;
            ProposalImages = proposalImages;
        } // CreateViewModel
    }
}
