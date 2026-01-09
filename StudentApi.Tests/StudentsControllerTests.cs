using Microsoft.EntityFrameworkCore;
using StudentApi.Controllers;
using StudentApi.Models;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Controllers;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using Xunit;

namespace StudentApi.Tests
{
    public class StudentsControllerTests
    {
        // Helper method to create a fresh, empty "Fake" database for every test
        private AppDbContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact] // This tells xUnit this is a test
        public async Task GetStudent_ReturnsStudent_WhenStudentExists()
        {
            // 1. ARRANGE (Set up the scenario)
            var context = GetDatabaseContext();
            context.Students.Add(new Student { Id = 1, FirstName = "Alice", LastName = "Smith", Email = "alice@test.com" });
            await context.SaveChangesAsync();

            var controller = new StudentsController(context);

            // 2. ACT (Execute the method we are testing)
            var result = await controller.GetStudent(1);

            // 3. ASSERT (Verify the results)
            var actionResult = Assert.IsType<ActionResult<Student>>(result);
            var returnedStudent = Assert.IsType<Student>(actionResult.Value);
            Assert.Equal("Alice", returnedStudent.FirstName);
        }

        [Fact]
        public async Task GetStudent_ReturnsNotFound_WhenStudentDoesNotExist()
        {
            // Arrange
            var context = GetDatabaseContext();
            var controller = new StudentsController(context);

            // Act
            var result = await controller.GetStudent(999);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}