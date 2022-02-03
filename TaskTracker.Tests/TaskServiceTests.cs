using System;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TaskTracker.Bll;

namespace TaskTracker.Tests;

public class TaskServiceTests
{
    #region Create task
    [Test]
    public void CreateTaskInNonExistentFolder()
    {
        // Arrange
        var mockRepository = new Mock<ITaskRepository>();
        mockRepository.Setup(r => r.GetFolder(It.IsAny<int>()))
                      .Returns((Folder)null);

        var sut = new TaskService(mockRepository.Object);

        // Act
        Action action = () => sut.CreateTask(title: "title 42",
            description: "description 42", folderId: 42);

        // Assert
        action.Should().Throw<DomainEntityNotFoundException>()
            .WithMessage("Папка не обнаружена")
            .And.DomainEntityType.Should().Be(typeof(Folder));
        mockRepository.Verify(r => r.SaveNewTask(It.IsAny<UserTask>(), It.IsAny<int>()),
                              Times.Never);
    }

    [Test]
    public void CreateTaskInExistentFolder()
    {
        // Arrange
        const string TITLE = "title 42";
        const string DESCRIPTION = "description 42";
        const int FOLDER_ID = 42;

        var folder = new Folder(title: "title 42");

        var mockRepository = new Mock<ITaskRepository>();
        mockRepository.Setup(r => r.GetFolder(FOLDER_ID))
                      .Returns(folder);

        var sut = new TaskService(mockRepository.Object);

        // Act
        var task = sut.CreateTask(TITLE, DESCRIPTION, FOLDER_ID);

        // Assert
        task.Title.Should().Be(TITLE);
        task.Description.Should().Be(DESCRIPTION);
        mockRepository.Verify(r => r.SaveNewTask(It.IsAny<UserTask>(), It.IsAny<int>()),
                              Times.Once);
    }
    #endregion

    #region Complete and incomplete task
    [Test]
    public void CompleteNonexistentTask()
    {
        // Arrange
        var mockRepository = new Mock<ITaskRepository>();
        mockRepository.Setup(r => r.GetTask(It.IsAny<int>()))
                      .Returns((UserTask)null);

        var sut = new TaskService(mockRepository.Object);

        // Act
        Action action = () => sut.CompleteTask(taskId: 42);

        // Assert
        action.Should().Throw<DomainEntityNotFoundException>()
                       .WithMessage("Задача не обнаружена")
                       .And.DomainEntityType.Should().Be(typeof(UserTask));
        mockRepository.Verify(r => r.UpdateTask(It.IsAny<UserTask>()),
                              Times.Never);
    }

    [Test]
    public void CompleteExistentTask()
    {
        // Arrange
        const int TASK_ID = 42;
        var task = new UserTask(title: "title 42", description: "description 42");

        var mockRepository = new Mock<ITaskRepository>();
        mockRepository.Setup(r => r.GetTask(TASK_ID))
                      .Returns(task);

        var sut = new TaskService(mockRepository.Object);

        // Act
        sut.CompleteTask(TASK_ID);

        // Assert
        // TODO check date?
        mockRepository.Verify(r => r.UpdateTask(It.IsAny<UserTask>()),
                              Times.Once);
    }

    [Test]
    public void IncompleteNonexistentTask()
    {
        // Arrange
        var mockRepository = new Mock<ITaskRepository>();
        mockRepository.Setup(r => r.GetTask(It.IsAny<int>()))
                      .Returns((UserTask)null);

        var sut = new TaskService(mockRepository.Object);

        // Act
        Action action = () => sut.IncompleteTask(taskId: 42);

        // Assert
        action.Should().Throw<DomainEntityNotFoundException>()
                       .WithMessage("Задача не обнаружена")
                       .And.DomainEntityType.Should().Be(typeof(UserTask));
        mockRepository.Verify(r => r.UpdateTask(It.IsAny<UserTask>()),
                              Times.Never);
    }

    [Test]
    public void IncompleteExistentTask()
    {
        // Arrange
        const int TASK_ID = 42;
        var task = new UserTask(title: "title 42", description: "description 42");

        var mockRepository = new Mock<ITaskRepository>();
        mockRepository.Setup(r => r.GetTask(TASK_ID))
                      .Returns(task);

        var sut = new TaskService(mockRepository.Object);

        // Act
        sut.IncompleteTask(TASK_ID);

        // Assert
        // TODO check date?
        mockRepository.Verify(r => r.UpdateTask(It.IsAny<UserTask>()),
                              Times.Once);
    }
    #endregion

