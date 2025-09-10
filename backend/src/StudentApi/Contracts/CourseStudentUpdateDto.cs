
using System.ComponentModel.DataAnnotations;

namespace StudentApi.Contracts;

public class CourseStudentUpdateDto
{
    [Required]
    public int StudentId { get; set; }
}
