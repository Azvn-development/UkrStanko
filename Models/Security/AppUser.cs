using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using UkrStanko.Models;

namespace UkrStanko.Models.Security
{
    public class AppUser: IdentityUser {
        [Display(Name = "Имя пользователя")]
        public override string UserName { get; set; } // имя пользователя

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = Messages.emailErrorMessage)]
        [EmailAddress(ErrorMessage = Messages.emailErrorMessage)]
        public override string Email { get; set; } // e-mail пользователя

        [Display(Name = "Роль")]
        public string Role { get; set; } // роль пользователя
    }
}