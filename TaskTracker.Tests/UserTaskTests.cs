using System;
using FluentAssertions;
using NUnit.Framework;
using TaskTracker.Bll;

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

    [Test]
    public void DeleteTask()
    {
        // Arrange
        var task = new UserTask(title: "title 42", description: "description 42");
        var deletionDate = new DateTime(year: 2022, month: 2, day: 10);

        // Act
        task.Delete(deletionDate);

        // Assert
        task.DeletionDate.Should().Be(deletionDate);
        task.IsDeleted.Should().BeTrue();
    }
}
