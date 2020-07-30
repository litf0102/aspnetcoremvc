using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using aspnetcoremvc.Utils;
using aspnetcoremvc.Entities;
using aspnetcoremvc.Services.UserApp.Dtos;
using aspnetcoremvc.Repositories;
using aspnetcoremvc.Services.UserApp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.IO;
using aspnetcoremvc.Services.DepartmentApp.Dtos;
using aspnetcoremvc.Services.DepartmentApp;
using aspnetcoremvc.Services.RoleApp;
using aspnetcoremvc.Services.MenuApp;
using aspnetcoremvc.Services.MenuApp.Dtos;
using aspnetcoremvc.Services.RoleApp.Dtos;
using aspnetcoremvc.Log4net;
using log4net.Repository;
using log4net;
using log4net.Config;
using aspnetcoremvc.Filters;
using aspnetcoremvc.Extensions;

namespace aspnetcoremvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public static ILoggerRepository logRepository { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // AppSetting定義
            services.AddSingleton(new AppsettingsHelper(Configuration));

            services.AddControllersWithViews();

            // Log4net DI
            services.AddSingleton<ILoggerHelper, LogHelper>();
            logRepository = LogManager.CreateRepository("");
            XmlConfigurator.Configure(logRepository, new FileInfo("Log4net.config.xml"));

            // AutoMapper DI
            services.AddAutoMapper();

            // DB ConnectionString
            var sqlConnectionString = Configuration.GetConnectionString("Default");

            // DBContext DI
            services.AddDbContext<MVCCOREDbContext>(options => options.UseSqlServer(sqlConnectionString));

            // BusinessLogic DI
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserAppService, UserAppService>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IMenuAppService, MenuAppService>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IDepartmentAppService, DepartmentAppService>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleAppService, RoleAppService>();

            services.AddMvc();
            // Sessionサービス定義
            services.AddSession();

            // JWTトークン認証
            services.AddAuthorizationExtension();

            // グローバル例外定義
            services.AddControllers(option =>
            {
                option.Filters.Add(typeof(GlobalExceptionsFilter));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory())
            });

            //Session
            app.UseSession();

            app.UseRouting();

            // 認証処理
            app.UseAuthentication();
            app.UseAuthorization();

            // AutoMapper mapping
            var cfg = app.UseAutoMapper();
            cfg.CreateMap<Menu, MenuDto>();
            cfg.CreateMap<MenuDto, Menu>();
            cfg.CreateMap<Department, DepartmentDto>();
            cfg.CreateMap<DepartmentDto, Department>();
            cfg.CreateMap<RoleDto, Role>();
            cfg.CreateMap<Role, RoleDto>();
            cfg.CreateMap<RoleMenuDto, RoleMenu>();
            cfg.CreateMap<RoleMenu, RoleMenuDto>();
            cfg.CreateMap<UserDto, User>();
            cfg.CreateMap<User, UserDto>();
            cfg.CreateMap<UserRoleDto, UserRole>();
            cfg.CreateMap<UserRole, UserRoleDto>();

            // ExceptionExtension
            app.UseCustomExceptionExtensions();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");
            });

            InitData.Initialize(app.ApplicationServices);
        }
    }
}
