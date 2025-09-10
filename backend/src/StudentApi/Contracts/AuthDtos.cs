using System.ComponentModel.DataAnnotations;

namespace StudentApi.Contracts;

public class RegisterRequest
{
    [Required, EmailAddress, StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(128, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public string Role { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Name { get; set; }

    [StringLength(100)]
    public string? Surname { get; set; }

   
    [StringLength(100)]
    public string? NumberOrTitle { get; set; }
}

public class LoginRequest
{
    [Required, EmailAddress, StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(128, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;
}

public record AuthResponse(string Token, string Role);
