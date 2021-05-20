﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VPMS_Project.Data;

namespace VPMS_Project.Migrations
{
    [DbContext(typeof(EmpStoreContext))]
    [Migration("20210212185112_newinit")]
    partial class newinit
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("VPMS_Project.Data.Attendence", b =>
                {
                    b.Property<int>("AttendenceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("AppliedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Approver")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("BreakingHours")
                        .HasColumnType("float");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("EmpId")
                        .HasColumnType("int");

                    b.Property<int?>("EmployeesEmpId")
                        .HasColumnType("int");

                    b.Property<string>("Explanation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("InTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("OutTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("TotalHours")
                        .HasColumnType("float");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("WorkingHours")
                        .HasColumnType("float");

                    b.HasKey("AttendenceId");

                    b.HasIndex("EmployeesEmpId");

                    b.ToTable("Attendence");
                });

            modelBuilder.Entity("VPMS_Project.Data.Employees", b =>
                {
                    b.Property<int>("EmpId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("AnnualAllocated")
                        .HasColumnType("int");

                    b.Property<int>("CasualAllocated")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Dob")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmpFName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmpLName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("FromDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("HalfLeaveAllocated")
                        .HasColumnType("int");

                    b.Property<int?>("JobId")
                        .HasColumnType("int");

                    b.Property<int>("JobTitleId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastDayWorked")
                        .HasColumnType("datetime2");

                    b.Property<int>("MedicalAllocated")
                        .HasColumnType("int");

                    b.Property<int>("Mobile")
                        .HasColumnType("int");

                    b.Property<int>("PMId")
                        .HasColumnType("int");

                    b.Property<string>("ProfilePhoto")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ShortLeaveAllocated")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Todate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("WorkSince")
                        .HasColumnType("datetime2");

                    b.HasKey("EmpId");

                    b.HasIndex("JobId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("VPMS_Project.Data.Job", b =>
                {
                    b.Property<int>("JobId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Annual")
                        .HasColumnType("int");

                    b.Property<int>("Casual")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("HalfDays")
                        .HasColumnType("int");

                    b.Property<string>("JobName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Medical")
                        .HasColumnType("int");

                    b.Property<int>("ShortLeaves")
                        .HasColumnType("int");

                    b.HasKey("JobId");

                    b.ToTable("Job");
                });

            modelBuilder.Entity("VPMS_Project.Data.LeaveApply", b =>
                {
                    b.Property<int>("LeaveApplyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("AppliedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ApproverName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EmpId")
                        .HasColumnType("int");

                    b.Property<int?>("EmployeesEmpId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LeaveType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NoOfDays")
                        .HasColumnType("int");

                    b.Property<string>("Reason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RecommendName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Startdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Visible")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LeaveApplyId");

                    b.HasIndex("EmployeesEmpId");

                    b.ToTable("LeaveApply");
                });

            modelBuilder.Entity("VPMS_Project.Data.MarkAttendence", b =>
                {
                    b.Property<int>("MarkAttendenceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("EmpId")
                        .HasColumnType("int");

                    b.Property<int?>("EmployeesEmpId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("InTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("OutTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("TotalHours")
                        .HasColumnType("float");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MarkAttendenceId");

                    b.HasIndex("EmployeesEmpId");

                    b.ToTable("MarkAttendence");
                });

            modelBuilder.Entity("VPMS_Project.Data.StandardWorkHours", b =>
                {
                    b.Property<int>("HourId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("NoOfHours")
                        .HasColumnType("int");

                    b.HasKey("HourId");

                    b.ToTable("StandardWorkHours");
                });

            modelBuilder.Entity("VPMS_Project.Data.TimeTracker", b =>
                {
                    b.Property<int>("TrackId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("BreakEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("BreakStart")
                        .HasColumnType("datetime2");

                    b.Property<double>("BreakingHours")
                        .HasColumnType("float");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("EmpId")
                        .HasColumnType("int");

                    b.Property<int?>("EmployeesEmpId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("InTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("OutTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("TempInTime")
                        .HasColumnType("datetime2");

                    b.Property<double>("TotalHours")
                        .HasColumnType("float");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("WorkingHours")
                        .HasColumnType("float");

                    b.HasKey("TrackId");

                    b.HasIndex("EmployeesEmpId");

                    b.ToTable("TimeTracker");
                });

            modelBuilder.Entity("VPMS_Project.Data.Attendence", b =>
                {
                    b.HasOne("VPMS_Project.Data.Employees", "Employees")
                        .WithMany("Attendence")
                        .HasForeignKey("EmployeesEmpId");
                });

            modelBuilder.Entity("VPMS_Project.Data.Employees", b =>
                {
                    b.HasOne("VPMS_Project.Data.Job", "Job")
                        .WithMany("Employees")
                        .HasForeignKey("JobId");
                });

            modelBuilder.Entity("VPMS_Project.Data.LeaveApply", b =>
                {
                    b.HasOne("VPMS_Project.Data.Employees", "Employees")
                        .WithMany("LeaveApply")
                        .HasForeignKey("EmployeesEmpId");
                });

            modelBuilder.Entity("VPMS_Project.Data.MarkAttendence", b =>
                {
                    b.HasOne("VPMS_Project.Data.Employees", "Employees")
                        .WithMany("MarkAttendence")
                        .HasForeignKey("EmployeesEmpId");
                });

            modelBuilder.Entity("VPMS_Project.Data.TimeTracker", b =>
                {
                    b.HasOne("VPMS_Project.Data.Employees", "Employees")
                        .WithMany("TimeTracker")
                        .HasForeignKey("EmployeesEmpId");
                });
#pragma warning restore 612, 618
        }
    }
}
