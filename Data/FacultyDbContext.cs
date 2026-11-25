using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SPC_Faculty_Pages.Models.Database;

namespace SPC_Faculty_Pages.Data;

public partial class FacultyDbContext : DbContext
{
    public FacultyDbContext(DbContextOptions<FacultyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Faculty> Faculties { get; set; }

    public virtual DbSet<Facultycourse> Facultycourses { get; set; }

    public virtual DbSet<Officehour> Officehours { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PRIMARY");

            entity.ToTable("course");

            entity.HasIndex(e => e.CourseCode, "AkCourseCode").IsUnique();

            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.CourseCode).HasMaxLength(20);
            entity.Property(e => e.CourseTitle).HasMaxLength(150);
            entity.Property(e => e.SourceSystem)
                .HasMaxLength(50)
                .HasDefaultValueSql("'PeopleSoft'");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PRIMARY");

            entity.ToTable("department");

            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.DepartmentName).HasMaxLength(100);
        });

        modelBuilder.Entity<Faculty>(entity =>
        {
            entity.HasKey(e => e.FacultyId).HasName("PRIMARY");

            entity.ToTable("faculty");

            entity.HasIndex(e => e.DepartmentId, "AkDepartmentID");

            entity.HasIndex(e => e.Email, "Email").IsUnique();

            entity.Property(e => e.FacultyId).HasColumnName("FacultyID");
            entity.Property(e => e.Biography).HasColumnType("text");
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.OfficeLocation).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(25);

            entity.HasOne(d => d.Department).WithMany(p => p.Faculties)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("faculty_ibfk_1");
        });

        modelBuilder.Entity<Facultycourse>(entity =>
        {
            entity.HasKey(e => new { e.FacultyId, e.CourseId }).HasName("PRIMARY");

            entity.ToTable("facultycourse");

            entity.HasIndex(e => e.CourseId, "CourseID");

            entity.Property(e => e.FacultyId).HasColumnName("FacultyID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.Term).HasMaxLength(30);

            entity.HasOne(d => d.Course).WithMany(p => p.Facultycourses)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("facultycourse_ibfk_2");

            entity.HasOne(d => d.Faculty).WithMany(p => p.Facultycourses)
                .HasForeignKey(d => d.FacultyId)
                .HasConstraintName("facultycourse_ibfk_1");
        });

        modelBuilder.Entity<Officehour>(entity =>
        {
            entity.HasKey(e => e.OfficeHourId).HasName("PRIMARY");

            entity.ToTable("officehour");

            entity.HasIndex(e => e.FacultyId, "FacultyID");

            entity.Property(e => e.OfficeHourId).HasColumnName("OfficeHourID");
            entity.Property(e => e.DayOfWeek).HasMaxLength(15);
            entity.Property(e => e.EndTime).HasColumnType("time");
            entity.Property(e => e.FacultyId).HasColumnName("FacultyID");
            entity.Property(e => e.Location).HasMaxLength(100);
            entity.Property(e => e.StartTime).HasColumnType("time");

            entity.HasOne(d => d.Faculty).WithMany(p => p.Officehours)
                .HasForeignKey(d => d.FacultyId)
                .HasConstraintName("officehour_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
