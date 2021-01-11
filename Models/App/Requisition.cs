using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UkrStanko.Models.App
{
    public class Requisition
    {
        [Display(Name = "Номер")]
        public int Id { get; set; }

        [Display(Name = "Дата")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Инициатор")]
        public string UserName { get; set; }

        [Display(Name = "Телефон")]
        public string Phone { get; set; }

        [Display(Name = "Клиент")]
        public string ContactName { get; set; }

        [Display(Name = "Локация")]
        public string Location { get; set; }

        [Display(Name = "Станок")]
        public int MachineId { get; set; }
        public Machine Machine { get; set; }

        [Display(Name = "Комментарий")]
        public string Comment { get; set; }

        // Вспомогательные поля
        [NotMapped]
        public KeyValuePair<string, DateTime>? UserPriority { get; set; }

        [NotMapped]
        public string UserImagePath { get; set; }

        [NotMapped]
        public Dictionary<string, string> ProposalUserPaths { get; set; }
    }
}