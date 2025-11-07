namespace TaskTracker.UnitTests.Tests;

public class UserTaskUnitTests
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
        sut.CompletedDateTime.ShouldBe(completedDateTime);
        sut.ModifiedDateTime.ShouldBe(completedDateTime);
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
        sut.CompletedDateTime.ShouldBeNull();
        sut.ModifiedDateTime.ShouldBe(modifiedDateTime);
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
        sut.DueDateTime.ShouldBeNull();
        overdueDayCount.ShouldBe(0);
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
        sut.DueDateTime.ShouldBe(dueDateTime);
        overdueDayCount.ShouldBe(2);
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
        sut.DueDateTime.ShouldBe(dueDateTime);
        overdueDayCount.ShouldBe(0);
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
        sut.DueDateTime.ShouldBe(dueDateTime);
        overdueDayCount.ShouldBe(0);
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
        sut.DueDateTime.ShouldBe(dueDateTime);
        overdueDayCount.ShouldBe(0);
    }
    #endregion

    #region Create task
    [Fact]
    public void CreateTaskWithFieldNormalization()
    {
        // Arrange
        const string TITLE = "Task title";
        const int FOLDER_ID = 42;
        var dueDateTime = new DateTime(year: 2025, month: 4, day: 30);
        var createdDateTime = new DateTime(year: 2025, month: 4, day: 20);

        var userTaskDto = new UserTaskForCreationDto(
            Title: $"   {TITLE}    ",
            FolderId: FOLDER_ID,
            DueDateTime: dueDateTime,
            createdDateTime);

        var anyDateTime = new DateTime();

        // Act
        var sut = UserTask.CreateTask(userTaskDto, now: anyDateTime);

        // Assert
        sut.Title.ShouldBe(TITLE);
        sut.FolderId.ShouldBe(FOLDER_ID);
        sut.DueDateTime.ShouldBe(dueDateTime);
        sut.CreatedDateTime.ShouldBe(createdDateTime);
        sut.ModifiedDateTime.ShouldBeNull();
    }

    [Fact]
    public void CreateTaskWithoutTitle()
    {
        // Arrange
        var userTaskDto = new UserTaskForCreationDto(
            Title: "   \t   \n   ",
            FolderId: null,
            DueDateTime: null,
            CreatedDateTime: null);

        var anyDateTime = new DateTime();

        // Act
        var action = () => UserTask.CreateTask(userTaskDto, now: anyDateTime);

        // Assert
        Should.Throw<DomainException>(action)
            .Message.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public void CreateTaskWithoutCreatedDateTime()
    {
        // Arrange
        var userTaskDto = new UserTaskForCreationDto(
            Title: "Folder Title 42",
            FolderId: 42,
            DueDateTime: null,
            CreatedDateTime: null);

        var now = new DateTime(year: 2025, month: 4, day: 24);

        // Act
        var sut = UserTask.CreateTask(userTaskDto, now);

        // Assert
        sut.CreatedDateTime.ShouldBe(now);
    }

    [Fact]
    public void CreateTaskWithCreatedDateTimeNotEqualsToNow()
    {
        // Arrange
        var createdDateTime = new DateTime(year: 2025, month: 1, day: 1);
        var now = new DateTime(year: 2025, month: 1, day: 2);

        var userTaskDto = new UserTaskForCreationDto(
            Title: "Task Title 42",
            FolderId: 42,
            DueDateTime: null,
            createdDateTime);

        // Act
        var sut = UserTask.CreateTask(userTaskDto, now);

        // Assert
        sut.CreatedDateTime.ShouldBe(createdDateTime);
    }
    #endregion

    #region Update task
    [Fact]
    public void UpdateTaskWithFieldNormalization()
    {
        // Arrange
        var sut = CreateSut(
            title: "Old task title",
            folderId: 1,
            dueDateTime: new DateTime(year: 2025, month: 1, day: 1));

        const string NEW_TITLE = "New task title";
        const string DESCRIPTION = "   Task description    ";
        const int NEW_FOLDER_ID = 2;
        var newDueDateTime = new DateTime(year: 2025, month: 1, day: 2);
        var modifiedDateTime =  new DateTime(year: 2025, month: 1, day: 3);

        var userTaskDto = new UserTaskForUpdateDto(
            Title: $"   {NEW_TITLE}    ",
            Description: DESCRIPTION,
            NEW_FOLDER_ID,
            TagIds: null,
            newDueDateTime,
            modifiedDateTime);

        // Act
        sut.UpdateTask(userTaskDto, modifiedDateTime, tags: []);

        // Assert
        sut.Title.ShouldBe(NEW_TITLE);
        sut.Description.ShouldBe(DESCRIPTION);
        sut.FolderId.ShouldBe(NEW_FOLDER_ID);
        sut.Tags.ShouldBeEmpty();
        sut.DueDateTime.ShouldBe(newDueDateTime);
        sut.ModifiedDateTime.ShouldBe(modifiedDateTime);
    }

    [Fact]
    public void UpdateTaskWithoutTitle()
    {
        // Arrange
        const string OLD_TITLE = "Old task title";
        const int OLD_FOLDER_ID = 1;
        var oldDueDateTime = new DateTime(year: 2025, month: 1, day: 1);

        var sut = CreateSut(OLD_TITLE, OLD_FOLDER_ID, oldDueDateTime);

        var userTaskDto = new UserTaskForUpdateDto(
            Title: "   \t   \n   ",
            Description: "Task description",
            FolderId: 2,
            TagIds: [],
            DueDateTime: new DateTime(year: 2025, month: 1, day: 2),
            ModifiedDateTime: null);

        var anyDateTime = new DateTime();

        // Act
        var action = () => sut.UpdateTask(userTaskDto, now: anyDateTime, tags: []);

        // Assert
        Should.Throw<DomainException>(action)
            .Message.ShouldNotBeNullOrWhiteSpace();

        sut.Title.ShouldBe(OLD_TITLE);
        sut.Description.ShouldBeNull();
        sut.FolderId.ShouldBe(OLD_FOLDER_ID);
        sut.Tags.ShouldBeNull();
        sut.DueDateTime.ShouldBe(oldDueDateTime);
        sut.ModifiedDateTime.ShouldBeNull();
    }

    [Fact]
    public void UpdateTaskWithoutModifiedDateTime()
    {
        // Arrange
        var sut = CreateSut();

        var now = new DateTime(year: 2025, month: 5, day: 1);

        var userTaskDto = new UserTaskForUpdateDto(
            Title: "Task title 42",
            Description: null,
            FolderId: null,
            TagIds: null,
            DueDateTime: new DateTime(),
            ModifiedDateTime: null);

        // Act
        sut.UpdateTask(userTaskDto, now, tags: []);

        // Assert
        sut.ModifiedDateTime.ShouldBe(now);
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
            Description: null,
            FolderId: null,
            TagIds: null,
            DueDateTime: new DateTime(),
            modifiedDateTime);

        // Act
        sut.UpdateTask(userTaskDto, now, tags: []);

        // Assert
        sut.ModifiedDateTime.ShouldBe(modifiedDateTime);
    }

    [Fact]
    public void UpdateTaskAddTags()
    {
        // Arrange
        var sut = CreateSut();

        const string TAG_1_NAME = "Tag 1";
        var tag1 = CreateTag(TAG_1_NAME);

        const string TAG_2_NAME = "Tag 2";
        var tag2 = CreateTag(TAG_2_NAME);

        var tags = new List<Tag> { tag1, tag2 };

        var userTaskDto = new UserTaskForUpdateDto(
            Title: "Task title 42",
            Description: null,
            FolderId: null,
            TagIds: null,
            DueDateTime: null,
            ModifiedDateTime: null);

        var anyDateTime = new DateTime();

        // Act
        sut.UpdateTask(userTaskDto, now: anyDateTime, tags);

        // Assert
        sut.Tags.ShouldBeEquivalentTo(tags);
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
         sut.MovedToTrashDateTime.ShouldBe(movedToTrashDateTime);
         sut.IsInTrash.ShouldBeTrue();
         sut.ModifiedDateTime.ShouldBe(movedToTrashDateTime);
         sut.FolderId.ShouldBeNull();
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
        sut.MovedToTrashDateTime.ShouldBe(movedToTrashDateTime);
        sut.IsInTrash.ShouldBeTrue();
        sut.ModifiedDateTime.ShouldBe(movedToTrashDateTime);
        sut.FolderId.ShouldBeNull();
    }

    [Fact]
    public void MoveTaskToTrashThatIsInTrashAlready()
    {
        // Arrange
        var sut = CreateSut();
        var oldMovedToTrashDateTime = new DateTime(year: 2025, month: 5, day: 1);
        var newMovedToTrashDateTime = new DateTime(year: 2025, month: 5, day: 2);

        sut.MoveToTrash(oldMovedToTrashDateTime);

        // Act
        var action = () => sut.MoveToTrash(newMovedToTrashDateTime);

        // Assert
        Should.Throw<DomainException>(action)
            .Message.ShouldNotBeNullOrWhiteSpace();

        sut.MovedToTrashDateTime.ShouldBe(oldMovedToTrashDateTime);
        sut.IsInTrash.ShouldBeTrue();
        sut.ModifiedDateTime.ShouldBe(oldMovedToTrashDateTime);
        sut.FolderId.ShouldBeNull();
    }
    #endregion

    #region Move task from trash
    [Fact]
    public void MoveTaskFromTrashToInbox()
    {
        // Arrange
        var sut = CreateSut();

        var movedToTrashDateTime = new DateTime(year: 2025, month: 5, day: 1);
        var movedFromTrashDateTime = new DateTime(year: 2025, month: 5, day: 2);

        sut.MoveToTrash(movedToTrashDateTime);

        // Act
        sut.MoveFromTrash(movedFromTrashDateTime, folderId: null);

        // Assert
        sut.MovedToTrashDateTime.ShouldBeNull();
        sut.IsInTrash.ShouldBeFalse();
        sut.ModifiedDateTime.ShouldBe(movedFromTrashDateTime);
        sut.FolderId.ShouldBeNull();
    }

    [Fact]
    public void MoveTaskFromTrashToSpecificFolder()
    {
        // Arrange
        var sut = CreateSut();

        var movedToTrashDateTime = new DateTime(year: 2025, month: 5, day: 1);
        var movedFromTrashDateTime = new DateTime(year: 2025, month: 5, day: 2);

        const int NEW_FOLDER_ID = 1;

        sut.MoveToTrash(movedToTrashDateTime);

        // Act
        sut.MoveFromTrash(movedFromTrashDateTime, NEW_FOLDER_ID);

        // Assert
        sut.MovedToTrashDateTime.ShouldBeNull();
        sut.IsInTrash.ShouldBeFalse();
        sut.ModifiedDateTime.ShouldBe(movedFromTrashDateTime);
        sut.FolderId.ShouldBe(NEW_FOLDER_ID);
    }

    [Fact]
    public void MoveTaskFromTrashThatIsNotInTrashAlready()
    {
        // Arrange
        var sut = CreateSut();

        var movedToTrashDateTime = new DateTime(year: 2025, month: 5, day: 1);
        var firstMovedFromTrashDateTime = new DateTime(year: 2025, month: 5, day: 2);
        var secondMovedFromTrashDateTime = new DateTime(year: 2025, month: 5, day: 3);

        const int FIRST_FOLDER_ID = 1;
        const int SECOND_FOLDER_ID = 2;

        sut.MoveToTrash(movedToTrashDateTime);
        sut.MoveFromTrash(firstMovedFromTrashDateTime, FIRST_FOLDER_ID);

        // Act
       var action = ()
           => sut.MoveFromTrash(secondMovedFromTrashDateTime, SECOND_FOLDER_ID);

        // Assert
        Should.Throw<DomainException>(action)
            .Message.ShouldNotBeNullOrWhiteSpace();

        sut.MovedToTrashDateTime.ShouldBeNull();
        sut.IsInTrash.ShouldBeFalse();
        sut.ModifiedDateTime.ShouldBe(firstMovedFromTrashDateTime);
        sut.FolderId.ShouldBe(FIRST_FOLDER_ID);
    }
    #endregion

    #region helpers
    private UserTask CreateSut(
        string title = "Task title 42",
        int? folderId = null,
        DateTime? dueDateTime = null,
        DateTime? createdDateTime = null,
        DateTime? now = null)
    {
        createdDateTime ??= new DateTime(year: 2025, month: 1, day: 1);
        now ??= new DateTime(year: 2025, month: 1, day: 2);

        var userTaskDto = new UserTaskForCreationDto(
            title,
            folderId,
            dueDateTime,
            createdDateTime);

        var task = UserTask.CreateTask(userTaskDto, now.Value);
        return task;
    }

    private Tag CreateTag(
        string title = "Tag title 42",
        string color = "#424242",
        DateTime? createdDateTime = null,
        DateTime? now = null)
    {
        createdDateTime ??= new DateTime(year: 2025, month: 1, day: 1);
        now ??= new DateTime(year: 2025, month: 1, day: 2);

        var tagDto = new TagForCreationDto(title, color, createdDateTime);

        var tag = Tag.CreateTag(tagDto, now.Value);
        return tag;
    }
    #endregion
}
