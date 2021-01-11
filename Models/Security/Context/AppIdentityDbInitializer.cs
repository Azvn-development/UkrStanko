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
            // Добавление ролей
            AppRole adminRole = new AppRole { Name = "Администратор" };
            AppRole managerRole = new AppRole { Name = "Менеджер" };

            if (await roleManager.FindByNameAsync("Администратор") == null) await roleManager.CreateAsync(adminRole);
            if (await roleManager.FindByNameAsync("Менеджер") == null) await roleManager.CreateAsync(managerRole);

            // Добавление пользователей
            if (await userManager.FindByNameAsync("root") == null)
            {
                AppUser adminUser = new AppUser { UserName = "root", Email = "root@gmail.com", PhoneNumber = "root", Role = adminRole.Name };
                var result = await userManager.CreateAsync(adminUser, "12346Qq");
                
                if(result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Администратор");
                } // if
            } // if
        } // InitializeAsync
    } // UkrStankoDbInitializer
}