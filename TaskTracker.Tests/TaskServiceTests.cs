using System.Threading.Tasks;

namespace TaskTracker.Tests;

// TODO rewrite to integration tests
public class TaskServiceTests
{
    #region Create task
    [Test]
    public async Task CreateTaskInNonExistentFolder()
    {
        // Arrange
        var mockRepository = new Mock<ITaskRepository>();
        mockRepository.Setup(r => r.GetFolder(It.IsAny<int>()))
                      .Returns(Task.FromResult((Folder)null));

        var userTaskChangeData = new UserTaskChangeData(id: 42, title: "Task title 42",
            description: "Description 42", null, folderId: 42);

        var sut = new TaskService(mockRepository.Object);

        // Act
        var action = () => sut.CreateTask(userTaskChangeData);

        // Assert
        var exception = await action.Should().ThrowAsync<DomainEntityNotFoundException>()
                                             .WithMessage("Папка не обнаружена");
        exception.And.DomainEntityType.Should().Be(typeof(Folder));
        mockRepository.Verify(r => r.SaveNewTask(It.IsAny<UserTask>(), It.IsAny<int>()),
                              Times.Never);
    }

    [Test]
    public async Task CreateTaskInExistentFolder()
    {
        // Arrange
        const string TITLE = "title 42";
        const string DESCRIPTION = "description 42";
        const int FOLDER_ID = 42;

        var folder = CreateFolder(FOLDER_ID);

        var mockRepository = new Mock<ITaskRepository>();
        mockRepository.Setup(r => r.GetFolder(FOLDER_ID))
                      .Returns(Task.FromResult(folder));

        var userTaskChangeData = new UserTaskChangeData(id: 42, TITLE, DESCRIPTION,
            dueDate: null, FOLDER_ID);

        var sut = new TaskService(mockRepository.Object);

        // Act
        var task = await sut.CreateTask(userTaskChangeData);

        // Assert
        task.Title.Should().Be(TITLE);
        task.Description.Should().Be(DESCRIPTION);
        mockRepository.Verify(r => r.SaveNewTask(It.IsAny<UserTask>(), It.IsAny<int>()),
                              Times.Once);
    }
    #endregion

    #region Complete and incomplete task
    [Test]
    public async Task CompleteNonexistentTask()
    {
        // Arrange
        var mockRepository = new Mock<ITaskRepository>();
        mockRepository.Setup(r => r.GetTask(It.IsAny<int>()))
                      .Returns(Task.FromResult((UserTask)null));

        var sut = new TaskService(mockRepository.Object);

        // Act
        var action = () => sut.CompleteTask(taskId: 42);

        // Assert
        var exception = await action.Should().ThrowAsync<DomainEntityNotFoundException>()
                                             .WithMessage("Задача не обнаружена");
        exception.And.DomainEntityType.Should().Be(typeof(UserTask));
        mockRepository.Verify(r => r.UpdateTask(It.IsAny<UserTask>()),
                              Times.Never);
    }

    [Test]
    public async Task CompleteExistentTask()
    {
        // Arrange
        const int TASK_ID = 42;
        var task = CreateTask(TASK_ID);

        var mockRepository = new Mock<ITaskRepository>();
        mockRepository.Setup(r => r.GetTask(TASK_ID))
                      .Returns(Task.FromResult(task));

        var sut = new TaskService(mockRepository.Object);

        // Act
        await sut.CompleteTask(TASK_ID);

        // Assert
        // TODO check date?
        mockRepository.Verify(r => r.UpdateTask(It.IsAny<UserTask>()),
                              Times.Once);
    }

    [Test]
    public async Task IncompleteNonexistentTask()
    {
        // Arrange
        var mockRepository = new Mock<ITaskRepository>();
        mockRepository.Setup(r => r.GetTask(It.IsAny<int>()))
                      .Returns(Task.FromResult((UserTask)null));

        var sut = new TaskService(mockRepository.Object);

        // Act
        var action = () => sut.IncompleteTask(taskId: 42);

        // Assert
        var exception = await action.Should().ThrowAsync<DomainEntityNotFoundException>()
                                             .WithMessage("Задача не обнаружена");
        exception.And.DomainEntityType.Should().Be(typeof(UserTask));
        mockRepository.Verify(r => r.UpdateTask(It.IsAny<UserTask>()),
                              Times.Never);
    }

    [Test]
    public async Task IncompleteExistentTask()
    {
        // Arrange
        const int TASK_ID = 42;
        var task = CreateTask(TASK_ID);

        var mockRepository = new Mock<ITaskRepository>();
        mockRepository.Setup(r => r.GetTask(TASK_ID))
                      .Returns(Task.FromResult(task));

        var sut = new TaskService(mockRepository.Object);

        // Act
        await sut.IncompleteTask(TASK_ID);

        // Assert
        // TODO check date?
        mockRepository.Verify(r => r.UpdateTask(It.IsAny<UserTask>()),
                              Times.Once);
    }
    #endregion

