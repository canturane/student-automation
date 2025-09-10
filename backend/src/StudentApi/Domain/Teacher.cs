namespace StudentApi.Domain;

public class Teacher
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = default!;
    public string Surname { get; set; } = default!;
    public string? Title { get; set; }

    public User? User { get; set; } 
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
