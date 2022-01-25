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
}
