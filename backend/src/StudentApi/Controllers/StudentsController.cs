// Controllers/StudentsController.cs
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
public class StudentsController(AppDbContext db) : ControllerBase
{
    // --- Tüm öğrencileri listele (Admin & Teacher) ---
    [Authorize(Roles = "Admin,Teacher")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var students = await db.Students
            .Include(s => s.User)
            .Select(s => new StudentDto
            {
                Id = s.Id,
                UserId = s.UserId,
                Name = s.Name,
                Surname = s.Surname,
                Number = s.Number
            })
            .ToListAsync();

        return Ok(students);
    }



[Authorize(Roles = "Student")]
[HttpGet("me")]
public async Task<IActionResult> GetOwnProfile()
{
    var userId = User.GetUserId();
    if (userId == null)
        return Unauthorized("Invalid or missing token claim.");

    var student = await db.Students
        .Include(s => s.User)
        .FirstOrDefaultAsync(s => s.UserId == userId);

    if (student == null)
        return NotFound("Student not found for current user.");

    return Ok(new StudentDto
    {
        Id = student.Id,
        UserId = student.UserId,
        Name = student.Name,
        Surname = student.Surname,
        Number = student.Number
    });
}




    // --- Yeni öğrenci ekle (User + Student) ---
    [Authorize(Roles = "Admin,Teacher")]
    [HttpPost]
    public async Task<IActionResult> Create(StudentWithUserCreateDto dto)
    {
        // Email daha önce alınmış mı?
        if (await db.Users.AnyAsync(u => u.Email == dto.Email))
            return Conflict("This email is already registered.");

        // User oluştur
        var user = new User
        {
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = UserRole.Student
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();

        // Student oluştur
        var student = new Student
        {
            UserId = user.Id,
            Name = dto.Name,
            Surname = dto.Surname,
            Number = dto.Number
        };

        db.Students.Add(student);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOwnProfile), new { }, null);
    }

    // --- Öğrenci bilgilerini güncelle (sadece öğrenci tablosu) ---
    [Authorize(Roles = "Admin,Teacher")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, StudentUpdateDto dto)
    {
        var student = await db.Students.FindAsync(id);
        if (student == null) return NotFound();

        student.Name = dto.Name;
        student.Surname = dto.Surname;
        student.Number = dto.Number;

        await db.SaveChangesAsync();
        return NoContent();
    }
}
