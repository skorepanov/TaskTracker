namespace TaskTracker.Tests;

public class FolderTests
{
    #region Add task to folder
    [Fact]
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

    [Fact]
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

    #region Create folder
    [Fact]
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
        sut.ModifiedDateTime.Should().BeNull();
    }

    [Fact]
    public void CreateFolderWithoutCreatedDateTime()
    {
        // Arrange
        var folderDto = new FolderForCreationDto(
            Title: "Folder Title 42",
            CreatedDateTime: null);

        var now = new DateTime(year: 2025, month: 4, day: 24);

        // Act
        var sut = Folder.CreateFolder(folderDto, now);

        // Assert
        sut.CreatedDateTime.Should().Be(now);
    }

    [Fact]
    public void CreateFolderWithCreatedDateTimeNotEqualsToNow()
    {
        // Arrange
        var createdDateTime = new DateTime(year: 2025, month: 1, day: 1);
        var now = new DateTime(year: 2025, month: 1, day: 2);

        var folderDto = new FolderForCreationDto(
            Title: "Folder Title 42",
            createdDateTime);

        // Act
        var sut = Folder.CreateFolder(folderDto, now);

        // Assert
        sut.CreatedDateTime.Should().Be(createdDateTime);
    }
    #endregion

    #region Update folder
    [Fact]
    public void UpdateFolderWithFieldNormalization()
    {
        // Arrange
        var sut = CreateSut(title: "Old folder title");

        const string NEW_TITLE = "New folder title";
        var modifiedDateTime = new DateTime(year: 2025, month: 4, day: 28);

        var folderDto = new FolderForUpdateDto(
            Title: $"   {NEW_TITLE}    ",
            modifiedDateTime);

        // Act
        sut.UpdateFolder(folderDto, modifiedDateTime);

        // Assert
        sut.Title.Should().Be(NEW_TITLE);
        sut.ModifiedDateTime.Should().Be(modifiedDateTime);
    }

    [Fact]
    public void UpdateFolderWithoutModifiedDateTime()
    {
        // Arrange
        var sut = CreateSut();

        var now = new DateTime(year: 2025, month: 4, day: 28);

        var folderDto = new FolderForUpdateDto(
            Title: "Folder title 42",
            ModifiedDateTime: null);

        // Act
        sut.UpdateFolder(folderDto, now);

        // Assert
        sut.ModifiedDateTime.Should().Be(now);
    }

    [Fact]
    public void UpdateFolderWithModifiedDateTimeNotEqualsToNow()
    {
        // Arrange
        var modifiedDateTime = new DateTime(year: 2025, month: 1, day: 1);
        var now = new DateTime(year: 2025, month: 1, day: 2);

        var sut = CreateSut();

        var folderDto = new FolderForUpdateDto(
            Title: "Folder title 42",
            modifiedDateTime);

        // Act
        sut.UpdateFolder(folderDto, now);

        // Assert
        sut.ModifiedDateTime.Should().Be(modifiedDateTime);
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
        string? description = null,
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
