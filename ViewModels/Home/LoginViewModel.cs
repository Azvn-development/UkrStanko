using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

using UkrStanko.Models;

namespace UkrStanko.ViewModels.Home
{
    public class LoginViewModel
    {
        [Display(Name = "Логин")]
        [Required(ErrorMessage = Messages.requiredErrorMessage)]
        public string Login { get; set; } // логин пользователя

        [Display(Name = "Пароль")]
        [Required(ErrorMessage = Messages.requiredErrorMessage)]
        [DataType(DataType.Password)]
        public string Password { get; set; } // пароль пользователя

        // Конструктор
        public LoginViewModel() { } // LoginViewModel
    }
}
