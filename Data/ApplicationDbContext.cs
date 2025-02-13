using Employee_Management_and_Vacation_System.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;

namespace EmployeeManagementSystem
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<VacationType> VacationTypes { get; set; }
        public DbSet<RequestState> RequestStates { get; set; }
        public DbSet<VacationRequest> VacationRequests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "Server=DESKTOP-UN22KF2\\SQLEXPRESS;Database=EmployeeManagementDB;Trusted_Connection=True;TrustServerCertificate=True;";
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Employee self-referencing relationship
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.ReportedTo)
                .WithMany(e => e.Subordinates)
                .HasForeignKey(e => e.ReportedToEmployeeNumber)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure VacationRequest relationships
            modelBuilder.Entity<VacationRequest>()
                .HasOne(vr => vr.ApprovedBy)  // Fixed: use vr instead of e
                .WithMany(e => e.ApprovedRequests)
                .HasForeignKey(vr => vr.ApprovedByEmployeeNumber)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VacationRequest>()
                .HasOne(vr => vr.DeclinedBy)  // Fixed: use vr instead of e
                .WithMany(e => e.DeclinedRequests)
                .HasForeignKey(vr => vr.DeclinedByEmployeeNumber)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}