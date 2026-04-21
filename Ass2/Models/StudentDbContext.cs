using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Ass2.Models;

public partial class StudentDbContext : DbContext
{
    public StudentDbContext()
    {
    }

    public StudentDbContext(DbContextOptions<StudentDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentMark> StudentMarks { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=StudentDB;UId=sa;pwd=123456789;TrustServerCertificate=True");
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("Student");

            entity.Property(e => e.FullName).HasMaxLength(200);
        });

        modelBuilder.Entity<StudentMark>(entity =>
        {
            entity.HasKey(e => new { e.StudentId, e.SubjectId });

            entity.ToTable("StudentMark");

            entity.HasIndex(e => e.SubjectId, "IX_StudentMark_SubjectId");

            entity.Property(e => e.GradeDate).HasDefaultValueSql("(CONVERT([date],getdate()))");
            entity.Property(e => e.Mark).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentMarks)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_StudentMark_Student");

            entity.HasOne(d => d.Subject).WithMany(p => p.StudentMarks)
                .HasForeignKey(d => d.SubjectId)
                .HasConstraintName("FK_StudentMark_Subject");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.ToTable("Subject");

            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Title).HasMaxLength(200);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
