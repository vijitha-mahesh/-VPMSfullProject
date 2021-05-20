using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VPMS_Project.Data;
using VPMS_Project.Repository;
using VPMS_Project.Models;
using VPMS_Project.Helpers;

namespace VPMS_Project
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            string connectionString = Configuration.GetConnectionString("default");
            services.AddDbContext<EmpStoreContext>(                
                options => options.UseSqlServer(connectionString));
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<EmpStoreContext>();
            
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
            });
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();
            services.AddHttpContextAccessor();
            services.AddRazorPages().AddRazorRuntimeCompilation();           
            services.AddScoped<IEmpRepository, EmpRepository>();
            services.AddScoped<JobRepository, JobRepository>();
            services.AddScoped<LeaveRepository, LeaveRepository>();
            services.AddScoped<TimeTrackRepo, TimeTrackRepo>();
            services.AddScoped<AttendenceRepo, AttendenceRepo>();
            services.AddScoped<TaskRepo, TaskRepo>();
            services.AddScoped<Repo, Repo>();
            services.AddScoped<Repo2, Repo2>();
            services.AddScoped<Repo3, Repo3>();
            services.AddScoped<Repo4, Repo4>();

            services.AddScoped<ProjectRepository, ProjectRepository>();
            services.AddScoped<CustomerRepository, CustomerRepository>();
            services.AddScoped<TaskRepository, TaskRepository>();
            services.AddScoped<ProjectManagerRepository, ProjectManagerRepository>();
            services.AddScoped<BudgetRepository, BudgetRepository>();
            services.AddScoped<DocumentRepository, DocumentRepository>();
            services.AddScoped<CollectionRepository, CollectionRepository>();
            services.AddScoped<PaymentRepository, PaymentRepository>();

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
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // endpoints.MapDefaultControllerRoute();
                endpoints.MapControllerRoute(
                    name:"Default",
                    pattern: "{controller=Account}/{action=Login}/{id?}"
                    );
            });
        }
    }
}
