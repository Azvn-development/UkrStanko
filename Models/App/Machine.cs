using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UkrStanko.Models.App
{
    public class Machine
    {
        [Display(Name = "Номер")]
        public int Id { get; set; }

        [Display(Name = "Модель")]
        public string Name { get; set; }

        [Display(Name = "Тип станка")]
        public int MachineTypeId { get; set; }
        public MachineType MachineType { get; set; }

        // Вспомогательные поля
        [NotMapped]
        [Display(Name = "Аналог")]
        public string Analogue { get; set; }
    }
}