
namespace StudentApi.Contracts;

public class GradeDto
{
    public int Id { get; set; }
    public int EnrollmentId { get; set; }
    public decimal? Score { get; set; }
    public DateTime? CreatedAt { get; set; }

    public string CourseName { get; set; } = null!;
    public string CourseStatus { get; set; } = null!;
    public string TeacherName { get; set; } = null!;
}