    #region Delete task
    [Test]
    public void DeleteNonexistentTask()
    {
        // Arrange
        var mockRepository = new Mock<ITaskRepository>();
        mockRepository.Setup(r => r.GetTask(It.IsAny<int>()))
                      .Returns((UserTask)null);

        var sut = new TaskService(mockRepository.Object);

        // Act
        Action action = () => sut.DeleteTask(taskId: 42);

        // Assert
        action.Should().Throw<DomainEntityNotFoundException>()
                       .WithMessage("Задача не обнаружена")
                       .And.DomainEntityType.Should().Be(typeof(UserTask));
        mockRepository.Verify(r => r.UpdateTask(It.IsAny<UserTask>()),
                              Times.Never);
    }

    [Test]
    public void DeleteExistentTask()
    {
        // Arrange
        const int TASK_ID = 42;
        var task = new UserTask(title: "title 42", description: "description 42");

        var mockRepository = new Mock<ITaskRepository>();
        mockRepository.Setup(r => r.GetTask(TASK_ID))
                      .Returns(task);

        var sut = new TaskService(mockRepository.Object);

        // Act
        sut.DeleteTask(TASK_ID);

        // Assert
        // TODO check date?
        mockRepository.Verify(r => r.UpdateTask(It.IsAny<UserTask>()),
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
        var sut = taskService.CreateFolder(TITLE);

        // Assert
        sut.Title.Should().Be(TITLE);
        mockRepository.Verify(r => r.SaveNewFolder(It.IsAny<Folder>()),
                              Times.Once());
    }
    #endregion

    #region Move task to other folder
    [Test]
    public void MoveNonExistentTaskToOtherFolder()
    {
        // Arrange
        var mockRepository = new Mock<ITaskRepository>();
        mockRepository.Setup(r => r.GetTask(It.IsAny<int>()))
                      .Returns((UserTask)null);

        var sut = new TaskService(mockRepository.Object);

        // Act
        Action action = () => sut.MoveTaskToOtherFolder(taskId: 42_1, destinationFolderId: 42_2);

        // Assert
        action.Should().Throw<DomainEntityNotFoundException>()
            .WithMessage("Задача не обнаружена")
            .And.DomainEntityType.Should().Be(typeof(UserTask));
        mockRepository.Verify(r => r.UpdateTaskFolder(It.IsAny<int>(), It.IsAny<int>()),
                              Times.Never);
    }

    [Test]
    public void MoveTaskToNonExistentFolder()
    {
        // Arrange
        const int TASK_ID = 42_1;
        var task = new UserTask(title: "title 42", description: "description 42");

        var mockRepository = new Mock<ITaskRepository>();
        mockRepository.Setup(r => r.GetTask(TASK_ID))
                      .Returns(task);
        mockRepository.Setup(r => r.GetFolder(It.IsAny<int>()))
                      .Returns((Folder)null);

        var sut = new TaskService(mockRepository.Object);

        // Act
        Action action = () => sut.MoveTaskToOtherFolder(TASK_ID, destinationFolderId: 42_2);

        // Assert
        action.Should().Throw<DomainEntityNotFoundException>()
            .WithMessage("Папка не обнаружена")
            .And.DomainEntityType.Should().Be(typeof(Folder));
        mockRepository.Verify(r => r.UpdateTaskFolder(It.IsAny<int>(), It.IsAny<int>()),
                              Times.Never);
    }

    [Test]
    public void MoveTaskToFolder()
    {
        // Arrange
        const int TASK_ID = 42_1;
        var task = new UserTask(title: "title 42_1", description: "description 42_1");

        const int FOLDER_ID = 42_2;
        var folder = new Folder(title: "title 42_2");

        var mockRepository = new Mock<ITaskRepository>();
        mockRepository.Setup(r => r.GetTask(TASK_ID))
                      .Returns(task);
        mockRepository.Setup(r => r.GetFolder(FOLDER_ID))
                      .Returns(folder);

        var sut = new TaskService(mockRepository.Object);

        // Act
        sut.MoveTaskToOtherFolder(TASK_ID, FOLDER_ID);

        // Assert
        mockRepository.Verify(r => r.UpdateTaskFolder(TASK_ID, FOLDER_ID), Times.Once);
    }
    #endregion
}
