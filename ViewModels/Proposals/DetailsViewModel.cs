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
    public class DetailsViewModel
    {
        public Proposal Proposal { get; set; } // просматриваемое предложение
        public List<string> ProposalImages { get; set; } // пути к изображениям просматриваемого предложения

        // Вспомогательные коллекции
        public List<Requisition> Requisitions { get; set; } // заявки на предложение

        // Конструктор
        public DetailsViewModel() { } // EditViewModel
        public DetailsViewModel(Proposal proposal, List<string> proposalImages, List<Requisition> requisitions)
        {
            // Инициализация полей данного класса
            Proposal = proposal;
            ProposalImages = proposalImages;
            Requisitions = requisitions;
        } // CreateViewModel
    }
}
