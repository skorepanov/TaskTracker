using FluentAssertions;
using NUnit.Framework;

namespace TaskTracker.Tests;

public class UserTaskTests
{
    [Test]
    public void CreateTask()
    {
        // Arrange
        const int ID = 42;
        const string TITLE = "title 42";
        const string DESCRIPTION = "description 42";

        // Act
        var task = new UserTask(ID, TITLE, DESCRIPTION);

        // Assert
        task.Id.Should().Be(ID);
        task.Title.Should().Be(TITLE);
        task.Description.Should().Be(DESCRIPTION);
    }
}
