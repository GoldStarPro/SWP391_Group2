using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace HR_Management.Models
{
    public partial class HRManagementContext : DbContext
    {
        public HRManagementContext()
        {
        }

        public HRManagementContext(DbContextOptions<HRManagementContext> options)
        : base(options)
        {
        }

        public virtual DbSet<Employee> employees { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.\\HOANG;Database=HR_Management;Integrated security=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Employee_ID)
                    .HasName("PK__Employee__2725D70A90B662AA");

                entity.ToTable("employee");

                entity.Property(e => e.Employee_ID).HasColumnName("employee_id");

                entity.Property(e => e.ID_Card_Number)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("id_card_number")
                    .IsFixedLength(true);

                entity.Property(e => e.Ethnicity).HasMaxLength(30);

                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Notes).HasMaxLength(100);

                entity.Property(e => e.Gender).HasMaxLength(3);

                entity.Property(e => e.Full_Name).HasMaxLength(30);

                entity.Property(e => e.Social_Insurance_ID).HasColumnName("social_insurance_id");

                entity.Property(e => e.Expertise_ID).HasColumnName("expertise_id");

                entity.Property(e => e.Unit_ID).HasColumnName("unit_id");

                entity.Property(e => e.Qualification_ID).HasColumnName("qualification_id");

                entity.Property(e => e.Date_Of_Birth).HasColumnType("datetime");

                entity.Property(e => e.Place_Of_Birth)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.Password).HasMaxLength(30);

                entity.Property(e => e.Nationality).HasMaxLength(30);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("phone_number")
                    .IsFixedLength(true);

                entity.Property(e => e.Religion).HasMaxLength(30);

                entity.HasOne(d => d.SocialInsuranceIDNavigation)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.Social_Insurance_ID)
                    .HasConstraintName("FK__Employee__SocialInsuranceID__34C8D9D1");

                entity.HasOne(d => d.ExpertiseIDNavigation)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.Expertise_ID)
                    .HasConstraintName("FK__Employee__ExpertiseID__38996AB5");

                entity.HasOne(d => d.UnitIDNavigation)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.Unit_ID)
                    .HasConstraintName("FK__Employee__UnitID__36B12243");

                entity.HasOne(d => d.SalaryIDNavigation)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.Salary_ID)
                    .HasConstraintName("FK__Employee__SalaryID__35BCFE0A");

                entity.HasOne(d => d.QualificationIDNavigation)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.Qualification_ID)
                    .HasConstraintName("FK__Employee__QualificationID__33D4B598");

                entity.HasOne(d => d.TaxIDNavigation)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.Tax_ID)
                    .HasConstraintName("FK__Employee__TaxID__37A5467C");

            });

            OnModelCreatingPartial(modelBuilder);
    }
           
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
