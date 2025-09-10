using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApi.Contracts;
using StudentApi.Domain;
using StudentApi.Infrastructure;
using StudentApi.Extensions;

namespace StudentApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController(AppDbContext db) : ControllerBase
{
    // --- Ders oluştur (Admin) ---
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CourseCreateDto dto)
    {
        var teacher = await db.Teachers.FindAsync(dto.TeacherId);
        if (teacher == null)
            return BadRequest("Invalid teacher ID.");

        var course = new Course
        {
            Name = dto.Name,
            TeacherId = dto.TeacherId,
            Status = CourseStatus.Planned
        };

        db.Courses.Add(course);
        await db.SaveChangesAsync();

        return StatusCode(201);
    }

    // --- Öğretmen kendi derslerini görebilir ---
    [Authorize(Roles = "Teacher")]
    [HttpGet("mine")]
    public async Task<IActionResult> GetMyCourses()
    {
        var userId = User.GetUserId();
        if (userId == null) return Unauthorized();

        var teacher = await db.Teachers.FirstOrDefaultAsync(t => t.UserId == userId);
        if (teacher == null) return NotFound("Teacher profile not found.");

        var courses = await db.Courses
            .Where(c => c.TeacherId == teacher.Id)
            .Select(c => new CourseDto
            {
                Id = c.Id,
                Name = c.Name,
                Status = c.Status.ToString(),
                TeacherId = c.TeacherId,
                TeacherName = c.Teacher.Name + " " + c.Teacher.Surname
            })
            .ToListAsync();

        return Ok(courses);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var courses = await db.Courses
            .Include(c => c.Teacher)
            .Select(c => new CourseDto
            {
                Id = c.Id,
                Name = c.Name,
                Status = c.Status.ToString(),
                TeacherId = c.TeacherId,
                TeacherName = c.Teacher.Name + " " + c.Teacher.Surname
            })
            .ToListAsync();

        return Ok(courses);
    }

    // --- Öğretmen ders durumunu güncelleyebilir ---
    [Authorize(Roles = "Teacher")]
    [HttpPut("{courseId}/status")]
    public async Task<IActionResult> UpdateStatus(int courseId, CourseStatusUpdateDto dto)
    {
        var userId = User.GetUserId();
        if (userId == null) return Unauthorized();

        var teacher = await db.Teachers.FirstOrDefaultAsync(t => t.UserId == userId);
        if (teacher == null) return NotFound();

        var course = await db.Courses.FirstOrDefaultAsync(c => c.Id == courseId && c.TeacherId == teacher.Id);
        if (course == null) return NotFound("Course not found or access denied.");

        course.Status = dto.Status;
        await db.SaveChangesAsync();

        return NoContent();
    }

    // --- Öğretmen derse öğrenci ekler ---
    [Authorize(Roles = "Teacher")]
    [HttpPost("{courseId}/students")]
    public async Task<IActionResult> AddStudentToCourse(int courseId, CourseStudentUpdateDto dto)
    {
        var userId = User.GetUserId();
        var teacher = await db.Teachers.FirstOrDefaultAsync(t => t.UserId == userId);
        if (teacher == null) return NotFound();

        var course = await db.Courses.FirstOrDefaultAsync(c => c.Id == courseId && c.TeacherId == teacher.Id);
        if (course == null) return NotFound("Course not found or access denied.");

        // Zaten kayıtlı mı?
        var exists = await db.Enrollments.AnyAsync(e => e.CourseId == courseId && e.StudentId == dto.StudentId);
        if (exists) return Conflict("Student is already enrolled.");

        db.Enrollments.Add(new Enrollment
        {
            CourseId = courseId,
            StudentId = dto.StudentId
        });

        await db.SaveChangesAsync();
        return StatusCode(201);
    }

    // --- Öğretmen dersten öğrenci siler ---
    [Authorize(Roles = "Teacher")]
    [HttpDelete("{courseId}/students/{studentId}")]
    public async Task<IActionResult> RemoveStudentFromCourse(int courseId, int studentId)
    {
        var userId = User.GetUserId();
        var teacher = await db.Teachers.FirstOrDefaultAsync(t => t.UserId == userId);
        if (teacher == null) return NotFound();

        var course = await db.Courses.FirstOrDefaultAsync(c => c.Id == courseId && c.TeacherId == teacher.Id);
        if (course == null) return NotFound("Course not found or access denied.");

        var enrollment = await db.Enrollments
            .FirstOrDefaultAsync(e => e.CourseId == courseId && e.StudentId == studentId);

        if (enrollment == null)
            return NotFound("Student not enrolled in this course.");

        db.Enrollments.Remove(enrollment);
        await db.SaveChangesAsync();

        return NoContent();
    }

    [Authorize(Roles = "Teacher,Admin")]
    [HttpGet("{courseId}/students")]
    public async Task<IActionResult> GetCourseStudents(int courseId)
    {
        var course = await db.Courses
            .Include(c => c.Enrollments)
                .ThenInclude(e => e.Student)
            .Include(c => c.Enrollments)
                .ThenInclude(e => e.Grades)   
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null) return NotFound("Course not found.");

        var students = course.Enrollments.Select(e => new EnrolledStudentDto
        {
            EnrollmentId = e.Id,
            StudentId = e.Student.Id,
            Name = e.Student.Name,
            Surname = e.Student.Surname,
            Number = e.Student.Number,
            Score = e.Grades.FirstOrDefault() != null ? e.Grades.First().Score : null
        }).ToList();

        return Ok(students);
    }

    [Authorize(Roles = "Teacher")]
    [HttpGet("course/{courseId}/grades")]
    public async Task<IActionResult> GetGradesForCourse(int courseId)
    {
        var userId = User.GetUserId();
        var teacher = await db.Teachers.FirstOrDefaultAsync(t => t.UserId == userId);
        if (teacher == null) return Unauthorized();

        var grades = await db.Grades
            .Include(g => g.Enrollment)
            .Where(g => g.Enrollment.CourseId == courseId && g.Enrollment.Course.TeacherId == teacher.Id)
            .Select(g => new GradeDto
            {
                Id = g.Id,
                EnrollmentId = g.EnrollmentId,
                Score = g.Score,
                CreatedAt = g.CreatedAt
            })
            .ToListAsync();

        return Ok(grades);
    }
}
