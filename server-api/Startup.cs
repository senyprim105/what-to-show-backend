using Microsoft.VisualBasic;
using System.Reflection.Metadata;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Omu.AwesomeMvc;
using server_api.Data;
using server_api.Data.Models;
using server_api.Data.Models.Interfaces;
using server_api.Data.Models.Repositories;
using server_api.Auth;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace server_api
{
    public class Startup
    {
        void InjectionDependency(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IFileRepository, EFFileRepository>();
            services.AddTransient<IGenreRepository, EFGenreRepository>();
            services.AddTransient<IMovieRepository, EFMovieRepository>();
            services.AddTransient<IPersonRepository, EFPersonRepository>();
            services.AddTransient<IReviewRepository, EFReviewRepository>();
            services.AddTransient<IUserDataRepository, EFUserDataRepository>();
            services.AddTransient<IFileUpload, FileUpload>();
        }
        private const string SecretKey = "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH"; // todo: get this from somewhere secure
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
        private readonly string dbSetting = "db.json";
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddConfiguration(configuration)
            .AddJsonFile(dbSetting, optional: true)
            .AddJsonFile($"{Path.GetFileNameWithoutExtension(dbSetting)}.${env.EnvironmentName}.${Path.GetExtension(dbSetting)}", optional: true)
            .Build();
        }
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddDbContext<DbApp>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 3;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<DbApp>()
            .AddDefaultTokenProviders();

            InjectionDependency(services);

            //Так как недостатке прав в api вызывается ридирект на login или на AccessDenied со статусом 200:
            // - меняем заголовки для случаев перенапрвавления с api
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.LoginPath = "/Login";
                options.AccessDeniedPath = "/AccessDenided";
                options.ExpireTimeSpan = TimeSpan.FromHours(3);
                options.SlidingExpiration = true;
                options.Events.OnRedirectToLogin = context =>
                       {
                           System.Diagnostics.Debug.WriteLine($"redirect from {context.Request.Path} status={context.Response.StatusCode} to {context.RedirectUri}");
                           if (context.Request.Path.StartsWithSegments("/" + Auth.Constants.Strings.General.ApiDerictory)
                           && context.Response.StatusCode == 200)
                           {
                               System.Diagnostics.Debug.WriteLine($"Change status code to 401");

                               context.Response.Headers["Location"] = context.RedirectUri;
                               context.Response.StatusCode = 401;
                               return Task.CompletedTask;
                           }
                           context.Response.Redirect(context.RedirectUri);
                           return Task.CompletedTask;
                       };
                options.Events.OnRedirectToAccessDenied = context =>
                      {
                          System.Diagnostics.Debug.WriteLine($"redirect from {context.Request.Path} status={context.Response.StatusCode} to {context.RedirectUri}");
                          if (context.Request.Path.StartsWithSegments("/" + Auth.Constants.Strings.General.ApiDerictory)
                          && context.Response.StatusCode == 200)
                          {
                              System.Diagnostics.Debug.WriteLine($"Change status code to 401");

                              context.Response.Headers["Location"] = context.RedirectUri;
                              context.Response.StatusCode = 401;
                              return Task.CompletedTask;
                          }
                          context.Response.Redirect(context.RedirectUri);
                          return Task.CompletedTask;
                      };

            });

            var provider = new AweMetaProvider();

            services.AddMvc(options =>
                    {
                        options.EnableEndpointRouting = false;
                        options.ModelMetadataDetailsProviders.Add(provider);
                    });
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddHttpContextAccessor();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void  Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            if (env.IsDevelopment())
            {
                using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetRequiredService<DbApp>();

                    if (!context.Database.CanConnect())//Создаем БД
                    {
                    context.Database.Migrate();
                    Repository.CreateDefaultAccount(serviceProvider,configuration).Wait();
                    }
                }
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();

            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMvc(configureRoutes =>
            {
                configureRoutes.MapRoute(name: "home", template: "/", defaults: new { controller = "Home", action = "About" });
                configureRoutes.MapRoute(name: "login", template: "/Login", defaults: new { controller = "Account", action = "Login" });
                configureRoutes.MapRoute(name: "about", template: "/About", defaults: new { controller = "Home", action = "About" });
                configureRoutes.MapRoute(name: "accessdenided", template: "/AccessDenided", defaults: new { controller = "Home", action = "AccessDenided" });
                configureRoutes.MapRoute(name: "admin", template: "/Admin", defaults: new { controller = "Admin", action = "Index" });
                configureRoutes.MapRoute(name: "admin-object", template: "/Admin/{controller}/{action=Index}/{id?}");
                configureRoutes.MapRoute(name: "image", template: "/image/{id?}", defaults: new { controller = "Image", action = "GetImage" });
            });
        }
    }
}
