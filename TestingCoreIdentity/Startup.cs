using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestingCoreIdentity.Data;
using TestingCoreIdentity.Models;
using TestingCoreIdentity.Services;
using Hangfire;

namespace TestingCoreIdentity
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
            //services.AddDbContext<ApplicationDbContext>(options =>
            //  options.UseSqlServer("Server=MGSERVER2012\\SQLEXPRESS;Database=TestingCoreIdentity;MultipleActiveResultSets=true;Persist Security Info=True;User ID=AspCoreUser;Password=Welcome@123;"));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // For hangfire
            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection")));


            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;


                // User settings
                options.User.RequireUniqueEmail = true;
                //    options.User.AllowedUserNameCharacters = "@.abcdefghijklmnopqrstuvwxyz";
            });





            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
         

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            app.UseBrowserLink();
            app.UseDeveloperExceptionPage();
            app.UseDatabaseErrorPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //}

            // Use hangfire to run job in background
            //https://code.msdn.microsoft.com/windowsdesktop/Integrate-background-jobs-be713dc4
            // http://docs.hangfire.io/en/latest/background-methods/performing-recurrent-tasks.html
            app.UseHangfireServer();
            app.UseHangfireDashboard();
            // End hangfire 

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
