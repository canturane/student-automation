
using System.ComponentModel.DataAnnotations;

namespace StudentApi.Contracts;

public class StudentUpdateDto
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Surname { get; set; } = null!;

    [Required]
    public string Number { get; set; } = null!;
}
