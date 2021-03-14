using Dinder.Data;
using DinderDL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Dinder
{
    public class Startup
    {
        private string _contentRootPath = "";
        public static string DinderMDFDirectory
        {
            get
            {
                var directoryPath = AppDomain.CurrentDomain.BaseDirectory;
                directoryPath = Path.GetFullPath(Path.Combine(directoryPath, "..//..//..//"));
                return directoryPath;
            }
        }
        public string ConnectionString { get; set; }


        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _contentRootPath = environment.ContentRootPath;
            Configuration = configuration;
            ConnectionString = "Server=(LocalDB)\\MSSQLLocalDB; " + "AttachDbFilename=" + DinderMDFDirectory + "\\Dinder.mdf;" + "Integrated Security=True;MultipleActiveResultSets=True;Connect Timeout=30;";
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string conn = Configuration.GetConnectionString("IdentityDB");

            string conn2 = Configuration.GetConnectionString("DinderDB");

            conn2 = conn2.Replace("%CONTENTROOTPATH%", _contentRootPath);


            services.AddDbContext<UserEntityContext>(options => options.UseSqlServer(conn2));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddTransient<DbInitializer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
