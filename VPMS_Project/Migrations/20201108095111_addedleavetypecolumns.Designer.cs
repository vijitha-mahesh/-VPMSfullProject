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
    [Migration("20201108095111_addedleavetypecolumns")]
    partial class addedleavetypecolumns
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

            modelBuilder.Entity("VPMS_Project.Data.Employees", b =>
                {
                    b.HasOne("VPMS_Project.Data.Job", "Job")
                        .WithMany("Employees")
                        .HasForeignKey("JobId");
                });
#pragma warning restore 612, 618
        }
    }
}
