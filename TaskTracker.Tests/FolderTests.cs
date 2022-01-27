using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace TaskTracker.Tests;

public class FolderTests
{
    [Test]
    public void CreateFolder()
    {
        // Arrange
        const int ID = 42;
        const string TITLE = "title 42";

        // Act
        var folder = new Folder(ID, TITLE);

        // Assert
        folder.Id.Should().Be(ID);
        folder.Title.Should().Be(TITLE);
    }

    #region Add task to folder
    [Test]
    public void AddNullTaskToFolder()
    {
        // Arrange
        var folder = new Folder(title: "title 42");

        // Act
        Action action = () => folder.AddTask(task: null);

        // Assert
        action.Should().Throw<ArgumentNullException>()
                       .WithParameterName(paramName: "task");
        folder.Tasks.Should().BeEmpty();
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
        var folder = new Folder(ID, TITLE);

        // Act
        folder.AddTask(task);

        // Assert
        folder.Tasks.Should().Equal(expectedTasks);
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
        var folder = new Folder(ID, TITLE);

        // Act & Assert
        folder.AddTask(task);
        folder.Tasks.Should().ContainSingle(t => t.Equals(task));

        var otherTask = new UserTask(TASK_ID, TASK_TITLE, TASK_DESCRIPTION);
        folder.AddTask(otherTask);
        folder.Tasks.Should().ContainSingle(t => t.Equals(task));
    }
    #endregion

    #region Get incomplete task count
    [Test]
    public void GetIncompleteTaskCountWhenFolderIsEmpty()
    {
        // Arrange
        var folder = new Folder(title: "title 42");

        // Act
        var incompleteTaskCount = folder.IncompleteTaskCount;

        // Assert
        incompleteTaskCount.Should().Be(0);
    }

    [Test]
    public void GetIncompleteTaskCountWhenFolderHasDifferentTasks()
    {
        // Arrange
        var folder = new Folder(title: "title 42_1");

        var incompleteTask = new UserTask(title: "title 42_2", description: "description 42_2");
        var completedTask = new UserTask(title: "title 42_3", description: "description 42_3");
        completedTask.Complete(new DateTime(year: 2022, month: 1, day: 10));
        var deletedTask = new UserTask(title: "title 42_4", description: "description 42_4");
        deletedTask.Delete(new DateTime(year: 2022, month: 2, day: 20));

        folder.AddTask(incompleteTask);
        folder.AddTask(completedTask);
        folder.AddTask(deletedTask);

        // Act
        var incompleteTaskCount = folder.IncompleteTaskCount;

        // Assert
        incompleteTaskCount.Should().Be(1);
    }
    #endregion
}
