using Microsoft.EntityFrameworkCore;
using StudentApi.Domain;

namespace StudentApi.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<Grade> Grades => Set<Grade>();
    public DbSet<Attendance> Attendances => Set<Attendance>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<User>().HasIndex(x => x.Email).IsUnique();
        mb.Entity<Student>().HasIndex(x => x.Number).IsUnique();

        mb.Entity<User>()
          .HasOne(u => u.Student)
          .WithOne(s => s.User)
          .HasForeignKey<Student>(s => s.UserId)
          .OnDelete(DeleteBehavior.Cascade);

        mb.Entity<User>()
          .HasOne(u => u.Teacher)
          .WithOne(t => t.User)
          .HasForeignKey<Teacher>(t => t.UserId)
          .OnDelete(DeleteBehavior.Cascade);

        mb.Entity<Course>()
          .HasOne(c => c.Teacher)
          .WithMany(t => t.Courses)
          .HasForeignKey(c => c.TeacherId)
          .OnDelete(DeleteBehavior.Restrict);

        mb.Entity<Enrollment>()
          .HasIndex(e => new { e.CourseId, e.StudentId })
          .IsUnique();

        mb.Entity<Enrollment>()
          .HasOne(e => e.Course)
          .WithMany(c => c.Enrollments)
          .HasForeignKey(e => e.CourseId)
          .OnDelete(DeleteBehavior.Cascade);

        mb.Entity<Enrollment>()
          .HasOne(e => e.Student)
          .WithMany(s => s.Enrollments)
          .HasForeignKey(e => e.StudentId)
          .OnDelete(DeleteBehavior.Cascade);

        // Attendance.Date -> timestamp without time zone
        mb.Entity<Attendance>()
          .Property(a => a.Date)
          .HasColumnType("timestamp without time zone");
    }
}
