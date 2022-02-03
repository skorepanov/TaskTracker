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
        var sut = new UserTask(title: "title 42", description: "description 42");
        var completionDate = new DateTime(year: 2022, month: 2, day: 10);

        // Act
        sut.Complete(completionDate);

        // Assert
        sut.CompletionDate.Should().Be(completionDate);
        sut.IsCompleted.Should().BeTrue();
    }

    [Test]
    public void IncompleteTask()
    {
        // Arrange
        var sut = new UserTask(title: "title 42", description: "description 42");
        var completionDate = new DateTime(year: 2022, month: 2, day: 10);
        sut.Complete(completionDate);

        // Act
        sut.Incomplete();

        // Assert
        sut.CompletionDate.Should().BeNull();
        sut.IsCompleted.Should().BeFalse();
    }
    #endregion

    #region Calculate overdue days
    [Test]
    public void CalculateOverdueDaysForTaskWithoutDueDate()
    {
        // Arrange
        var sut = new UserTask(title: "title 42", description: "description 42");
        var today = new DateTime(year: 2022, month: 2, day: 7);

        // Act
        var overdueDayCount = sut.CalculateOverdueDays(today);

        // Assert
        sut.DueDate.Should().BeNull();
        overdueDayCount.Should().Be(0);
    }

    [Test]
    public void CalculateOverdueDaysForTaskWithDueDate()
    {
        // Arrange
        var sut = new UserTask(title: "title 42", description: "description 42");

        var dueDate = new DateTime(year: 2022, month: 2, day: 5);
        sut.DueDate = dueDate;

        var today = new DateTime(year: 2022, month: 2, day: 7);

        // Act
        var overdueDayCount = sut.CalculateOverdueDays(today);

        // Assert
        sut.DueDate.Should().Be(dueDate);
        overdueDayCount.Should().Be(2);
    }

    [Test]
    public void CalculateOverdueDaysForNonOverdueTask()
    {
        // Arrange
        var sut = new UserTask(title: "title 42", description: "description 42");

        var dueDate = new DateTime(year: 2022, month: 2, day: 5);
        sut.DueDate = dueDate;

        var today = new DateTime(year: 2022, month: 2, day: 3);

        // Act
        var overdueDayCount = sut.CalculateOverdueDays(today);

        // Assert
        sut.DueDate.Should().Be(dueDate);
        overdueDayCount.Should().Be(0);
    }

    [Test]
    public void CalculateOverdueDaysForTodayTask()
    {
        // Arrange
        var sut = new UserTask(title: "title 42", description: "description 42");

        var dueDate = new DateTime(year: 2022, month: 2, day: 5);
        sut.DueDate = dueDate;

        var today = dueDate;

        // Act
        var overdueDayCount = sut.CalculateOverdueDays(today);

        // Assert
        sut.DueDate.Should().Be(dueDate);
        overdueDayCount.Should().Be(0);
    }

    [Test]
    public void CalculateOverdueDaysForDeletedTask()
    {
        // Arrange
        var sut = new UserTask(title: "title 42", description: "description 42");

        var dueDate = new DateTime(year: 2022, month: 2, day: 5);
        sut.DueDate = dueDate;

        var deletionDate = new DateTime(year: 2022, month: 2, day: 6);
        sut.Delete(deletionDate);

        var today = new DateTime(year: 2022, month: 2, day: 7);

        // Act
        var overdueDayCount = sut.CalculateOverdueDays(today);

        // Assert
        sut.DueDate.Should().Be(dueDate);
        overdueDayCount.Should().Be(0);
    }
    #endregion

    #region Is today task
    [Test]
    public void IsTodayTaskThatCompletedToday()
    {
        // Arrange
        var today = new DateTime(year: 2022, month: 2, day: 5);

        var sut = new UserTask(title: "Title 42", description: "Description 42");
        var completionDate = today;
        sut.Complete(completionDate);

        // Act
        var isTodayTask = sut.IsTodayTask(today);

        // Assert
        isTodayTask.Should().BeTrue();
    }

    [Test]
    public void IsTodayTaskThatCompletedEarlier()
    {
        // Arrange
        var sut = new UserTask(title: "Title 42", description: "Description 42");
        var completionDate = new DateTime(year: 2022, month: 2, day: 5);
        sut.Complete(completionDate);

        var today = new DateTime(year: 2022, month: 2, day: 7);

        // Act
        var isTodayTask = sut.IsTodayTask(today);

        // Assert
        isTodayTask.Should().BeFalse();
    }

    [Test]
    public void IsTodayIncompleteTaskWithoutDueDate()
    {
        // Arrange
        var sut = new UserTask(title: "Title 42", description: "Description 42");
        var today = new DateTime(year: 2022, month: 2, day: 7);

        // Act
        var isTodayTask = sut.IsTodayTask(today);

        // Assert
        isTodayTask.Should().BeFalse();
    }

    [TestCaseSource(nameof(GetTestCasesForIsTodayTaskWithDueDate))]
    public void IsTodayIncompleteTaskWithDueDate(DateTime dueDate, DateTime today,
                                                 bool expectedResult)
    {
        // Arrange
        var sut = new UserTask(title: "Title 42", description: "Description 42");
        sut.DueDate = dueDate;

        // Act
        var isTodayTask = sut.IsTodayTask(today);

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
        var sut = new UserTask(title: "Title 42", description: "Description 42");
        sut.DueDate = today;
        sut.Delete(deletionDate);

        // Act
        var isTodayTask = sut.IsTodayTask(today);

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
        var sut = new UserTask(title: "title 42", description: "description 42");
        var deletionDate = new DateTime(year: 2022, month: 2, day: 10);

        // Act
        sut.Delete(deletionDate);

        // Assert
        sut.DeletionDate.Should().Be(deletionDate);
        sut.IsDeleted.Should().BeTrue();
    }
    #endregion
}
