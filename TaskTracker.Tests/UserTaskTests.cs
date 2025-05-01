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
    public void CalculateOverdueDaysForDeletedTask()
    {
        // Arrange
        var sut = CreateSut();

        var dueDateTime = new DateTime(year: 2022, month: 2, day: 5);
        sut.DueDateTime = dueDateTime;

        var deletionDate = new DateTime(year: 2022, month: 2, day: 6);
        sut.Delete(deletionDate);

        var today = new DateTime(year: 2022, month: 2, day: 7);

        // Act
        var overdueDayCount = sut.CalculateOverdueDays(today);

        // Assert
        sut.DueDateTime.Should().Be(dueDateTime);
        overdueDayCount.Should().Be(0);
    }
    #endregion

    #region Is today task
    [Fact]
    public void IsTodayTaskThatCompletedToday()
    {
        // Arrange
        var today = new DateTime(year: 2022, month: 2, day: 5);

        var sut = CreateSut();
        var completedDateTime = today;
        sut.Complete(completedDateTime);

        // Act
        var isTodayTask = sut.IsTodayTask(today);

        // Assert
        isTodayTask.Should().BeTrue();
    }

    [Fact]
    public void IsTodayTaskThatCompletedEarlier()
    {
        // Arrange
        var sut = CreateSut();
        var completedDateTime = new DateTime(year: 2022, month: 2, day: 5);
        sut.Complete(completedDateTime);

        var today = new DateTime(year: 2022, month: 2, day: 7);

        // Act
        var isTodayTask = sut.IsTodayTask(today);

        // Assert
        isTodayTask.Should().BeFalse();
    }

    [Fact]
    public void IsTodayIncompleteTaskWithoutDueDateTime()
    {
        // Arrange
        var sut = CreateSut();
        var today = new DateTime(year: 2022, month: 2, day: 7);

        // Act
        var isTodayTask = sut.IsTodayTask(today);

        // Assert
        isTodayTask.Should().BeFalse();
    }

    public static TheoryData<DateTime, DateTime, bool> IsTodayTaskWithDueDateTimeCases
        = new()
        {
            {
                new DateTime(year: 2022, month: 2, day: 6),
                new DateTime(year: 2022, month: 2, day: 7),
                true
            },
            {
                new DateTime(year: 2022, month: 2, day: 7),
                new DateTime(year: 2022, month: 2, day: 7),
                true
            },
            {
                new DateTime(year: 2022, month: 2, day: 8),
                new DateTime(year: 2022, month: 2, day: 7),
                false
            }
        };

    [Theory, MemberData(nameof(IsTodayTaskWithDueDateTimeCases))]
    public void IsTodayIncompleteTaskWithDueDateTime(
        DateTime dueDateTime, DateTime today, bool expectedResult)
    {
        // Arrange
        var sut = CreateSut();
        sut.DueDateTime = dueDateTime;

        // Act
        var isTodayTask = sut.IsTodayTask(today);

        // Assert
        isTodayTask.Should().Be(expectedResult);
    }

    public static TheoryData<DateTime, DateTime> IsTodayDeletedTaskCases
        = new()
        {
            {
                new DateTime(year: 2022, month: 2, day: 6),
                new DateTime(year: 2022, month: 2, day: 7)
            },
            {
                new DateTime(year: 2022, month: 2, day: 7),
                new DateTime(year: 2022, month: 2, day: 7)
            }
        };

    [Theory, MemberData(nameof(IsTodayDeletedTaskCases))]
    public void IsTodayDeletedTask(DateTime deletionDate, DateTime today)
    {
        // Arrange
        var sut = CreateSut();
        sut.DueDateTime = today;
        sut.Delete(deletionDate);

        // Act
        var isTodayTask = sut.IsTodayTask(today);

        // Assert
        isTodayTask.Should().BeFalse();
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
        // arrange
        var userTaskDto = new UserTaskForCreationDto(
            Title: "Folder Title 42",
            Description: "Folder description 42",
            FolderId: 42,
            DueDateTime: null,
            CreatedDateTime: null);

        var now = new DateTime(year: 2025, month: 4, day: 24);

        // act
        var sut = UserTask.CreateTask(userTaskDto, now);

        // assert
        sut.CreatedDateTime.Should().Be(now);
    }

    [Fact]
    public void CreateTaskWithCreatedDateTimeNotEqualsToNow()
    {
        // arrange
        var createdDateTime = new DateTime(year: 2025, month: 1, day: 1);
        var now = new DateTime(year: 2025, month: 1, day: 2);

        var userTaskDto = new UserTaskForCreationDto(
            Title: "Task Title 42",
            Description: "Task Description 42",
            FolderId: 42,
            DueDateTime: null,
            createdDateTime);

        // act
        var sut = UserTask.CreateTask(userTaskDto, now);

        // assert
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

    #region Delete task
    [Fact]
    public void DeleteTask()
    {
        // Arrange
        var sut = CreateSut();
        var deletionDate = new DateTime(year: 2022, month: 2, day: 10);

        // Act
        sut.Delete(deletionDate);

        // Assert
        sut.DeletionDate.Should().Be(deletionDate);
        sut.IsDeleted.Should().BeTrue();
        sut.ModifiedDateTime.Should().Be(deletionDate);
    }
    #endregion

    #region helpers
    private UserTask CreateSut(
        string title = "Task title 42",
        string description = "Description 42",
        int? folderId = 42,
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
