using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

using UkrStanko.Models;
using UkrStanko.Models.App;

namespace UkrStanko.ViewModels.Requisitions
{
    public class DetailsViewModel
    {
        public Requisition Requisition { get; set; } // просматриваемая заявка

        // Вспомогательные коллекции
        public List<Proposal> Proposals { get; set; } // предложения на заявку

        // Конструктор
        public DetailsViewModel() { } // EditViewModel
        public DetailsViewModel(Requisition requisition, List<Proposal> proposals)
        {
            // Инициализация полей данного класса
            Requisition = requisition;
            Proposals = proposals;
        } // CreateViewModel
    }
}
