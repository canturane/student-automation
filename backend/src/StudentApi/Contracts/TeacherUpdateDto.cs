
using System.ComponentModel.DataAnnotations;

namespace StudentApi.Contracts;

public class TeacherUpdateDto
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Surname { get; set; } = null!;

    public string? Title { get; set; }
}
