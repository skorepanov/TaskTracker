namespace TaskTracker.Tests;

public class UserTaskTests
{
    #region Complete task
    [Test]
    [Category("CompleteTask")]
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
    }
    #endregion

    #region Incomplete task
    [Test]
    [Category("IncompleteTask")]
    public void IncompleteTask()
    {
        // Arrange
        var sut = CreateSut();
        var completedDateTime = new DateTime(year: 2022, month: 2, day: 10);
        sut.Complete(completedDateTime);

        // Act
        sut.Incomplete();

        // Assert
        sut.CompletedDateTime.Should().BeNull();
        sut.IsCompleted.Should().BeFalse();
    }
    #endregion

    #region Calculate overdue days
    [Test]
    [Category("CalculateOverdueDays")]
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

    [Test]
    [Category("CalculateOverdueDays")]
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

    [Test]
    [Category("CalculateOverdueDays")]
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

    [Test]
    [Category("CalculateOverdueDays")]
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

    [Test]
    [Category("CalculateOverdueDays")]
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
    [Test]
    [Category("IsTodayTask")]
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

    [Test]
    [Category("IsTodayTask")]
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

    [Test]
    [Category("IsTodayTask")]
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

    [Test]
    [Category("IsTodayTask")]
    [TestCaseSource(nameof(GetTestCasesForIsTodayTaskWithDueDateTime))]
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

    private static IEnumerable<TestCaseData> GetTestCasesForIsTodayTaskWithDueDateTime()
    {
        yield return new TestCaseData(new DateTime(year: 2022, month: 2, day: 6),
                                      new DateTime(year: 2022, month: 2, day: 7),
                                      true);

        yield return new TestCaseData(new DateTime(year: 2022, month: 2, day: 7),
                                      new DateTime(year: 2022, month: 2, day: 7),
                                      true);

        yield return new TestCaseData(new DateTime(year: 2022, month: 2, day: 8),
                                      new DateTime(year: 2022, month: 2, day: 7),
                                      false);
    }

    [Test]
    [Category("IsTodayTask")]
    [TestCaseSource(nameof(GetTestCasesForIsTodayDeletedTask))]
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

    private static IEnumerable<TestCaseData> GetTestCasesForIsTodayDeletedTask()
    {
        yield return new TestCaseData(new DateTime(year: 2022, month: 2, day: 6),
                                      new DateTime(year: 2022, month: 2, day: 7));

        yield return new TestCaseData(new DateTime(year: 2022, month: 2, day: 7),
                                      new DateTime(year: 2022, month: 2, day: 7));
    }
    #endregion

    #region Create task
    [Test]
    [Category("CreateTask")]
    public void CreateTaskWithFieldNormalization()
    {
        // Arrange
        const string TITLE = "Task title";
        const string DESCRIPTION = "Task description";
        const int FOLDER_ID = 42;
        var dueDateTime = new DateTime(year: 2022, month: 2, day: 23);

        var userTaskDtoWithSpaces = new UserTaskForCreationDto(
            Title: $"   {TITLE}    ",
            Description: $"    {DESCRIPTION}    ",
            FolderId: FOLDER_ID,
            DueDateTime: dueDateTime
        );

        // Act
        var sut = UserTask.CreateTask(userTaskDtoWithSpaces);

        // Assert
        sut.Title.Should().Be(TITLE);
        sut.Description.Should().Be(DESCRIPTION);
        sut.FolderId.Should().Be(FOLDER_ID);
        sut.DueDateTime.Should().Be(dueDateTime);
    }
    #endregion

    #region Update task
    [Test]
    [Category("UpdateTask")]
    public void UpdateTaskWithFieldNormalization()
    {
        // Arrange
        var sut = CreateSut(
            title: "Task old title",
            description: "Task old description",
            folderId: 42_1,
            dueDateTime: new DateTime(year: 2025, month: 1, day: 1)
        );

        const string NEW_TITLE = "Task new title";
        const string NEW_DESCRIPTION = "Task new description";
        const int NEW_FOLDER_ID = 42_2;
        var newDueDateTime = new DateTime(year: 2025, month: 1, day: 2);

        var userTaskDtoWithSpaces = new UserTaskForUpdateDto(
            Title: $"   {NEW_TITLE}    ",
            Description: $"   {NEW_DESCRIPTION}    ",
            FolderId: NEW_FOLDER_ID,
            DueDateTime: newDueDateTime
        );

        // Act
        sut.UpdateTask(userTaskDtoWithSpaces);

        // Assert
        sut.Title.Should().Be(NEW_TITLE);
        sut.Description.Should().Be(NEW_DESCRIPTION);
        sut.FolderId.Should().Be(NEW_FOLDER_ID);
        sut.DueDateTime.Should().Be(newDueDateTime);
    }
    #endregion

    #region Delete task
    [Test]
    [Category("DeleteTask")]
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
    }
    #endregion

    #region helpers
    private UserTask CreateSut(
        string title = "Task title 42",
        string description = "Description 42",
        int? folderId = 42,
        DateTime? dueDateTime = null)
    {
        var userTaskDto = new UserTaskForCreationDto(
            title, description, folderId, dueDateTime);

        return UserTask.CreateTask(userTaskDto);
    }
    #endregion
}
