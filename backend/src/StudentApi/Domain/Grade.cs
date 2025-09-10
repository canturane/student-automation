namespace StudentApi.Domain;

public class Grade
{
    public int Id { get; set; }
    public int EnrollmentId { get; set; }
    public decimal Score { get; set; }  // 0-100
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Enrollment Enrollment { get; set; } = default!;
}
