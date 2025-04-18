﻿using Microsoft.EntityFrameworkCore;
using Workflow.Models;

namespace Workflow.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Application> Applications { get; set; }
        public DbSet<ApplicationHistory> ApplicationHistories { get; set; }
        public DbSet<ApplicationStatusEntity> ApplicationStatuses { get; set; }

        public DbSet<Status> Statuses { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<WorkflowInstanceInfo> WorkflowInstanceInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ApplicationStatusEntity (Status Table) Seed Data
            modelBuilder.Entity<ApplicationStatusEntity>().HasData(
                new ApplicationStatusEntity { Id = (int)ApplicationStatus.Pending, Name = "Pending", Description = "Waiting for review" },
                new ApplicationStatusEntity { Id = (int)ApplicationStatus.InProcess, Name = "In Process", Description = "Currently being reviewed" },
                new ApplicationStatusEntity { Id = (int)ApplicationStatus.Forwarded, Name = "Forwarded", Description = "Sent for further processing" },
                new ApplicationStatusEntity { Id = (int)ApplicationStatus.Approved, Name = "Approved", Description = "Application approved" },
                new ApplicationStatusEntity { Id = (int)ApplicationStatus.Rejected, Name = "Rejected", Description = "Application rejected" },
                new ApplicationStatusEntity { Id = (int)ApplicationStatus.OnHold, Name = "On Hold", Description = "Paused for review" },
                new ApplicationStatusEntity { Id = (int)ApplicationStatus.Completed, Name = "Completed", Description = "Process completed successfully" },
                new ApplicationStatusEntity { Id = (int)ApplicationStatus.Canceled, Name = "Canceled", Description = "Application canceled" }
            );

            // Define Foreign Key Relationships for ApplicationHistory
            modelBuilder.Entity<ApplicationHistory>()
                .HasOne(h => h.PreviousStatus)
                .WithMany()
                .HasForeignKey(h => h.PreviousStatusId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading delete

            modelBuilder.Entity<ApplicationHistory>()
                .HasOne(h => h.NewStatus)
                .WithMany()
                .HasForeignKey(h => h.NewStatusId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading delete

            modelBuilder.Entity<LeaveRequest>()
            .HasOne(lr => lr.WorkflowInstanceInfo)
            .WithMany()  // Assuming WorkflowInstanceInfo does not need a collection of LeaveRequests
            .HasForeignKey(lr => lr.WorkflowInstanceInfoId);

            modelBuilder.Entity<LeaveRequest>()
                .HasOne(lr => lr.ManagerDecision)
                .WithMany(s => s.ManagerLeaveRequests)
                .HasForeignKey(lr => lr.ManagerDecisionId);

            modelBuilder.Entity<LeaveRequest>()
                .HasOne(lr => lr.HrDecision)
                .WithMany(s => s.HrLeaveRequests)
                .HasForeignKey(lr => lr.HrDecisionId);

            // Dummy seed data for Status table
            modelBuilder.Entity<Status>().HasData(
                new Status { Id = 1, Name = "Pending" },
                new Status { Id = 2, Name = "Approved" },
                new Status { Id = 3, Name = "Rejected" }
            );
        }


    }

}
