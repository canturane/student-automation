using StudentApi.Domain;

public class Attendance
{
    public int Id { get; set; }

    public int EnrollmentId { get; set; }
    public Enrollment Enrollment { get; set; } = null!;

    public DateTime Date { get; set; } = DateTime.UtcNow.Date;
    public bool IsPresent { get; set; }
}


