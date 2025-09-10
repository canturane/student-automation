
using System.ComponentModel.DataAnnotations;

namespace StudentApi.Contracts;

public class TeacherWithUserCreateDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = null!;

    [Required, MinLength(6)]
    public string Password { get; set; } = null!;

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Surname { get; set; } = null!;

    public string? Title { get; set; }
}
