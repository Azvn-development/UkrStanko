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
    public class CreateViewModel
    {
        [Display(Name = "Телефон")]
        [Required(ErrorMessage = Messages.requiredErrorMessage)]
        [MinLength(7, ErrorMessage = Messages.phoneMinLengthErrorMessage)]
        public string Phone { get; set; } // телефон клиента

        [Display(Name = "Клиент")]
        [Required(ErrorMessage = Messages.requiredErrorMessage)]
        public string ContactName { get; set; } // имя клиента

        [Display(Name = "Локация")]
        [Required(ErrorMessage = Messages.requiredErrorMessage)]
        public string Location { get; set; } // локация клиента

        [Display(Name = "Станок")]
        [Required(ErrorMessage = Messages.requiredErrorMessage)]
        public string Machine { get; set; } // станок

        [Display(Name = "Комментарий")]
        public string Comment { get; set; } // комментарий

        // Вспомогательные коллекции
        public List<Machine> Machines { get; set; } // все станки приложения

        // Конструктор
        public CreateViewModel() { } // CreateViewModel
        public CreateViewModel(string phone, string contactName, string location, List<Machine> machines)
        {
            Phone = phone;
            ContactName = contactName;
            Location = location;

            Machines = machines;
        } // CreateViewModel
    }
}
