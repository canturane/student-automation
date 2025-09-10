
using System.ComponentModel.DataAnnotations;
using StudentApi.Domain;

namespace StudentApi.Contracts;

public class CourseStatusUpdateDto
{
    [Required]
    public CourseStatus Status { get; set; }
}
