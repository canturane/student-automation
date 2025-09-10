
using System.ComponentModel.DataAnnotations;

namespace StudentApi.Contracts;

public class GradeCreateDto
{
    [Required]
    public int EnrollmentId { get; set; }

    [Range(0, 100)]
    public decimal Score { get; set; }
}
