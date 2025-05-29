namespace TaskTracker.Tests;

public class UserTaskTests
{
    #region Complete task
    [Fact]
    public void CompleteTask()
    {
        // Arrange
        var sut = CreateSut();
        var completedDateTime = new DateTime(year: 2022, month: 2, day: 10);

        // Act
        sut.Complete(completedDateTime);

        // Assert
        sut.CompletedDateTime.Should().Be(completedDateTime);
        sut.IsCompleted.Should().BeTrue();
        sut.ModifiedDateTime.Should().Be(completedDateTime);
    }
    #endregion

    #region Incomplete task
    [Fact]
    public void IncompleteTask()
    {
        // Arrange
        var sut = CreateSut();
        var completedDateTime = new DateTime(year: 2022, month: 2, day: 10);
        sut.Complete(completedDateTime);

        var modifiedDateTime = new DateTime(year: 2022, month: 2, day: 15);

        // Act
        sut.Incomplete(modifiedDateTime);

        // Assert
        sut.CompletedDateTime.Should().BeNull();
        sut.IsCompleted.Should().BeFalse();
        sut.ModifiedDateTime.Should().Be(modifiedDateTime);
    }
    #endregion

    #region Calculate overdue days
    [Fact]
    public void CalculateOverdueDaysForTaskWithoutDueDateTime()
    {
        // Arrange
        var sut = CreateSut();
        var today = new DateTime(year: 2022, month: 2, day: 7);

        // Act
        var overdueDayCount = sut.CalculateOverdueDays(today);

        // Assert
        sut.DueDateTime.Should().BeNull();
        overdueDayCount.Should().Be(0);
    }

    [Fact]
    public void CalculateOverdueDaysForTaskWithDueDateTime()
    {
        // Arrange
        var sut = CreateSut();

        var dueDateTime = new DateTime(year: 2022, month: 2, day: 5);
        sut.DueDateTime = dueDateTime;

        var today = new DateTime(year: 2022, month: 2, day: 7);

        // Act
        var overdueDayCount = sut.CalculateOverdueDays(today);

        // Assert
        sut.DueDateTime.Should().Be(dueDateTime);
        overdueDayCount.Should().Be(2);
    }

    [Fact]
    public void CalculateOverdueDaysForNonOverdueTask()
    {
        // Arrange
        var sut = CreateSut();

        var dueDateTime = new DateTime(year: 2022, month: 2, day: 5);
        sut.DueDateTime = dueDateTime;

        var today = new DateTime(year: 2022, month: 2, day: 3);

        // Act
        var overdueDayCount = sut.CalculateOverdueDays(today);

        // Assert
        sut.DueDateTime.Should().Be(dueDateTime);
        overdueDayCount.Should().Be(0);
    }

    [Fact]
    public void CalculateOverdueDaysForTodayTask()
    {
        // Arrange
        var sut = CreateSut();

        var dueDateTime = new DateTime(year: 2022, month: 2, day: 5);
        sut.DueDateTime = dueDateTime;

        var today = dueDateTime;

        // Act
        var overdueDayCount = sut.CalculateOverdueDays(today);

        // Assert
        sut.DueDateTime.Should().Be(dueDateTime);
        overdueDayCount.Should().Be(0);
    }

    [Fact]
    public void CalculateOverdueDaysForTaskInTrash()
    {
        // Arrange
        var sut = CreateSut();

        var dueDateTime = new DateTime(year: 2022, month: 2, day: 5);
        sut.DueDateTime = dueDateTime;

        var movedToTrashDateTime = new DateTime(year: 2022, month: 2, day: 6);
        sut.MoveToTrash(movedToTrashDateTime);

        var today = new DateTime(year: 2022, month: 2, day: 7);

        // Act
        var overdueDayCount = sut.CalculateOverdueDays(today);

        // Assert
        sut.DueDateTime.Should().Be(dueDateTime);
        overdueDayCount.Should().Be(0);
    }
    #endregion

