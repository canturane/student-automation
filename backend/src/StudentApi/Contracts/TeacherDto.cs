
namespace StudentApi.Contracts;

public class TeacherDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string? Title { get; set; }
}
