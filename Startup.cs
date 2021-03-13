using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Licenta.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Licenta.Utility;
using Licenta.Services.FileManager;
using Licenta.Models;
using Licenta.Hubs;

namespace Licenta
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            // SignalR pentru chat
            services.AddSignalR();
                // .AddAzureSignalR();

            // Identity user & roles: options => options.SignIn.RequireConfirmedAccount = true
            services.AddIdentity<IdentityUser,IdentityRole>().AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            services.AddRazorPages();
            // Pentru autorizare
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

            // serviciu de autentificare folosind facebook
            services.AddAuthentication().AddFacebook(options =>
            {
                options.AppId = "466176558139835";
                options.AppSecret = "f0dbe3c21dd052fae97744ea9babcbc7";
            });
            // serviciu de autentificare folosind Google
            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = "827539349601-15h9pfogf11vm5h9ou58virjt6qf8pfj.apps.googleusercontent.com";
                options.ClientSecret = "zMxhfOPRE6-qm219iagk3wX_";
            });

            // serviciu de file Manager, imagini de profil
            services.AddTransient<IFileManager, FileManager>();

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

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // Rutele
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{area=Clienti}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                
                // pentru chat
                endpoints.MapHub<ChatHub>("/chathub");
            });
        }
    }
}