    #region Delete task
    [Test]
    public async Task DeleteNonexistentTask()
    {
        // Arrange
        var mockRepository = new Mock<ITaskRepository>();
        mockRepository.Setup(r => r.GetTask(It.IsAny<int>()))
                      .Returns(Task.FromResult((UserTask)null));

        var sut = new TaskService(mockRepository.Object);

        // Act
        var action = () => sut.DeleteTask(taskId: 42);

        // Assert
        var exception = await action.Should().ThrowAsync<DomainEntityNotFoundException>()
                                             .WithMessage("Задача не обнаружена");
        exception.And.DomainEntityType.Should().Be(typeof(UserTask));
        mockRepository.Verify(r => r.UpdateTask(It.IsAny<UserTask>()),
                              Times.Never);
    }

    [Test]
    public async Task DeleteExistentTask()
    {
        // Arrange
        const int TASK_ID = 42;
        var task = CreateTask(TASK_ID);

        var mockRepository = new Mock<ITaskRepository>();
        mockRepository.Setup(r => r.GetTask(TASK_ID))
                      .Returns(Task.FromResult(task));

        var sut = new TaskService(mockRepository.Object);

        // Act
        await sut.DeleteTask(TASK_ID);

        // Assert
        // TODO check date?
        mockRepository.Verify(r => r.UpdateTask(It.IsAny<UserTask>()),
                              Times.Once);
    }
     #endregion

    #region Create folder
    [Test]
    public async Task CreateFolder()
    {
        // Arrange
        const string TITLE = "title 42";
        var folderChangeData = new FolderChangeData(id: 42, TITLE);

        var mockRepository = new Mock<ITaskRepository>();
        var taskService = new TaskService(mockRepository.Object);

        // Act
        var sut = await taskService.CreateFolder(folderChangeData);

        // Assert
        sut.Title.Should().Be(TITLE);
        mockRepository.Verify(r => r.SaveNewFolder(It.IsAny<Folder>()),
                              Times.Once());
    }
    #endregion

    #region Move task to other folder
    [Test]
    public async Task MoveNonExistentTaskToOtherFolder()
    {
        // Arrange
        var mockRepository = new Mock<ITaskRepository>();
        mockRepository.Setup(r => r.GetTask(It.IsAny<int>()))
                      .Returns(Task.FromResult((UserTask)null));

        var sut = new TaskService(mockRepository.Object);

        // Act
        var action = () => sut.MoveTaskToOtherFolder(taskId: 42_1, destinationFolderId: 42_2);

        // Assert
        var exception = await action.Should().ThrowAsync<DomainEntityNotFoundException>()
                                             .WithMessage("Задача не обнаружена");
        exception.And.DomainEntityType.Should().Be(typeof(UserTask));
        mockRepository.Verify(r => r.UpdateTaskFolder(It.IsAny<int>(), It.IsAny<int>()),
                              Times.Never);
    }

    [Test]
    public async Task MoveTaskToNonExistentFolder()
    {
        // Arrange
        const int TASK_ID = 42_1;
        var task = CreateTask(TASK_ID);

        var mockRepository = new Mock<ITaskRepository>();
        mockRepository.Setup(r => r.GetTask(TASK_ID))
                      .Returns(Task.FromResult(task));
        mockRepository.Setup(r => r.GetFolder(It.IsAny<int>()))
                      .Returns(Task.FromResult((Folder)null));

        var sut = new TaskService(mockRepository.Object);

        // Act
        var action = () => sut.MoveTaskToOtherFolder(TASK_ID, destinationFolderId: 42_2);

        // Assert
        var exception = await action.Should().ThrowAsync<DomainEntityNotFoundException>()
                                             .WithMessage("Папка не обнаружена");
        exception.And.DomainEntityType.Should().Be(typeof(Folder));
        mockRepository.Verify(r => r.UpdateTaskFolder(It.IsAny<int>(), It.IsAny<int>()),
                              Times.Never);
    }

    [Test]
    public async Task MoveTaskToFolder()
    {
        // Arrange
        const int TASK_ID = 42_1;
        var task = CreateTask(TASK_ID);

        const int FOLDER_ID = 42_2;
        var folder = CreateFolder(FOLDER_ID);

        var mockRepository = new Mock<ITaskRepository>();
        mockRepository.Setup(r => r.GetTask(TASK_ID))
                      .Returns(Task.FromResult(task));
        mockRepository.Setup(r => r.GetFolder(FOLDER_ID))
                      .Returns(Task.FromResult(folder));

        var sut = new TaskService(mockRepository.Object);

        // Act
        await sut.MoveTaskToOtherFolder(TASK_ID, FOLDER_ID);

        // Assert
        mockRepository.Verify(r => r.UpdateTaskFolder(TASK_ID, FOLDER_ID), Times.Once);
    }
    #endregion

    #region helpers
    private Folder CreateFolder(int id = default, string title = default)
    {
        var folderChangeData = new FolderChangeData(id, title);
        return Folder.CreateFolder(folderChangeData);
    }

    private UserTask CreateTask(int id = 42, string title = "Task title 42",
                                string description = "Description 42")
    {
        var userTaskChangeData = new UserTaskChangeData(id, title, description,
            dueDate: null, folderId: 42);

        return UserTask.CreateTask(userTaskChangeData);
    }
    #endregion
}
