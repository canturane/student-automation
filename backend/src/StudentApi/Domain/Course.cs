namespace StudentApi.Domain;

public enum CourseStatus { Planned=0, Started=1, Finished=2 }

public class Course
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public CourseStatus Status { get; set; } = CourseStatus.Planned;

    public int TeacherId { get; set; }
    public Teacher Teacher { get; set; } = default!;

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
