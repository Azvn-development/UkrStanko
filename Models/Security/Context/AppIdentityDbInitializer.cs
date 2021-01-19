using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Web;
using System.Threading.Tasks;

namespace UkrStanko.Models.Security.Context
{
    public class AppIndentityDbInitializer
    {
        public static async Task InitializeAsync(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            
        } // InitializeAsync
    } // UkrStankoDbInitializer
}