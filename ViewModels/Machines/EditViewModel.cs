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
    public class EditViewModel
    {
        public int Id { get; set; } // ид станка

        [Display(Name = "Модель")]
        [Required(ErrorMessage = Messages.requiredErrorMessage)]
        public string Name { get; set; } // модель станка

        [Display(Name = "Тип станка")]
        [Required(ErrorMessage = Messages.requiredErrorMessage)]
        public string MachineType { get; set; } // тип станка
        public int MachineTypeId { get; set; } // ид подгруппы типа станка

        [Display(Name = "Аналог")]
        public string Analogue { get; set; } // аналог станка

        // Вспомогательные коллекции
        public List<Machine> Machines { get; set; } // все станки
        public List<MachineType> MachineTypes { get; set; } // все типы станков

        // Конструктор
        public EditViewModel() { } // EditViewModel
        public EditViewModel(Machine machine, string analogue, List<Machine> machines, List<MachineType> machineTypes)
        {
            Id = machine.Id;
            Name = machine.Name;
            MachineType = machine.MachineType.ParentMachineType.Name;
            MachineTypeId = machine.MachineTypeId;
            Analogue = analogue;

            Machines = machines;
            MachineTypes = machineTypes;
        } // EditViewModel
    }
}
