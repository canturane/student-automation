using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApi.Contracts;
using StudentApi.Domain;
using StudentApi.Infrastructure;

namespace StudentApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly StudentApi.Infrastructure.Security.IJwtTokenService _jwt;

    public AuthController(AppDbContext db, StudentApi.Infrastructure.Security.IJwtTokenService jwt)
    {
        _db = db;
        _jwt = jwt;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (await _db.Users.AnyAsync(u => u.Email == req.Email))
            return Conflict("Email already exists.");

        if (!Enum.TryParse<UserRole>(req.Role, true, out var role))
            return BadRequest("Invalid role. Use Admin/Teacher/Student.");

        var user = new User
        {
            Email = req.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password),
            Role = role
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        switch (role)
        {
            case UserRole.Student:
                _db.Students.Add(new Student
                {
                    UserId = user.Id,
                    Name = req.Name ?? "Student",
                    Surname = req.Surname ?? "User",
                    Number = string.IsNullOrWhiteSpace(req.NumberOrTitle)
                        ? $"S{user.Id:00000}"
                        : req.NumberOrTitle!
                });
                break;

            case UserRole.Teacher:
                _db.Teachers.Add(new Teacher
                {
                    UserId = user.Id,
                    Name = req.Name ?? "Teacher",
                    Surname = req.Surname ?? "User",
                    Title = req.NumberOrTitle
                });
                break;

            case UserRole.Admin:
                // ekstra profile gerek yok
                break;
        }

        await _db.SaveChangesAsync();

        var token = _jwt.CreateToken(user.Id, user.Email, user.Role);
        return Ok(new AuthResponse(token, user.Role.ToString().ToLowerInvariant()));
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == req.Email);
        if (user == null)
            return Unauthorized("Invalid credentials.");

        if (!BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
            return Unauthorized("Invalid credentials.");

        var token = _jwt.CreateToken(user.Id, user.Email, user.Role);
        return Ok(new AuthResponse(token, user.Role.ToString().ToLowerInvariant()));
    }
}
