
// This is the DBContext file

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

        public virtual DbSet<SocialInsurance> SocialInsurances { get; set; }
        public virtual DbSet<Expertise> Expertises { get; set; }
        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Salary> Salarys { get; set; }
        public virtual DbSet<Month> Months { get; set; }
        public virtual DbSet<SalaryStatistic> SalaryStatistics { get; set; }
        public virtual DbSet<PersonalIncomeTax> PersonalIncomeTaxs { get; set; }
        public virtual DbSet<Qualification> Qualifications { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.\\HOANG;Database=HR_Management;Integrated security=true;Trust Server Certificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<SocialInsurance>(entity =>
            {
                entity.HasKey(e => e.Social_Insurance_ID)
                    .HasName("PK__SocialInsurance__8338BDD3381DBBD6");

                entity.ToTable("social_insurance");

                entity.Property(e => e.Social_Insurance_ID).HasColumnName("social_insurance_id");

                entity.Property(e => e.Notes).HasMaxLength(50);

                entity.Property(e => e.Issue_Date).HasColumnType("datetime");

                entity.Property(e => e.Issue_Place).HasMaxLength(50);

                entity.Property(e => e.Registered_Medical_Facility)
                    .HasMaxLength(50)
                    .HasColumnName("registered_medical_facility");
            });

            modelBuilder.Entity<Expertise>(entity =>
            {
                entity.HasKey(e => e.Expertise_ID)
                    .HasName("PK__Expertise__27258E0F845370EC");

                entity.ToTable("expertise");

                entity.Property(e => e.Expertise_ID).HasColumnName("expertise_id");

                entity.Property(e => e.Notes).HasMaxLength(100);

                entity.Property(e => e.Expertise_Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("expertise_name");
            });

            modelBuilder.Entity<Unit>(entity =>
            {
                entity.HasKey(e => e.Unit_ID)
                    .HasName("PK__Unit__2725865738B0E4C3");

                entity.ToTable("unit");

                entity.Property(e => e.Unit_ID).HasColumnName("unit_id");

                entity.Property(e => e.Notes).HasMaxLength(100);

                entity.Property(e => e.Unit_Name)
                    .HasMaxLength(100)
                    .HasColumnName("unit_name");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.Project_ID)
                    .HasName("PK__Project__2932841538F0G4D3");

                entity.ToTable("project");

                entity.Property(e => e.Project_ID).HasColumnName("project_id");

                entity.Property(e => e.Notes).HasMaxLength(200);

                entity.Property(e => e.Project_Name)
                    .HasMaxLength(100)
                    .HasColumnName("project_name");

                entity.Property(e => e.Start_Date).HasColumnType("datetime");
                entity.Property(e => e.End_Date).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");
            });

            modelBuilder.Entity<Salary>(entity =>
            {
                entity.HasKey(e => e.Salary_ID)
                    .HasName("PK__Salary__6609A48DE7B24381");

                entity.ToTable("salary");

                entity.Property(e => e.Notes).HasMaxLength(100);

                entity.Property(e => e.New_Basic_Salary).HasColumnName("new_basic_salary");

                entity.Property(e => e.Expertise_ID).HasColumnName("expertise_id");

                entity.Property(e => e.Unit_ID).HasColumnName("unit_id");

                entity.Property(e => e.Qualification_ID).HasColumnName("qualification_id");

                entity.Property(e => e.Entry_Date).HasColumnType("datetime");

                entity.Property(e => e.Modify_Date).HasColumnType("datetime");

                entity.HasOne(d => d.ExpertiseIDNavigation)
                    .WithMany(p => p.Salarys)
                    .HasForeignKey(d => d.Expertise_ID)
                    .HasConstraintName("FK_Salary_Expertise");

                entity.HasOne(d => d.UnitIDNavigation)
                    .WithMany(p => p.Salarys)
                    .HasForeignKey(d => d.Unit_ID)
                    .HasConstraintName("FK_Salary_Unit");

                entity.HasOne(d => d.QualificationIDNavigation)
                    .WithMany(p => p.Salarys)
                    .HasForeignKey(d => d.Qualification_ID)
                    .HasConstraintName("FK_Salary_Qualification");
            });

            modelBuilder.Entity<Month>(entity =>
            {
                entity.HasKey(e => e.Month_ID)
                    .HasName("PK__Month__94C86991E5F9E4E7");

                entity.ToTable("month");

                entity.Property(e => e.Notes).HasMaxLength(200);

                entity.Property(e => e.Month_Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<SalaryStatistic>(entity =>
            {
                entity.HasKey(e => e.Salary_Statistic_ID)
                    .HasName("PK__SalaryStatistic__B68A0319767B458C");

                entity.ToTable("salary_statistic");

                entity.Property(e => e.Salary_Statistic_ID).HasColumnName("salary_statistic_id");

                entity.Property(e => e.Notes).HasMaxLength(200);

                entity.Property(e => e.Employee_ID).HasColumnName("employee_id");

                entity.Property(e => e.Create_Date).HasColumnType("datetime");
                entity.Property(e => e.BasicSalary).HasColumnName("basic_salary");
                entity.Property(e => e.TaxToPay).HasColumnName("tax_to_pay");
                entity.Property(e => e.TotalSalary).HasColumnName("total_salary");

                entity.HasOne(d => d.EmployeeIDNavigation)
                    .WithMany(p => p.SalaryStatistics)
                    .HasForeignKey(d => d.Employee_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SalaryStatistic__EmployeeID__4BAC3F29");

                entity.HasOne(d => d.MonthIDNavigation)
                    .WithMany(p => p.SalaryStatistics)
                    .HasForeignKey(d => d.Month_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SalaryStatistic__MonthID__4CA06362");
            });

            modelBuilder.Entity<PersonalIncomeTax>(entity =>
            {
                entity.HasKey(e => e.Tax_ID)
                    .HasName("PK__PersonalIncomeTax__9CC2FDA33CE76C30");

                entity.ToTable("personal_income_tax");

                entity.Property(e => e.Tax_Authority).HasMaxLength(100);

                entity.Property(e => e.Notes).HasMaxLength(50);

                entity.Property(e => e.Registration_Date).HasColumnType("datetime");

                entity.HasOne(d => d.SalaryIDNavigation)
                    .WithMany(p => p.PersonalIncomeTaxs)
                    .HasForeignKey(d => d.Salary_ID)
                    .HasConstraintName("FK__PersonalIncomeTax__SalaryID__30F848ED");
            });

            modelBuilder.Entity<Qualification>(entity =>
            {
                entity.HasKey(e => e.Qualification_ID)
                    .HasName("PK__Qualification__27250069D349E5D2");

                entity.ToTable("Qualification");

                entity.Property(e => e.Qualification_ID).HasColumnName("qualification_id");

                entity.Property(e => e.Notes).HasMaxLength(100);

                entity.Property(e => e.Qualification_Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

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

                entity.Property(e => e.Gender).HasMaxLength(6);

                entity.Property(e => e.Full_Name).HasMaxLength(50);

                entity.Property(e => e.Social_Insurance_ID).HasColumnName("social_insurance_id");

                entity.Property(e => e.Expertise_ID).HasColumnName("expertise_id");

                entity.Property(e => e.Unit_ID).HasColumnName("unit_id");

                entity.Property(e => e.Project_ID).HasColumnName("project_id");

                entity.Property(e => e.Qualification_ID).HasColumnName("qualification_id");

                entity.Property(e => e.Date_Of_Birth).HasColumnType("datetime");

                entity.Property(e => e.Place_Of_Birth)
                    .HasMaxLength(30)
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
                    .HasConstraintName("FK__Employee__UnitID__36B12245");

                entity.HasOne(d => d.ProjectIDNavigation)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.Project_ID)
                    .HasConstraintName("FK__Employee__ProjectID__92B15297");

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
