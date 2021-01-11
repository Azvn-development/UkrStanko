using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace UkrStanko.Models.Security
{
    public class AppPasswordValidator: PasswordValidator<AppUser>
    {
        public int RequiredLength { get; set; }

        public AppPasswordValidator(int length)
        {
            RequiredLength = length;
        } // AppPasswordValidator

        public override Task<IdentityResult> ValidateAsync(UserManager<AppUser> userManager, AppUser user, string password)
        {
            List<IdentityError> errors = new List<IdentityError>();

            if (string.IsNullOrEmpty(password) || password.Length < RequiredLength) errors.Add(new IdentityError { Description = $"Минимальная длина пароля {RequiredLength} символа!" });

            return Task.FromResult(errors.Count > 0 ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success);
        }
    }
}