
namespace StudentApi.Contracts;

public class StudentDto
{
    public int Id { get; set; }              
    public int UserId { get; set; }          
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string Number { get; set; } = null!;
}
