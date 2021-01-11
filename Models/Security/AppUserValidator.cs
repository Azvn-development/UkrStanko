using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace UkrStanko.Models.Security
{
    public class AppUserValidator: UserValidator<AppUser>
    {
        public override Task<IdentityResult> ValidateAsync(UserManager<AppUser> userManager, AppUser user)
        {
            List<IdentityError> errors = new List<IdentityError>();

            if (string.IsNullOrEmpty(user.UserName.Trim())) errors.Add(new IdentityError { Description = "Вы указали пустое имя" });
            if (!Regex.IsMatch(user.UserName, @"^[a-zA-Z0-9а-яА-Я_]+$")) errors.Add(new IdentityError { Description = "В имени разрешается указывать буквы английского или русского языков, цифры и нижнее подчеркивание" });
            if (user.UserName.Contains("admin")) errors.Add(new IdentityError { Description = "Логин пользователя не должен содержать слово 'admin'" });

            return Task.FromResult(errors.Count > 0 ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success);
        }
    }
}