using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Xunit;
using StudentApi.Contracts;

namespace StudentApi.Tests;

public class AuthIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AuthIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_And_Login_Should_Return_Token()
    {
        // Arrange - test kullanıcı
        var registerDto = new RegisterRequest
        {
            Email = "testuser@example.com",
            Password = "123456",   
            Role = "Student",
            Name = "Test",
            Surname = "User",
            NumberOrTitle = "S00001"
        };

        // Register çağrısı
        var regRes = await _client.PostAsJsonAsync("/api/Auth/register", registerDto);
        regRes.EnsureSuccessStatusCode();

        // Login çağrısı
        var loginDto = new LoginRequest
        {
            Email = registerDto.Email,
            Password = registerDto.Password
        };

        var loginRes = await _client.PostAsJsonAsync("/api/Auth/login", loginDto);
        loginRes.EnsureSuccessStatusCode();

        var authResp = await loginRes.Content.ReadFromJsonAsync<AuthResponse>();

        // Assert
        Assert.NotNull(authResp);
        Assert.False(string.IsNullOrWhiteSpace(authResp!.Token));
        Assert.Equal("student", authResp.Role);
    }
}
