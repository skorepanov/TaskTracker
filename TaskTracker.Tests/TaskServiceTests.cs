using System.Threading.Tasks;

namespace TaskTracker.Tests;

public class TaskServiceTests
{
    #region Create task
    [Fact]
    public async Task CreateTaskInNonExistentFolder()
    {
        // Arrange
        var mockTaskRepository = new Mock<ITaskRepository>();

        var mockFolderRepository = new Mock<IFolderRepository>();
        mockFolderRepository
            .Setup(r => r.GetFolder(It.IsAny<int>()))
            .Returns(Task.FromResult((Folder?)null));

        const int FOLDER_ID = 42;

        var userTaskDto = new UserTaskForCreationDto(
            Title: "Task title 42",
            Description: "Description 42",
            FOLDER_ID,
            DueDateTime: null,
            CreatedDateTime: null);

        var sut = new TaskService(
            mockTaskRepository.Object,
            mockFolderRepository.Object);

        // Act
        var action = () => sut.CreateTask(userTaskDto);

        // Assert
        var exception = await action.Should()
            .ThrowAsync<DomainEntityNotFoundException>()
            .WithMessage($"Папка не обнаружена (id = {FOLDER_ID})");
        exception.And.DomainEntityType.Should().Be(typeof(Folder));
        mockTaskRepository
            .Verify(r => r.CreateTask(It.IsAny<UserTask>()),
                    Times.Never);
    }

    [Fact]
    public async Task CreateTaskInExistentFolder()
    {
        // Arrange
        const string TITLE = "title 42";
        const string DESCRIPTION = "description 42";
        const int FOLDER_ID = 42;

        var mockTaskRepository = new Mock<ITaskRepository>();

        var folder = CreateTestFolder(title: "Folder title 42");

        var mockFolderRepository = new Mock<IFolderRepository>();
        mockFolderRepository
            .Setup(r => r.GetFolder(FOLDER_ID))
            .Returns(Task.FromResult<Folder?>(folder));

        var userTaskDto = new UserTaskForCreationDto(
            TITLE,
            DESCRIPTION,
            FOLDER_ID,
            DueDateTime: null,
            CreatedDateTime: null);

        var sut = new TaskService(
            mockTaskRepository.Object,
            mockFolderRepository.Object);

        // Act
        var task = await sut.CreateTask(userTaskDto);

        // Assert
        task.Title.Should().Be(TITLE);
        task.Description.Should().Be(DESCRIPTION);
        mockTaskRepository
            .Verify(r => r.CreateTask(It.IsAny<UserTask>()),
                    Times.Once);
    }
    #endregion

    #region Complete task
    [Fact]
    public async Task CompleteNonexistentTask()
    {
        // Arrange
        const int TASK_ID = 42;
        var userTaskDto = new UserTaskForCompleteDto(CompletedDateTime: null);

        var mockTaskRepository = new Mock<ITaskRepository>();
        mockTaskRepository
            .Setup(r => r.GetTask(It.IsAny<int>()))
            .Returns(Task.FromResult((UserTask?)null));

        var sut = new TaskService(
            mockTaskRepository.Object,
            _folderRepository: new Mock<IFolderRepository>().Object);

        // Act
        var action = () => sut.CompleteTask(TASK_ID, userTaskDto);

        // Assert
        var exception = await action.Should()
            .ThrowAsync<DomainEntityNotFoundException>()
            .WithMessage($"Задача не обнаружена (id = {TASK_ID})");
        exception.And.DomainEntityType.Should().Be(typeof(UserTask));
        mockTaskRepository
            .Verify(r => r.UpdateTask(It.IsAny<UserTask>()),
                    Times.Never);
    }

    [Fact]
    public async Task CompleteExistentTask()
    {
        // Arrange
        const int TASK_ID = 42;
        var task = CreateTask();
        var userTaskDto = new UserTaskForCompleteDto(CompletedDateTime: null);

        var mockTaskRepository = new Mock<ITaskRepository>();
        mockTaskRepository
            .Setup(r => r.GetTask(TASK_ID))
            .Returns(Task.FromResult<UserTask?>(task));

        var sut = new TaskService(
            mockTaskRepository.Object,
            _folderRepository: new Mock<IFolderRepository>().Object);

        // Act
        await sut.CompleteTask(TASK_ID, userTaskDto);

        // Assert
        mockTaskRepository
            .Verify(r => r.UpdateTask(It.IsAny<UserTask>()),
                    Times.Once);
    }
    #endregion

    #region Incomplete task
    [Fact]
    public async Task IncompleteNonexistentTask()
    {
        // Arrange
        const int TASK_ID = 42;
        var userTaskDto = new UserTaskForIncompleteDto(ModifiedDateTime: null);

        var mockTaskRepository = new Mock<ITaskRepository>();
        mockTaskRepository
            .Setup(r => r.GetTask(It.IsAny<int>()))
            .Returns(Task.FromResult((UserTask?)null));

        var sut = new TaskService(
            mockTaskRepository.Object,
            _folderRepository: new Mock<IFolderRepository>().Object);

        // Act
        var action = () => sut.IncompleteTask(TASK_ID, userTaskDto);

        // Assert
        var exception = await action.Should()
            .ThrowAsync<DomainEntityNotFoundException>()
            .WithMessage($"Задача не обнаружена (id = {TASK_ID})");
        exception.And.DomainEntityType.Should().Be(typeof(UserTask));
        mockTaskRepository
            .Verify(r => r.UpdateTask(It.IsAny<UserTask>()),
                    Times.Never);
    }

