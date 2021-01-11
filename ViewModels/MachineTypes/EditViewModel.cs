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
    public class EditViewModel
    {
        public int Id { get; set; } // ид станка

        [Display(Name = "Тип станка")]
        [Required(ErrorMessage = Messages.requiredErrorMessage)]
        public string Name { get; set; } // тип станка

        // Конструктор
        public EditViewModel() { } // EditViewModel
        public EditViewModel(MachineType machineType)
        {
            Id = machineType.Id;
            Name = machineType.Name;
        } // EditViewModel
    }
}
