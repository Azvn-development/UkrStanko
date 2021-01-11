using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

using UkrStanko.Models;

namespace UkrStanko.ViewModels.Home
{
    public class PasswordViewModel
    {
        [Display(Name = "Логин")]
        [Required(ErrorMessage = Messages.requiredErrorMessage)]
        public string Login { get; set; } // логин пользователя

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = Messages.emailErrorMessage)]
        [EmailAddress(ErrorMessage = Messages.emailErrorMessage)]
        [Required(ErrorMessage = Messages.requiredErrorMessage)]
        public string Email { get; set; } // e-mail пользователя

        [Display(Name = "Новый пароль")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = Messages.requiredErrorMessage)]
        public string Password { get; set; } // пароль пользователя

        [Display(Name = "Подтверждение пароля")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = Messages.passwordErrorMessage)]
        [Required(ErrorMessage = Messages.requiredErrorMessage)]
        public string ConfirmPassword { get; set; } // подтверждение пароля пользователя

        // Конструктор
        public PasswordViewModel() { }
        public PasswordViewModel(string login, string email) 
        {
            Login = login;
            Email = email;
        } // PasswordViewModel
    }
}
