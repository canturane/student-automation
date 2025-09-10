// Controllers/TeachersController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApi.Contracts;
using StudentApi.Domain;
using StudentApi.Infrastructure;

namespace StudentApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeachersController(AppDbContext db) : ControllerBase
{
    // --- Tüm öğretmenleri listele (Admin) ---
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var teachers = await db.Teachers
            .Include(t => t.User)
            .Select(t => new TeacherDto
            {
                Id = t.Id,
                UserId = t.UserId,
                Name = t.Name,
                Surname = t.Surname,
                Title = t.Title
            })
            .ToListAsync();

        return Ok(teachers);
    }

    // --- Yeni öğretmen ekle (User + Teacher) ---
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(TeacherWithUserCreateDto dto)
    {
        // Email var mı kontrolü
        if (await db.Users.AnyAsync(u => u.Email == dto.Email))
            return Conflict("This email is already registered.");

        // User oluştur
        var user = new User
        {
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = UserRole.Teacher
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();

        // Teacher oluştur
        var teacher = new Teacher
        {
            UserId = user.Id,
            Name = dto.Name,
            Surname = dto.Surname,
            Title = dto.Title
        };

        db.Teachers.Add(teacher);
        await db.SaveChangesAsync();

        return StatusCode(201);
    }

    // --- Öğretmen güncelle ---
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, TeacherUpdateDto dto)
    {
        var teacher = await db.Teachers.FindAsync(id);
        if (teacher == null) return NotFound("Teacher not found.");

        teacher.Name = dto.Name;
        teacher.Surname = dto.Surname;
        teacher.Title = dto.Title;

        await db.SaveChangesAsync();
        return NoContent();
    }
}
