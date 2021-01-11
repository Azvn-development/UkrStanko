using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

using UkrStanko.Models;
using UkrStanko.Models.Security;

namespace UkrStanko.ViewModels.Users
{
    public class CreateViewModel
    {
        [Display(Name = "Имя пользователя")]
        [Required(ErrorMessage = Messages.requiredErrorMessage)]
        public string UserName { get; set; } // имя пользователя

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = Messages.emailErrorMessage)]
        [EmailAddress(ErrorMessage = Messages.emailErrorMessage)]
        [Required(ErrorMessage = Messages.requiredErrorMessage)]
        public string Email { get; set; } // e-mail пользователя

        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = Messages.requiredErrorMessage)]
        public string Password { get; set; } // пароль пользователя

        [Display(Name = "Подтверждение пароля")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = Messages.passwordErrorMessage)]
        [Required(ErrorMessage = Messages.requiredErrorMessage)]
        public string ConfirmPassword { get; set; } // подтверждение пароля пользователя

        [Display(Name = "Роль")]
        [Required(ErrorMessage = Messages.requiredErrorMessage)]
        public string Role { get; set; } // роль пользователя

        public string Image { get; set; } // фото пользователя

        // Вспомогательные коллекции
        public List<AppRole> Roles { get; set; } // все роли приложения

        // Конструктор
        public CreateViewModel() { } // CreateViewModel
        public CreateViewModel(List<AppRole> roles)
        {
            Roles = roles;
        } // CreateViewModel
    }
}
