using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.Web;
using SixLabors.ImageSharp.Web.Caching;
using SixLabors.ImageSharp.Web.Commands;
using SixLabors.ImageSharp.Web.DependencyInjection;
using SixLabors.ImageSharp.Web.Middleware;
using SixLabors.ImageSharp.Web.Processors;
using SixLabors.ImageSharp.Web.Providers;

using UkrStanko.Models.App.Context;
using UkrStanko.Models.App.Interfaces;
using UkrStanko.Models.App.Repositories;
using UkrStanko.Models.App.Services;
using UkrStanko.Models.App.Sheduler;
using UkrStanko.Models.Hubs;
using UkrStanko.Models.Security.Context;
using UkrStanko.Models.Security;

namespace UkrStanko
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        } // Startup

        public void ConfigureServices(IServiceCollection services)
        {
            // Добавление возможности обновления Razor в режиме реального времени
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            // Подключение MVC
            services.AddMvc(options =>
            {
                // Переопределение стандартных сообщений об ошибках
                options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor((x) => "Поле должно быть числом!");
                options.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() => "Введите корректное значение!");
            });

            // Обработка загружаемых изображений
            services.AddImageSharp()
                .SetRequestParser<QueryCollectionRequestParser>()
                .Configure<PhysicalFileSystemCacheOptions>(options =>
                {
                    options.CacheFolder = "is-cache";
                })
                .SetCache(provider =>
                {
                    return new PhysicalFileSystemCache(
                                provider.GetRequiredService<IOptions<PhysicalFileSystemCacheOptions>>(),
                                provider.GetRequiredService<IWebHostEnvironment>(),
                                provider.GetRequiredService<IOptions<ImageSharpMiddlewareOptions>>(),
                                provider.GetRequiredService<FormatUtilities>());
                })
                .SetCacheHash<CacheHash>()
                .AddProvider<PhysicalFileSystemProvider>()
                .AddProcessor<ResizeWebProcessor>()
                .AddProcessor<FormatWebProcessor>()
                .AddProcessor<BackgroundColorWebProcessor>()
                .AddProcessor<JpegQualityWebProcessor>();

            // Добавление техгологии отправки сообщений в реальном времени SignalR
            services.AddSignalR();

            // Добавление технологии авторизации и аутентификации пользователей Identity
            services.AddTransient<IUserValidator<AppUser>, AppUserValidator>();
            services.AddTransient<IPasswordValidator<AppUser>, AppPasswordValidator>(services => new AppPasswordValidator(3));
            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Home/Login/";
                options.AccessDeniedPath = "/Home/Login/";
            });

            // Добавление баз данных приложения
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration["Data:UkrStanko:AppDbContext"]));
            services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(Configuration["Data:UkrStanko:AppIdentityDbContext"]));

            // Добавление сервисов для работы с базами данных
            services.AddTransient<IProposalImageRepository, ProposalImageRepository>();
            services.AddTransient<IUserImageRepository, UserImageRepository>();
            services.AddTransient<IMachineRepository, MachineRepository>();
            services.AddTransient<IMessageRepository, MessageRepository>();
            services.AddTransient<IMachineTypeRepository, MachineTypeRepository>();
            services.AddTransient<IRequisitionRepository, RequisitionRepository>();
            services.AddTransient<IProposalRepository, ProposalRepository>();
            services.AddTransient<IReadedNoticeRepository, ReadedNoticeRepository>();

            // Сервисы планировщика задач
            services.AddTransient<JobFactory>();
            services.AddScoped<DataBaseJob>();
            services.AddScoped<IDataBaseService, DataBaseService>();
        } // ConfigureServices

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Подключение страниц с исключениями и кодами ошибок
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            } else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            } // if

            // Подключение ПО для обработки изображений
            app.UseImageSharp();

            // Подключение статической отправки файлов
            app.UseStaticFiles();

            // Добавление возможности маршрутизации
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Notices}/{action=Index}/{id?}");

                endpoints.MapHub<NotificationHub>("/chat");
            });
        }
    }
}
