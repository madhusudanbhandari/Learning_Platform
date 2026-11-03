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
                    .HasIndex(e=>new {e.CourseId, e.StudentId});

        modelBuilder.Entity<User>()
                    .HasIndex(u=>u.Id);
        
        modelBuilder.Entity<User>() 
                    .HasIndex(u=>u.Email)
                    .IsUnique();
        
        modelBuilder.Entity<Course>()
                    .HasIndex(c=>c.Id)
                    .IsUnique();

        modelBuilder.Entity<Enrollment>()
                    .HasKey(e=>e.Id);
        

    }


    

    
}