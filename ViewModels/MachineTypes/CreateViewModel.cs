using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

using UkrStanko.Models;
using UkrStanko.Models.App;

namespace UkrStanko.ViewModels.MachineTypes
{
    public class CreateViewModel
    {
        [Display(Name = "Тип станка")]
        [Required(ErrorMessage = Messages.requiredErrorMessage)]
        public string Name { get; set; } // тип станка

        // Конструктор
        public CreateViewModel() { } // CreateViewModel
    }
}
