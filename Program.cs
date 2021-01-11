using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using UkrStanko.Models.App.Sheduler;
using UkrStanko.Models.Security;
using UkrStanko.Models.Security.Context;

namespace UkrStanko
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                // Инициализация ролей и пользователей
                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
                await AppIndentityDbInitializer.InitializeAsync(userManager, roleManager);

                // Запуск планировщика задач базы данных
                DataBaseSheduler.Start(services);
            } // using

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .UseDefaultServiceProvider(options =>
                        {
                            options.ValidateScopes = false;
                        });
                });
    }
}
