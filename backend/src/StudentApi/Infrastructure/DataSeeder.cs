using Microsoft.EntityFrameworkCore;
using StudentApi.Domain;

namespace StudentApi.Infrastructure;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        await db.Database.MigrateAsync();

        // --- Admin User ---
        var admin = await db.Users.FirstOrDefaultAsync(u => u.Email == "admin@test.com");
        if (admin == null)
        {
            admin = new User
            {
                Email = "admin@test.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                Role = UserRole.Admin
            };
            db.Users.Add(admin);
            await db.SaveChangesAsync();
        }

        // --- Teacher User + Profile ---
        var teacherUser = await db.Users.FirstOrDefaultAsync(u => u.Email == "teacher@test.com");
        Teacher? teacher = null;
        if (teacherUser == null)
        {
            teacherUser = new User
            {
                Email = "teacher@test.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Teacher123!"),
                Role = UserRole.Teacher
            };
            db.Users.Add(teacherUser);
            await db.SaveChangesAsync();
        }

        teacher = await db.Teachers.FirstOrDefaultAsync(t => t.UserId == teacherUser.Id);
        if (teacher == null)
        {
            teacher = new Teacher
            {
                UserId = teacherUser.Id,
                Name = "Ahmet",
                Surname = "Yılmaz",
                Title = "Prof. Dr."
            };
            db.Teachers.Add(teacher);
            await db.SaveChangesAsync();
        }

        // --- Student User + Profile ---
        var studentUser = await db.Users.FirstOrDefaultAsync(u => u.Email == "student@test.com");
        Student? student = null;
        if (studentUser == null)
        {
            studentUser = new User
            {
                Email = "student@test.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student123!"),
                Role = UserRole.Student
            };
            db.Users.Add(studentUser);
            await db.SaveChangesAsync();
        }

        student = await db.Students.FirstOrDefaultAsync(s => s.UserId == studentUser.Id);
        if (student == null)
        {
            student = new Student
            {
                UserId = studentUser.Id,
                Name = "Ayşe",
                Surname = "Demir",
                Number = $"S{studentUser.Id:00000}"
            };
            db.Students.Add(student);
            await db.SaveChangesAsync();
        }

        // --- Course ---
        var course = await db.Courses.FirstOrDefaultAsync(c => c.Name == "Yazılım Mühendisliğine Giriş");
        if (course == null && teacher != null)
        {
            course = new Course
            {
                Name = "Yazılım Mühendisliğine Giriş",
                Status = CourseStatus.Started,
                TeacherId = teacher.Id
            };
            db.Courses.Add(course);
            await db.SaveChangesAsync();
        }

        // --- Enrollment ---
        if (course != null && student != null)
        {
            var enrollment = await db.Enrollments.FirstOrDefaultAsync(e =>
                e.CourseId == course.Id && e.StudentId == student.Id);
            if (enrollment == null)
            {
                enrollment = new Enrollment
                {
                    CourseId = course.Id,
                    StudentId = student.Id
                };
                db.Enrollments.Add(enrollment);
                await db.SaveChangesAsync();

                // --- Grade ---
                var grade = new Grade
                {
                    EnrollmentId = enrollment.Id,
                    Score = 85,
                    CreatedAt = DateTime.UtcNow
                };
                db.Grades.Add(grade);

                // --- Attendance ---
                var attendance = new Attendance
                {
                    EnrollmentId = enrollment.Id,
                    Date = DateTime.UtcNow.Date,
                    IsPresent = true
                };
                db.Attendances.Add(attendance);

                await db.SaveChangesAsync();
            }
        }
    }
}
