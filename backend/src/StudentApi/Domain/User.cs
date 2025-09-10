namespace StudentApi.Domain;

public enum UserRole { Admin=1, Teacher=2, Student=3 }

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public UserRole Role { get; set; }

    public Student? Student { get; set; }
    public Teacher? Teacher { get; set; }
}
