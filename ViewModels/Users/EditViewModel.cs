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
    public class EditViewModel
    {
        public string Id { get; set; } // ид пользователя

        [Display(Name = "Имя пользователя")]
        [Required(ErrorMessage = Messages.requiredErrorMessage)]
        public string UserName { get; set; } // имя пользователя

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = Messages.emailErrorMessage)]
        [EmailAddress(ErrorMessage = Messages.emailErrorMessage)]
        [Required(ErrorMessage = Messages.requiredErrorMessage)]
        public string Email { get; set; } // e-mail пользователя

        [Display(Name = "Роль")]
        [Required(ErrorMessage = Messages.requiredErrorMessage)]
        public string Role { get; set; } // роль пользователя

        public string ImagePath { get; set; } // путь к загруженной фотографии пользователя

        public string Image { get; set; } // новое фото пользователя

        // Вспомогательные коллекции
        public List<AppRole> Roles { get; set; } // все роли приложения

        // Конструктор
        public EditViewModel() { } // EditViewModel
        public EditViewModel(AppUser user, string imagePath, List<AppRole> roles)
        {
            Id = user.Id;
            UserName = user.UserName;
            Email = user.Email;
            Role = user.Role;
            ImagePath = imagePath;

            Roles = roles;
        } // CreateViewModel
    }
}