    [Fact]
    public async Task IncompleteExistentTask()
    {
        // Arrange
        const int TASK_ID = 42;
        var task = CreateTask();
        var userTaskDto = new UserTaskForIncompleteDto(ModifiedDateTime: null);

        var mockTaskRepository = new Mock<ITaskRepository>();
        mockTaskRepository
            .Setup(r => r.GetTask(TASK_ID))
            .Returns(Task.FromResult<UserTask?>(task));

        var sut = new TaskService(
            mockTaskRepository.Object,
            _folderRepository: new Mock<IFolderRepository>().Object);

        // Act
        await sut.IncompleteTask(TASK_ID, userTaskDto);

        // Assert
        mockTaskRepository
            .Verify(r => r.UpdateTask(It.IsAny<UserTask>()),
                    Times.Once);
    }
    #endregion

    #region Move task to trash
    [Fact]
    public async Task MoveNonexistentTaskToTrash()
    {
        // Arrange
        const int TASK_ID = 42;

        var userTaskDto = new UserTaskForMoveToTrashDto(It.IsAny<DateTime?>());

        var mockTaskRepository = new Mock<ITaskRepository>();
        mockTaskRepository
            .Setup(r => r.GetTask(It.IsAny<int>()))
            .Returns(Task.FromResult((UserTask?)null));

        var sut = new TaskService(
            mockTaskRepository.Object,
            _folderRepository: new Mock<IFolderRepository>().Object);

        // Act
        var action = () => sut.MoveTaskToTrash(TASK_ID, userTaskDto);

        // Assert
        var exception = await action.Should()
            .ThrowAsync<DomainEntityNotFoundException>()
            .WithMessage($"Задача не обнаружена (id = {TASK_ID})");
        exception.And.DomainEntityType.Should().Be(typeof(UserTask));
        mockTaskRepository
            .Verify(r => r.UpdateTask(It.IsAny<UserTask>()),
                    Times.Never);
    }

    [Fact]
    public async Task MoveExistentTaskToTrash()
    {
        // Arrange
        const int TASK_ID = 42;
        var task = CreateTask();

        var userTaskDto = new UserTaskForMoveToTrashDto(It.IsAny<DateTime?>());

        var mockTaskRepository = new Mock<ITaskRepository>();
        mockTaskRepository
            .Setup(r => r.GetTask(TASK_ID))
            .Returns(Task.FromResult<UserTask?>(task));

        var sut = new TaskService(
            mockTaskRepository.Object,
            _folderRepository: new Mock<IFolderRepository>().Object);

        // Act
        await sut.MoveTaskToTrash(TASK_ID, userTaskDto);

        // Assert
        mockTaskRepository
            .Verify(r => r.UpdateTask(It.IsAny<UserTask>()),
                    Times.Once);
    }
     #endregion

    #region Create folder
    [Fact]
    public async Task CreateFolder()
    {
        // Arrange
        const string TITLE = "title 42";
        var createdDateTime = new DateTime(year: 2025, month: 4, day: 24);
        var folderDto = new FolderForCreationDto(TITLE, createdDateTime);

        var mockFolderRepository = new Mock<IFolderRepository>();

        var taskService = new TaskService(
            _taskRepository: new Mock<ITaskRepository>().Object,
            mockFolderRepository.Object);

        // Act
        var sut = await taskService.CreateFolder(folderDto);

        // Assert
        sut.Title.Should().Be(TITLE);
        sut.CreatedDateTime.Should().Be(createdDateTime);
        mockFolderRepository
            .Verify(r => r.CreateFolder(It.IsAny<Folder>()),
                    Times.Once());
    }
    #endregion

    #region helpers
    private Folder CreateTestFolder(
        string title = "Folder title 42",
        DateTime? createdDateTime = null,
        DateTime? now = null)
    {
        createdDateTime ??= new DateTime(year: 2025, month: 1, day: 1);
        now ??= new DateTime(year: 2025, month: 1, day: 2);

        var folderDto = new FolderForCreationDto(title, createdDateTime.Value);

        return Folder.CreateFolder(folderDto, now.Value);
    }

    private UserTask CreateTask(
        string title = "Task title 42",
        string description = "Description 42",
        DateTime? createdDateTime = null,
        DateTime? now = null)
    {
        createdDateTime ??= new DateTime(year: 2025, month: 1, day: 1);
        now ??= new DateTime(year: 2025, month: 1, day: 2);

        var userTaskDto = new UserTaskForCreationDto(
            title,
            description,
            FolderId: 42,
            DueDateTime: null,
            createdDateTime);

        return UserTask.CreateTask(userTaskDto, now.Value);
    }
    #endregion
}
