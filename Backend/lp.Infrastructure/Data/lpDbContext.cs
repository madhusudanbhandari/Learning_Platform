using lp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace lp.Infrastructure;

public class LpDbConext : DbContext
{
    public LpDbConext(DbContextOptions<LpDbConext> options) : base(options)
    {
        
    }

    public DbSet<User> Users=>Set<User>();

    public DbSet<Course> Courses=>Set<Course>();
    public DbSet<Enrollment> Enrollments=>Set<Enrollment>();
    public DbSet<Payment> Payments=>Set<Payment>();

    public DbSet<Lesson> Lessons=>Set<Lesson>();



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Enrollment>()
                    .HasOne(e=>e.Student)
                    .WithMany(u=>u.Enrollments)
                    .HasForeignKey(e=>e.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Enrollment>()
                    .HasOne(e=>e.Course)
                    .WithMany(c=>c.Enrollments)
                    .HasForeignKey(e=>e.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Enrollment>()
                    .HasMany(e=>e.Payments)
                    .WithOne(p=>p.Enrollment)
                    .HasForeignKey(p=>p.EnrollId)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Payment>()
                    .HasIndex(p=>p.IdempotencyKey)
                    .IsUnique();

        modelBuilder.Entity<Lesson>()
                    .HasOne(l=>l.Course)
                    .WithMany(c=>c.Lessons)
                    .HasForeignKey(l=>l.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);
        

    }


    

    
}