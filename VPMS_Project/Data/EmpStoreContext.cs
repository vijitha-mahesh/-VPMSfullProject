using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VPMS_Project.Models;


namespace VPMS_Project.Data
{
    public class EmpStoreContext : IdentityDbContext<ApplicationUser>
    {
        private readonly DbContextOptions _options;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json");
            var configuration = builder.Build();

        }

        public EmpStoreContext(DbContextOptions options) : base(options)
        {
            _options = options;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ChatUser>()
            .HasKey(x => new { x.ChatId, x.UserId });


        }

        public DbSet<Projects> Projects { get; set; }
        public DbSet<Employees> Employees { get; set; }

        public DbSet<Job> Job { get; set; }

        public DbSet<LeaveApply> LeaveApply { get; set; }

        public DbSet<TimeTracker> TimeTracker { get; set; }

        public DbSet<Attendence> Attendence { get; set; }

        public DbSet<MarkAttendence> MarkAttendence { get; set; }

        public DbSet<StandardWorkHours> StandardWorkHours { get; set; }


        public DbSet<TimeSheetTask> TimeSheetTask { get; set; }

        public DbSet<Staff_Task> Staff_Task { get; set; }

        public DbSet<TimeSheetCheck> TimeSheetCheck { get; set; }


        /*project management - lashini */



        public DbSet<DeletedProjects> DeletedProjects { get; set; }

        public DbSet<Customers> Customers { get; set; }

        public DbSet<DeletedCustomers> DeletedCustomers { get; set; }

        public DbSet<Tasks> Tasks { get; set; }

        public DbSet<DeletedTasks> DeletedTasks { get; set; }

        public DbSet<Salarys> Salarys { get; set; }



        /*project management - Dilum */

        public DbSet<PreSalesProjects> PreSalesProjects { get; set; }

        public DbSet<PreSalesCustomers> PreSalesCustomers { get; set; }

        public DbSet<PreSalesTasks> PreSalesTasks { get; set; }

        public DbSet<PreSalesProjectManager> PreSalesProjectManagers { get; set; }

        public DbSet<PreSalesBudget> PreSalesBudget { get; set; }

        public DbSet<PreSalesDeleteBudgets> PreSalesDeleteBudgets { get; set; }

        public DbSet<PreSalesDeletedCustomers> PreSalesDeletedCustomers { get; set; }

        public DbSet<PreSalesDeletedTasks> PreSalesDeletedTasks { get; set; }

        public DbSet<PreSalesDeletedProjects> PreSalesDeletedProjects { get; set; }

        public DbSet<PreSalesDocument> PreSalesDocument { get; set; }

        public DbSet<PreSalescollection> PreSalescollection { get; set; }

        public DbSet<BudgetLineItem> VW_BudgetLineItem { get; set; } //Todo MNl
        public DbSet<ProjectSummary> VW_ProjectSummary { get; set; } //Todo MNl
        public DbSet<Payment> Payments { get; set; } //Todo MNl

        //Live chat -hashitha

        public DbSet<Message> Messages { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatUser> ChatUsers { get; set; }

        //Review -hashitha 

        public DbSet<WorkQualityModel> WorkQuality { get; set; }

        public DbSet<Communication> Communication { get; set; }







    }
}
