using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using TaskTracker.Bll;

namespace TaskTracker.Tests;

public class UserTaskTests
{
    #region Complete and incomplete task
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
    #endregion

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

    #region Is today task
    [Test]
    public void IsTodayTaskThatCompletedToday()
    {
        // Arrange
        var today = new DateTime(year: 2022, month: 2, day: 5);

        var task = new UserTask(title: "Title 42", description: "Description 42");
        var completionDate = today;
        task.Complete(completionDate);

        // Act
        var isTodayTask = task.IsTodayTask(today);

        // Assert
        isTodayTask.Should().BeTrue();
    }

    [Test]
    public void IsTodayTaskThatCompletedEarlier()
    {
        // Arrange
        var task = new UserTask(title: "Title 42", description: "Description 42");
        var completionDate = new DateTime(year: 2022, month: 2, day: 5);
        task.Complete(completionDate);

        var today = new DateTime(year: 2022, month: 2, day: 7);

        // Act
        var isTodayTask = task.IsTodayTask(today);

        // Assert
        isTodayTask.Should().BeFalse();
    }

    [Test]
    public void IsTodayIncompleteTaskWithoutDueDate()
    {
        // Arrange
        var task = new UserTask(title: "Title 42", description: "Description 42");
        var today = new DateTime(year: 2022, month: 2, day: 7);

        // Act
        var isTodayTask = task.IsTodayTask(today);

        // Assert
        isTodayTask.Should().BeFalse();
    }

    [TestCaseSource(nameof(GetTestCasesForIsTodayTaskWithDueDate))]
    public void IsTodayIncompleteTaskWithDueDate(DateTime dueDate, DateTime today,
                                                 bool expectedResult)
    {
        // Arrange
        var task = new UserTask(title: "Title 42", description: "Description 42");
        task.DueDate = dueDate;

        // Act
        var isTodayTask = task.IsTodayTask(today);

        // Assert
        isTodayTask.Should().Be(expectedResult);
    }

    private static IEnumerable<TestCaseData> GetTestCasesForIsTodayTaskWithDueDate()
    {
        yield return new TestCaseData(new DateTime(year: 2022, month: 2, day: 6),
                                      new DateTime(year: 2022, month: 2, day: 7),
                                      true);

        yield return new TestCaseData(new DateTime(year: 2022, month: 2, day: 7),
                                      new DateTime(year: 2022, month: 2, day: 7),
                                      true);

        yield return new TestCaseData(new DateTime(year: 2022, month: 2, day: 8),
                                      new DateTime(year: 2022, month: 2, day: 7),
                                      false);
    }

    [TestCaseSource(nameof(GetTestCasesForIsTodayDeletedTask))]
    public void IsTodayDeletedTask(DateTime deletionDate, DateTime today)
    {
        // Arrange
        var task = new UserTask(title: "Title 42", description: "Description 42");
        task.DueDate = today;
        task.Delete(deletionDate);

        // Act
        var isTodayTask = task.IsTodayTask(today);

        // Assert
        isTodayTask.Should().BeFalse();
    }

    private static IEnumerable<TestCaseData> GetTestCasesForIsTodayDeletedTask()
    {
        yield return new TestCaseData(new DateTime(year: 2022, month: 2, day: 6),
                                      new DateTime(year: 2022, month: 2, day: 7));

        yield return new TestCaseData(new DateTime(year: 2022, month: 2, day: 7),
                                      new DateTime(year: 2022, month: 2, day: 7));
    }
    #endregion

    #region Delete task
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
    #endregion
}
