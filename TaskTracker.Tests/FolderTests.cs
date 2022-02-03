using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using TaskTracker.Bll;

namespace TaskTracker.Tests;

public class FolderTests
{
    #region Add task to folder
    [Test]
    public void AddNullTaskToFolder()
    {
        // Arrange
        var sut = new Folder(title: "title 42");

        // Act
        Action action = () => sut.AddTask(task: null);

        // Assert
        action.Should().Throw<ArgumentNullException>()
                       .WithParameterName(paramName: "task");
        sut.Tasks.Should().BeEmpty();
    }

    [Test]
    public void AddOneTaskToFolder()
    {
        // Arrange
        const int TASK_ID = 42;
        const string TASK_TITLE = "title 42";
        const string TASK_DESCRIPTION = "description 42";
        var task = new UserTask(TASK_ID, TASK_TITLE, TASK_DESCRIPTION);
        var expectedTasks = new List<UserTask> { task };

        const int ID = 42;
        const string TITLE = "title 42";
        var sut = new Folder(ID, TITLE);

        // Act
        sut.AddTask(task);

        // Assert
        sut.Tasks.Should().Equal(expectedTasks);
    }

    [Test]
    public void AddSeveralTasksWithSameIdToFolder()
    {
        // Arrange
        const int TASK_ID = 42;
        const string TASK_TITLE = "title 42";
        const string TASK_DESCRIPTION = "description 42";
        var task = new UserTask(TASK_ID, TASK_TITLE, TASK_DESCRIPTION);

        const int ID = 42;
        const string TITLE = "title 42";
        var sut = new Folder(ID, TITLE);

        // Act & Assert
        sut.AddTask(task);
        sut.Tasks.Should().ContainSingle(t => t.Equals(task));

        var otherTask = new UserTask(TASK_ID, TASK_TITLE, TASK_DESCRIPTION);
        sut.AddTask(otherTask);
        sut.Tasks.Should().ContainSingle(t => t.Equals(task));
    }
    #endregion

    #region Get incomplete task count
    [Test]
    public void GetIncompleteTaskCountWhenFolderIsEmpty()
    {
        // Arrange
        var sut = new Folder(title: "title 42");

        // Act
        var incompleteTaskCount = sut.IncompleteTaskCount;

        // Assert
        incompleteTaskCount.Should().Be(0);
    }

    [Test]
    public void GetIncompleteTaskCountWhenFolderHasDifferentTasks()
    {
        // Arrange
        var sut = new Folder(title: "title 42_1");

        var incompleteTask = new UserTask(title: "title 42_2", description: "description 42_2");
        var completedTask = new UserTask(title: "title 42_3", description: "description 42_3");
        completedTask.Complete(new DateTime(year: 2022, month: 1, day: 10));
        var deletedTask = new UserTask(title: "title 42_4", description: "description 42_4");
        deletedTask.Delete(new DateTime(year: 2022, month: 2, day: 20));

        sut.AddTask(incompleteTask);
        sut.AddTask(completedTask);
        sut.AddTask(deletedTask);

        // Act
        var incompleteTaskCount = sut.IncompleteTaskCount;

        // Assert
        incompleteTaskCount.Should().Be(1);
    }
    #endregion
}
