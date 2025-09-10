
using System.ComponentModel.DataAnnotations;

namespace StudentApi.Contracts;

public class CourseCreateDto
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public int TeacherId { get; set; }  
}
