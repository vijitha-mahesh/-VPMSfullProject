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
    [Migration("20201027053916_addedtwocolumns")]
    partial class addedtwocolumns
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

                    b.Property<DateTime?>("Dob")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmpFName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmpLName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Mobile")
                        .HasColumnType("int");

                    b.Property<string>("Position")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("WorkSince")
                        .HasColumnType("datetime2");

                    b.HasKey("EmpId");

                    b.ToTable("Employees");
                });
#pragma warning restore 612, 618
        }
    }
}
