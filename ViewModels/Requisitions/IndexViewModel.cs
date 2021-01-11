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
    public class IndexViewModel
    {
        public List<Requisition> Requisitions { get; set; } // список заявок

        // Конструктор
        public IndexViewModel() { } // IndexViewModel
        public IndexViewModel(List<Requisition> requisitions)
        {
            Requisitions = requisitions;
        } // IndexViewModel
    }
}
