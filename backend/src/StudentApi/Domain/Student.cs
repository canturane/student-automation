namespace StudentApi.Domain;

public class Student
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = default!;
    public string Surname { get; set; } = default!;
    public string Number { get; set; } = default!;

    public User User { get; set; } = default!;
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