    #region Create task
    [Fact]
    public void CreateTaskWithFieldNormalization()
    {
        // Arrange
        const string TITLE = "Task title";
        const string DESCRIPTION = "Task description";
        const int FOLDER_ID = 42;
        var dueDateTime = new DateTime(year: 2025, month: 4, day: 30);
        var createdDateTime = new DateTime(year: 2025, month: 4, day: 20);

        var userTaskDto = new UserTaskForCreationDto(
            Title: $"   {TITLE}    ",
            Description: $"    {DESCRIPTION}    ",
            FolderId: FOLDER_ID,
            DueDateTime: dueDateTime,
            createdDateTime);

        // Act
        var sut = UserTask.CreateTask(userTaskDto, now: It.IsAny<DateTime>());

        // Assert
        sut.Title.Should().Be(TITLE);
        sut.Description.Should().Be(DESCRIPTION);
        sut.FolderId.Should().Be(FOLDER_ID);
        sut.DueDateTime.Should().Be(dueDateTime);
        sut.CreatedDateTime.Should().Be(createdDateTime);
    }

    [Fact]
    public void CreateTaskWithoutCreatedDateTime()
    {
        // Arrange
        var userTaskDto = new UserTaskForCreationDto(
            Title: "Folder Title 42",
            Description: "Folder description 42",
            FolderId: 42,
            DueDateTime: null,
            CreatedDateTime: null);

        var now = new DateTime(year: 2025, month: 4, day: 24);

        // Act
        var sut = UserTask.CreateTask(userTaskDto, now);

        // Assert
        sut.CreatedDateTime.Should().Be(now);
    }

    [Fact]
    public void CreateTaskWithCreatedDateTimeNotEqualsToNow()
    {
        // Arrange
        var createdDateTime = new DateTime(year: 2025, month: 1, day: 1);
        var now = new DateTime(year: 2025, month: 1, day: 2);

        var userTaskDto = new UserTaskForCreationDto(
            Title: "Task Title 42",
            Description: "Task Description 42",
            FolderId: 42,
            DueDateTime: null,
            createdDateTime);

        // Act
        var sut = UserTask.CreateTask(userTaskDto, now);

        // Assert
        sut.CreatedDateTime.Should().Be(createdDateTime);
    }
    #endregion

    #region Update task
    [Fact]
    public void UpdateTaskWithFieldNormalization()
    {
        // Arrange
        var sut = CreateSut(
            title: "Old task title",
            description: "Old task description",
            folderId: 1,
            dueDateTime: new DateTime(year: 2025, month: 1, day: 1)
        );

        const string NEW_TITLE = "New task title";
        const string NEW_DESCRIPTION = "New task description";
        const int NEW_FOLDER_ID = 2;
        var newDueDateTime = new DateTime(year: 2025, month: 1, day: 2);
        var modifiedDateTime =  new DateTime(year: 2025, month: 1, day: 3);

        var userTaskDto = new UserTaskForUpdateDto(
            Title: $"   {NEW_TITLE}    ",
            Description: $"   {NEW_DESCRIPTION}    ",
            NEW_FOLDER_ID,
            newDueDateTime,
            modifiedDateTime
        );

        // Act
        sut.UpdateTask(userTaskDto, modifiedDateTime);

        // Assert
        sut.Title.Should().Be(NEW_TITLE);
        sut.Description.Should().Be(NEW_DESCRIPTION);
        sut.FolderId.Should().Be(NEW_FOLDER_ID);
        sut.DueDateTime.Should().Be(newDueDateTime);
        sut.ModifiedDateTime.Should().Be(modifiedDateTime);
    }

    [Fact]
    public void UpdateTaskWithoutModifiedDateTime()
    {
        // Arrange
        var sut = CreateSut();

        var now = new DateTime(year: 2025, month: 5, day: 1);

        var userTaskDto = new UserTaskForUpdateDto(
            Title: "Task title 42",
            Description: "Task description 42",
            FolderId: 42,
            DueDateTime: new DateTime(),
            ModifiedDateTime: null);

        // Act
        sut.UpdateTask(userTaskDto, now);

        // Assert
        sut.ModifiedDateTime.Should().Be(now);
    }

    [Fact]
    public void UpdateTaskWithModifiedDateTimeNotEqualsToNow()
    {
        // Arrange
        var modifiedDateTime = new DateTime(year: 2025, month: 1, day: 1);
        var now = new DateTime(year: 2025, month: 1, day: 2);

        var sut = CreateSut();

        var userTaskDto = new UserTaskForUpdateDto(
            Title: "Task title 42",
            Description: "Task description 42",
            FolderId: 42,
            DueDateTime: new DateTime(),
            modifiedDateTime);

        // Act
        sut.UpdateTask(userTaskDto, now);

        // Assert
        sut.ModifiedDateTime.Should().Be(modifiedDateTime);
    }
    #endregion

