using FluentAssertions;
using NUnit.Framework;
using Moq;

namespace TaskTracker.Tests;

public class TaskServiceTests
{
    #region Create task
    [Test]
    public void CreateTaskWhenFolderNotExists()
    {
        // Arrange
        var mockRepository = new Mock<ITaskRepository>();
        mockRepository.Setup(r => r.GetFolder(It.IsAny<int>()))
                      .Returns((Folder)null);

        var taskService = new TaskService(mockRepository.Object);

        // Act
        var task = taskService.CreateTask(title: "title 42",
            description: "description 42", folderId: 42);

        // Assert
        // TODO: method should throw domain exception
        task.Should().BeNull();
        mockRepository.Verify(r => r.SaveNewTask(It.IsAny<UserTask>(), It.IsAny<int>()),
                              Times.Never);
    }

    [Test]
    public void CreateTaskWhenFolderExists()
    {
        // Arrange
        const string TITLE = "title 42";
        const string DESCRIPTION = "description 42";
        const int FOLDER_ID = 42;

        var folder = new Folder(title: "title 42");

        var mockRepository = new Mock<ITaskRepository>();
        mockRepository.Setup(r => r.GetFolder(FOLDER_ID))
                      .Returns(folder);

        var taskService = new TaskService(mockRepository.Object);

        // Act
        var task = taskService.CreateTask(TITLE, DESCRIPTION, FOLDER_ID);

        // Assert
        task.Title.Should().Be(TITLE);
        task.Description.Should().Be(DESCRIPTION);
        mockRepository.Verify(r => r.SaveNewTask(It.IsAny<UserTask>(), It.IsAny<int>()),
                              Times.Once);
    }
    #endregion

    #region Create folder
    [Test]
    public void CreateFolder()
    {
        // Arrange
        const string TITLE = "title 42";

        var mockRepository = new Mock<ITaskRepository>();
        var taskService = new TaskService(mockRepository.Object);

        // Act
        var folder = taskService.CreateFolder(TITLE);

        // Assert
        folder.Title.Should().Be(TITLE);
        mockRepository.Verify(r => r.SaveNewFolder(It.IsAny<Folder>()),
                              Times.Once());
    }
    #endregion
}
