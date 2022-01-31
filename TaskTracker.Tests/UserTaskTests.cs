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

    #region Calculate overdue days
    [Test]
    public void CalculateOverdueDaysForTaskWithoutDueDate()
    {
        // Arrange
        var task = new UserTask(title: "title 42", description: "description 42");
        var today = new DateTime(year: 2022, month: 2, day: 7);

        // Act
        var overdueDayCount = task.CalculateOverdueDays(today);

        // Assert
        task.DueDate.Should().BeNull();
        overdueDayCount.Should().Be(0);
    }

    [Test]
    public void CalculateOverdueDaysForTaskWithDueDate()
    {
        // Arrange
        var task = new UserTask(title: "title 42", description: "description 42");

        var dueDate = new DateTime(year: 2022, month: 2, day: 5);
        task.DueDate = dueDate;

        var today = new DateTime(year: 2022, month: 2, day: 7);

        // Act
        var overdueDayCount = task.CalculateOverdueDays(today);

        // Assert
        task.DueDate.Should().Be(dueDate);
        overdueDayCount.Should().Be(2);
    }

    [Test]
    public void CalculateOverdueDaysForNonOverdueTask()
    {
        // Arrange
        var task = new UserTask(title: "title 42", description: "description 42");

        var dueDate = new DateTime(year: 2022, month: 2, day: 5);
        task.DueDate = dueDate;

        var today = new DateTime(year: 2022, month: 2, day: 3);

        // Act
        var overdueDayCount = task.CalculateOverdueDays(today);

        // Assert
        task.DueDate.Should().Be(dueDate);
        overdueDayCount.Should().Be(0);
    }

    [Test]
    public void CalculateOverdueDaysForTodayTask()
    {
        // Arrange
        var task = new UserTask(title: "title 42", description: "description 42");

        var dueDate = new DateTime(year: 2022, month: 2, day: 5);
        task.DueDate = dueDate;

        var today = dueDate;

        // Act
        var overdueDayCount = task.CalculateOverdueDays(today);

        // Assert
        task.DueDate.Should().Be(dueDate);
        overdueDayCount.Should().Be(0);
    }

    [Test]
    public void CalculateOverdueDaysForDeletedTask()
    {
        // Arrange
        var task = new UserTask(title: "title 42", description: "description 42");

        var dueDate = new DateTime(year: 2022, month: 2, day: 5);
        task.DueDate = dueDate;

        var deletionDate = new DateTime(year: 2022, month: 2, day: 6);
        task.Delete(deletionDate);

        var today = new DateTime(year: 2022, month: 2, day: 7);

        // Act
        var overdueDayCount = task.CalculateOverdueDays(today);

        // Assert
        task.DueDate.Should().Be(dueDate);
        overdueDayCount.Should().Be(0);
    }
    #endregion

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
