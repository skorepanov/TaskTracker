namespace TaskTracker.Tests;

public class FolderTests
{
    #region Add task to folder
    [Test]
    [Category("AddTask")]
    public void AddOneTaskToFolder()
    {
        // Arrange
        var task = CreateTask();
        var expectedTasks = new List<UserTask> { task };

        var sut = CreateSut();

        // Act
        sut.AddTask(task);

        // Assert
        sut.Tasks.Should().Equal(expectedTasks);
    }

    [Test]
    [Category("AddTask")]
    public void AddSeveralTasksToFolder()
    {
        // Arrange
        var task = CreateTask();

        var sut = CreateSut();

        // Act & Assert
        sut.AddTask(task);
        sut.Tasks.Should().ContainSingle(t => t.Equals(task));

        var otherTask = CreateTask();
        sut.AddTask(otherTask);
        sut.Tasks.Should().ContainSingle(t => t.Equals(task));
        sut.Tasks.Should().ContainSingle(t => t.Equals(otherTask));
    }
    #endregion

    #region Get incomplete task count
    [Test]
    [Category("IncompleteTaskCount")]
    public void GetIncompleteTaskCountWhenFolderIsEmpty()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var incompleteTaskCount = sut.IncompleteTaskCount;

        // Assert
        incompleteTaskCount.Should().Be(0);
    }

    [Test]
    [Category("IncompleteTaskCount")]
    public void GetIncompleteTaskCountWhenFolderHasDifferentTasks()
    {
        // Arrange
        var sut = CreateSut();

        var incompleteTask = CreateTask();

        var completedTask = CreateTask();
        completedTask.Complete(new DateTime(year: 2022, month: 1, day: 10));

        var deletedTask = CreateTask();
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

    #region Create folder
    [Test]
    [Category("CreateFolder")]
    public void CreateFolderWithFieldNormalization()
    {
        // Arrange
        const string TITLE = "Folder title";
        var createdDateTime = new DateTime(year: 2025, month: 4, day: 24);

        var folderDto = new FolderForCreationDto(
            Title: $"   {TITLE}    ",
            createdDateTime);

        // Act
        var sut = Folder.CreateFolder(folderDto, now: It.IsAny<DateTime>());

        // Assert
        sut.Title.Should().Be(TITLE);
        sut.CreatedDateTime.Should().Be(createdDateTime);
    }

    [Test]
    [Category("CreateFolder")]
    public void CreateFolderWithoutCreatedDateTime()
    {
        // arrange
        var folderDto = new FolderForCreationDto(
            Title: "Folder Title 42",
            CreatedDateTime: null);

        var now = new DateTime(year: 2025, month: 4, day: 24);

        // act
        var sut = Folder.CreateFolder(folderDto, now);

        // assert
        sut.CreatedDateTime.Should().Be(now);
    }

    [Test]
    [Category("CreateFolder")]
    public void CreateFolderWithCreatedDateTimeNotEqualsToNow()
    {
        // arrange
        var createdDateTime = new DateTime(year: 2025, month: 1, day: 1);
        var now = new DateTime(year: 2025, month: 1, day: 2);

        var folderDto = new FolderForCreationDto(
            Title: "Folder Title 42",
            createdDateTime);

        // act
        var sut = Folder.CreateFolder(folderDto, now);

        // assert
        sut.CreatedDateTime.Should().Be(createdDateTime);
    }
    #endregion

    #region Update folder
    [Test]
    [Category("UpdateFolder")]
    public void UpdateFolderWithFieldNormalization()
    {
        // Arrange
        var sut = CreateSut(title: "Folder old title");

        const string NEW_TITLE = "Folder new title";

        var folderDtoWithSpaces = new FolderForUpdateDto(
            Id: It.IsAny<int>(),
            Title: $"   {NEW_TITLE}    ");

        // Act
        sut.UpdateFolder(folderDtoWithSpaces);

        // Assert
        sut.Title.Should().Be(NEW_TITLE);
    }
    #endregion

    #region helpers
    private Folder CreateSut(
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
