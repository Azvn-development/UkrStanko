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
    public class IndexViewModel
    {
        public List<Proposal> Proposals { get; set; } // список предложений

        // Конструктор
        public IndexViewModel() { } // IndexViewModel
        public IndexViewModel(List<Proposal> proposals)
        {
            Proposals = proposals;
        } // IndexViewModel
    }
}
