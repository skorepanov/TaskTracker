using System;
using FluentAssertions;
using NUnit.Framework;

namespace TaskTracker.Tests;

public class UserTaskTests
{
    [Test]
    public void CompleteTask()
    {
        // Arrange
        var task = new UserTask(title: "title 42", description: "description 42");
        var completionDate = new DateTime(year: 2022, month: 2, day: 10);

        // Act
        task.Complete(completionDate);

        // Assert
        task.CompletionDate.Should().Be(completionDate);
        task.IsCompleted.Should().BeTrue();
    }

    [Test]
    public void IncompleteTask()
    {
        // Arrange
        var task = new UserTask(title: "title 42", description: "description 42");
        var completionDate = new DateTime(year: 2022, month: 2, day: 10);
        task.Complete(completionDate);

        // Act
        task.Incomplete();

        // Assert
        task.CompletionDate.Should().BeNull();
        task.IsCompleted.Should().BeFalse();
    }
}
