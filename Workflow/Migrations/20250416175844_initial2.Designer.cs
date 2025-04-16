﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Workflow.Data;

#nullable disable

namespace Workflow.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250416175844_initial2")]
    partial class initial2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Workflow.Models.Application", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ApplicantName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("LoanAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StatusId");

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("Workflow.Models.ApplicationHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ApplicationId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ChangedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ChangedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NewStatusId")
                        .HasColumnType("int");

                    b.Property<int>("PreviousStatusId")
                        .HasColumnType("int");

                    b.Property<string>("Remarks")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.HasIndex("NewStatusId");

                    b.HasIndex("PreviousStatusId");

                    b.ToTable("ApplicationHistories");
                });

            modelBuilder.Entity("Workflow.Models.ApplicationStatusEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ApplicationStatuses");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Waiting for review",
                            Name = "Pending"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Currently being reviewed",
                            Name = "In Process"
                        },
                        new
                        {
                            Id = 3,
                            Description = "Sent for further processing",
                            Name = "Forwarded"
                        },
                        new
                        {
                            Id = 4,
                            Description = "Application approved",
                            Name = "Approved"
                        },
                        new
                        {
                            Id = 5,
                            Description = "Application rejected",
                            Name = "Rejected"
                        },
                        new
                        {
                            Id = 6,
                            Description = "Paused for review",
                            Name = "On Hold"
                        },
                        new
                        {
                            Id = 7,
                            Description = "Process completed successfully",
                            Name = "Completed"
                        },
                        new
                        {
                            Id = 8,
                            Description = "Application canceled",
                            Name = "Canceled"
                        });
                });

            modelBuilder.Entity("Workflow.Models.LeaveRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("EmployeeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HrDecision")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ManagerDecision")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WorkflowId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("LeaveRequests");
                });

            modelBuilder.Entity("Workflow.Models.Application", b =>
                {
                    b.HasOne("Workflow.Models.ApplicationStatusEntity", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Status");
                });

            modelBuilder.Entity("Workflow.Models.ApplicationHistory", b =>
                {
                    b.HasOne("Workflow.Models.Application", null)
                        .WithMany("ApplicationHistories")
                        .HasForeignKey("ApplicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Workflow.Models.ApplicationStatusEntity", "NewStatus")
                        .WithMany()
                        .HasForeignKey("NewStatusId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Workflow.Models.ApplicationStatusEntity", "PreviousStatus")
                        .WithMany()
                        .HasForeignKey("PreviousStatusId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("NewStatus");

                    b.Navigation("PreviousStatus");
                });

            modelBuilder.Entity("Workflow.Models.Application", b =>
                {
                    b.Navigation("ApplicationHistories");
                });
#pragma warning restore 612, 618
        }
    }
}
