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
    public class IndexViewModel
    {
        public List<MachineType> MachineTypes { get; set; } // список типов станков

        // Конструктор
        public IndexViewModel() { } // IndexViewModel
        public IndexViewModel(List<MachineType> machineTypes)
        {
            // Инициализация полей данного класса
            MachineTypes = machineTypes;
        } // IndexViewModel
    }
}
