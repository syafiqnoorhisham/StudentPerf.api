using Microsoft.EntityFrameworkCore;
using StudentPerf.api.Models;

namespace StudentPerf.api.Data
{
    public class StudentPerfContext : DbContext
    {
        public StudentPerfContext(DbContextOptions<StudentPerfContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Subject> Subjects { get; set; } = null!;
        public DbSet<Performance> Performances { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Student entity
            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Students");
                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.Name).HasColumnName("Name").IsRequired();
                entity.Property(e => e.Email).HasColumnName("Email").IsRequired();
            });

            // Configure Course entity
            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Courses");
                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.CourseCode).HasColumnName("CourseCode").IsRequired();
                entity.Property(e => e.CourseName).HasColumnName("CourseName").IsRequired();
            });

            // Configure Subject entity
            modelBuilder.Entity<Subject>(entity =>
            {
                entity.ToTable("Subjects");
                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.CourseId).HasColumnName("CourseId");
                entity.Property(e => e.SubjectName).HasColumnName("SubjectName").IsRequired();
                
                entity.HasOne(s => s.Course)
                    .WithMany(c => c.Subjects)
                    .HasForeignKey(s => s.CourseId);
            });

            // Configure Performance entity
            modelBuilder.Entity<Performance>(entity =>
            {
                entity.ToTable("Performances");
                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.StudentId).HasColumnName("StudentId");
                entity.Property(e => e.CourseId).HasColumnName("CourseId");
                entity.Property(e => e.SubjectId).HasColumnName("SubjectId");
                entity.Property(e => e.Grade).HasColumnName("Grade").IsRequired();
                entity.Property(e => e.SubmittedOn).HasColumnName("SubmittedOn");
                entity.Property(e => e.ModifiedOn).HasColumnName("ModifiedOn");

                entity.HasOne(p => p.Student)
                    .WithMany(s => s.Performances)
                    .HasForeignKey(p => p.StudentId);

                entity.HasOne(p => p.Course)
                    .WithMany(c => c.Performances)
                    .HasForeignKey(p => p.CourseId);

                entity.HasOne(p => p.Subject)
                    .WithMany(s => s.Performances)
                    .HasForeignKey(p => p.SubjectId);
            });
        }
    }
}
