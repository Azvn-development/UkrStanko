using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace UkrStanko.Models.App
{
    public class MachineType
    {
        [Display(Name = "Номер")]
        public int Id { get; set; }

        [Display(Name = "Тип станка")]
        public string Name { get; set; }

        [Display(Name = "Родительский тип")]
        public int? ParentMachineTypeId { get; set; }
        public MachineType ParentMachineType { get; set; }
    }
}