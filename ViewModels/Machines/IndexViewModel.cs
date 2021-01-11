using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

using UkrStanko.Models;
using UkrStanko.Models.App;

namespace UkrStanko.ViewModels.Machines
{
    public class IndexViewModel
    {
        public List<Machine> Machines { get; set; } // список станков

        // Конструктор
        public IndexViewModel() { } // IndexViewModel
        public IndexViewModel(List<Machine> machines)
        {
            Machines = machines;
        } // IndexViewModel
    }
}
