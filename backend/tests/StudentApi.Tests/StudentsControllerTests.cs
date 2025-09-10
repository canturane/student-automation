using Microsoft.EntityFrameworkCore;
using StudentApi.Controllers;
using StudentApi.Domain;
using StudentApi.Infrastructure;
using StudentApi.Contracts;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace StudentApi.Tests;

public class StudentsControllerTests
{
    // InMemory DbContext helper
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // her test için ayrı db
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task Create_Should_Add_New_Student()
    {
        // Arrange
        var db = GetDbContext();
        var controller = new StudentsController(db);

        var dto = new StudentWithUserCreateDto
        {
            Email = "ali@example.com",
            Password = "12345",
            Name = "Ali",
            Surname = "Veli",
            Number = "S00001"
        };

        // Act
        var result = await controller.Create(dto);

        // Assert
        var created = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Single(db.Students);
        Assert.Equal("Ali", db.Students.First().Name);
    }
}
