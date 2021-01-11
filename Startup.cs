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
            // ���������� ����������� ���������� Razor � ������ ��������� �������
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            // ����������� MVC
            services.AddMvc(options =>
            {
                // ��������������� ����������� ��������� �� �������
                options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor((x) => "���� ������ ���� ������!");
                options.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() => "������� ���������� ��������!");
            });

            // ��������� ����������� �����������
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

            // ���������� ���������� �������� ��������� � �������� ������� SignalR
            services.AddSignalR();

            // ���������� ���������� ����������� � �������������� ������������� Identity
            services.AddTransient<IUserValidator<AppUser>, AppUserValidator>();
            services.AddTransient<IPasswordValidator<AppUser>, AppPasswordValidator>(services => new AppPasswordValidator(3));
            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Home/Login/";
                options.AccessDeniedPath = "/Home/Login/";
            });

            // ���������� ��� ������ ����������
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration["Data:UkrStanko:AppDbContext"]));
            services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(Configuration["Data:UkrStanko:AppIdentityDbContext"]));

            // ���������� �������� ��� ������ � ������ ������
            services.AddTransient<IProposalImageRepository, ProposalImageRepository>();
            services.AddTransient<IUserImageRepository, UserImageRepository>();
            services.AddTransient<IMachineRepository, MachineRepository>();
            services.AddTransient<IMessageRepository, MessageRepository>();
            services.AddTransient<IMachineTypeRepository, MachineTypeRepository>();
            services.AddTransient<IRequisitionRepository, RequisitionRepository>();
            services.AddTransient<IProposalRepository, ProposalRepository>();
            services.AddTransient<IReadedNoticeRepository, ReadedNoticeRepository>();

            // ������� ������������ �����
            services.AddTransient<JobFactory>();
            services.AddScoped<DataBaseJob>();
            services.AddScoped<IDataBaseService, DataBaseService>();
        } // ConfigureServices

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // ����������� ������� � ������������ � ������ ������
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            } else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            } // if

            // ����������� �� ��� ��������� �����������
            app.UseImageSharp();

            // ����������� ����������� �������� ������
            app.UseStaticFiles();

            // ���������� ����������� �������������
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
