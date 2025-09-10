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
public class GradesController(AppDbContext db) : ControllerBase
{
    // --- Öğretmen: Not ekle ---
    [Authorize(Roles = "Teacher")]
    [HttpPost]
    public async Task<IActionResult> AddGrade(GradeCreateDto dto)
    {
        var userId = User.GetUserId();
        var teacher = await db.Teachers.FirstOrDefaultAsync(t => t.UserId == userId);
        if (teacher == null) return Unauthorized();

        // Enrollment öğretmene ait derste mi?
        var enrollment = await db.Enrollments
            .Include(e => e.Course)
            .FirstOrDefaultAsync(e => e.Id == dto.EnrollmentId && e.Course.TeacherId == teacher.Id);

        if (enrollment == null) 
            return BadRequest("Invalid enrollment or not your course.");

        // Duplicate not kontrolü (opsiyonel: bir enrollment için tek not tutulacaksa)
        var exists = await db.Grades.AnyAsync(g => g.EnrollmentId == dto.EnrollmentId);
        if (exists)
            return Conflict("Grade already exists for this enrollment.");

        var grade = new Grade
        {
            EnrollmentId = dto.EnrollmentId,
            Score = dto.Score,
            CreatedAt = DateTime.UtcNow
        };

        db.Grades.Add(grade);
        await db.SaveChangesAsync();

        // DTO olarak geri döndür
        var response = new GradeDto
        {
            Id = grade.Id,
            EnrollmentId = grade.EnrollmentId,
            Score = grade.Score,
            CreatedAt = grade.CreatedAt
        };

        return CreatedAtAction(nameof(GetMyGrades), new { id = grade.Id }, response);
    }

 [Authorize(Roles = "Student")]
[HttpGet("mine")]
public async Task<IActionResult> GetMyGrades()
{
    var userId = User.GetUserId();
    var student = await db.Students.FirstOrDefaultAsync(s => s.UserId == userId);
    if (student == null) return Unauthorized();

    var enrollments = await db.Enrollments
        .Include(e => e.Course)
            .ThenInclude(c => c.Teacher)
        .Include(e => e.Grades) // Notları da getir
        .Where(e => e.StudentId == student.Id)
        .ToListAsync();

    var results = enrollments.Select(e => new GradeDto
    {
        EnrollmentId = e.Id,
        CourseName = e.Course.Name,
        CourseStatus = e.Course.Status.ToString(),
        TeacherName = e.Course.Teacher.Name + " " + e.Course.Teacher.Surname,
        // Not varsa al, yoksa null
        Score = e.Grades.FirstOrDefault() != null ? e.Grades.First().Score : (decimal?)null,
        CreatedAt = e.Grades.FirstOrDefault() != null ? e.Grades.First().CreatedAt : DateTime.MinValue
    }).ToList();

    return Ok(results);
}
}