    #region Move task to trash
    [Fact]
     public void MoveTaskWithoutFolderToTrash()
     {
         // Arrange
         var sut = CreateSut(folderId: null);
         var movedToTrashDateTime = new DateTime(year: 2025, month: 5, day: 5);

         // Act
         sut.MoveToTrash(movedToTrashDateTime);

         // Assert
         sut.MovedToTrashDateTime.Should().Be(movedToTrashDateTime);
         sut.IsInTrash.Should().BeTrue();
         sut.ModifiedDateTime.Should().Be(movedToTrashDateTime);
         sut.FolderId.Should().BeNull();
     }

    [Fact]
    public void MoveTaskWithFolderToTrash()
    {
        // Arrange
        var sut = CreateSut(folderId: 42);
        var movedToTrashDateTime = new DateTime(year: 2025, month: 5, day: 5);

        // Act
        sut.MoveToTrash(movedToTrashDateTime);

        // Assert
        sut.MovedToTrashDateTime.Should().Be(movedToTrashDateTime);
        sut.IsInTrash.Should().BeTrue();
        sut.ModifiedDateTime.Should().Be(movedToTrashDateTime);
        sut.FolderId.Should().BeNull();
    }

    [Fact]
    public void MoveTaskToTrashThatIsInTrashAlready()
    {
        // Arrange
        var sut = CreateSut();
        var oldMovedToTrashDateTime = new DateTime(year: 2025, month: 5, day: 1);
        var newMovedToTrashDateTime = new DateTime(year: 2025, month: 5, day: 2);

        // Act
        sut.MoveToTrash(oldMovedToTrashDateTime);
        sut.MoveToTrash(newMovedToTrashDateTime);

        // Assert
        sut.MovedToTrashDateTime.Should().Be(oldMovedToTrashDateTime);
        sut.IsInTrash.Should().BeTrue();
        sut.ModifiedDateTime.Should().Be(oldMovedToTrashDateTime);
        sut.FolderId.Should().BeNull();
    }
    #endregion

    #region Move task from trash
    [Fact]
    public void MoveTaskFromTrash()
    {
        // Arrange
        var sut = CreateSut();

        var movedToTrashDateTime = new DateTime(year: 2025, month: 5, day: 1);
        var movedFromTrashDateTime = new DateTime(year: 2025, month: 5, day: 2);

        sut.MoveToTrash(movedToTrashDateTime);

        // Act
        sut.MoveFromTrash(movedFromTrashDateTime);

        // Assert
        sut.MovedToTrashDateTime.Should().BeNull();
        sut.IsInTrash.Should().BeFalse();
        sut.ModifiedDateTime.Should().Be(movedFromTrashDateTime);
    }

    [Fact]
    public void MoveTaskFromTrashThatIsNotInTrashAlready()
    {
        // Arrange
        var sut = CreateSut();

        var movedToTrashDateTime = new DateTime(year: 2025, month: 5, day: 1);
        var oldMovedFromTrashDateTime = new DateTime(year: 2025, month: 5, day: 2);
        var newMovedFromTrashDateTime = new DateTime(year: 2025, month: 5, day: 3);

        sut.MoveToTrash(movedToTrashDateTime);

        // Act
        sut.MoveFromTrash(oldMovedFromTrashDateTime);
        sut.MoveFromTrash(newMovedFromTrashDateTime);

        // Assert
        sut.MovedToTrashDateTime.Should().BeNull();
        sut.IsInTrash.Should().BeFalse();
        sut.ModifiedDateTime.Should().Be(oldMovedFromTrashDateTime);
    }
    #endregion

    #region helpers
    private UserTask CreateSut(
        string title = "Task title 42",
        string? description = null,
        int? folderId = null,
        DateTime? dueDateTime = null,
        DateTime? createdDateTime = null,
        DateTime? now = null)
    {
        createdDateTime ??= new DateTime(year: 2025, month: 1, day: 1);
        now ??= new DateTime(year: 2025, month: 1, day: 2);

        var userTaskDto = new UserTaskForCreationDto(
            title,
            description,
            folderId,
            dueDateTime,
            createdDateTime);

        return UserTask.CreateTask(userTaskDto, now.Value);
    }
    #endregion
}
