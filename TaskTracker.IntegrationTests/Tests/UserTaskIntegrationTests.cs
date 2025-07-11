namespace TaskTracker.IntegrationTests.Tests;

public class UserTaskIntegrationTests(ApiWebApplicationFactory factory)
    : IntegrationTestBase(factory)
{
    [Fact]
    public async Task GetTaskByIdWhenTaskExists()
    {
        // Arrange
        var folder = await CreateFolderInDatabase();

        var dueDateTime = new DateTime(
            year: 2025, month: 7, day: 1, hour: 1, minute: 1, second: 1,
            DateTimeKind.Utc);
        var createdDateTime = new DateTime(
            year: 2025, month: 7, day: 1, hour: 1, minute: 1, second: 2,
            DateTimeKind.Utc);
        var task = await CreateTaskInDatabase(
            title: "Task title", folder.Id, dueDateTime, createdDateTime);

        var otherTask = await CreateTaskInDatabase();

        var utcNow = new DateTime(
            year: 2025, month: 7, day: 2, hour: 1, minute: 1, second: 1,
            DateTimeKind.Utc);
        MockDateTimeProvider.Setup(p => p.UtcNow).Returns(utcNow);

        // Act
        var response = await Client.GetAsync(requestUri: $"/api/tasks/{task.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Result<UserTaskVm>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeTrue();
        content.Error.Should().BeNull();

        var responseTask = content.Value;
        responseTask.Should().NotBeNull();
        responseTask.Id.Should().Be(task.Id);
        responseTask.Title.Should().Be(task.Title);
        responseTask.Description.Should().BeNull();
        responseTask.FolderId.Should().Be(task.FolderId);
        responseTask.CompletedDateTime.Should().BeNull();
        responseTask.DueDateTime.Should().Be(task.DueDateTime);
        responseTask.MovedToTrashDateTime.Should().BeNull();
        responseTask.TagIds.Should().BeNull();
        responseTask.CreatedDateTime.Should().Be(task.CreatedDateTime);
        responseTask.ModifiedDateTime.Should().BeNull();
    }

    [Fact]
    public async Task GetTaskByIdWhenTaskNotExists()
    {
        // Arrange
        const int NON_EXISTENT_TASK_ID = 1;

        // Act
        var response = await Client.GetAsync(
            requestUri: $"/api/tasks/{NON_EXISTENT_TASK_ID}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Result<UserTaskVm>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeFalse();
        content.Error.Should().NotBeNullOrWhiteSpace();
        content.Value.Should().BeNull();
    }

    [Fact]
    public async Task GetIncompletedTasks()
    {
        // Arrange
        var folder = await CreateFolderInDatabase();

        var incompletedTask = await CreateIncompletedTaskInDatabase(folder.Id);
        var completedTask = await CreateCompletedTaskInDatabase(folder.Id);
        var taskInTrash = await CreateTaskInTrashInDatabase();

        // Act
        var response = await Client.GetAsync(requestUri: $"/api/tasks/incomplete");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content
            .ReadFromJsonAsync<Result<IReadOnlyList<UserTaskVm>>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeTrue();
        content.Error.Should().BeNull();
        content.Value.Should().NotBeNull().And.HaveCount(1);

        var responseTask = content.Value.Single();
        responseTask.Should().NotBeNull();
        responseTask.Id.Should().Be(incompletedTask.Id);
        responseTask.Title.Should().Be(incompletedTask.Title);
        responseTask.Description.Should().BeNull();
        responseTask.FolderId.Should().Be(incompletedTask.FolderId);
        responseTask.CompletedDateTime.Should().BeNull();
        responseTask.DueDateTime.Should().Be(incompletedTask.DueDateTime);
        responseTask.MovedToTrashDateTime.Should().BeNull();
        responseTask.TagIds.Should().BeNull();
        responseTask.CreatedDateTime.Should().Be(incompletedTask.CreatedDateTime);
        responseTask.ModifiedDateTime.Should().BeNull();
    }

    [Fact]
    public async Task GetCompletedTasks()
    {
        // Arrange
        var folder = await CreateFolderInDatabase();

        var incompletedTask = await CreateIncompletedTaskInDatabase(folder.Id);
        var completedTask = await CreateCompletedTaskInDatabase(folder.Id);
        var taskInTrash = await CreateTaskInTrashInDatabase();

        // Act
        var response = await Client.GetAsync(requestUri: $"/api/tasks/complete");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content
            .ReadFromJsonAsync<Result<IReadOnlyList<UserTaskVm>>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeTrue();
        content.Error.Should().BeNull();
        content.Value.Should().NotBeNull().And.HaveCount(1);

        var responseTask = content.Value.Single();
        responseTask.Should().NotBeNull();
        responseTask.Id.Should().Be(completedTask.Id);
        responseTask.Title.Should().Be(completedTask.Title);
        responseTask.Description.Should().BeNull();
        responseTask.FolderId.Should().Be(completedTask.FolderId);
        responseTask.CompletedDateTime.Should().Be(completedTask.CompletedDateTime);
        responseTask.DueDateTime.Should().Be(completedTask.DueDateTime);
        responseTask.MovedToTrashDateTime.Should().BeNull();
        responseTask.TagIds.Should().BeNull();
        responseTask.CreatedDateTime.Should().Be(completedTask.CreatedDateTime);
        responseTask.ModifiedDateTime.Should().Be(completedTask.ModifiedDateTime);
    }

    [Fact]
    public async Task GetTasksInTrash()
    {
        // Arrange
        var folder = await CreateFolderInDatabase();

        var incompletedTask = await CreateIncompletedTaskInDatabase(folder.Id);
        var completedTask = await CreateCompletedTaskInDatabase(folder.Id);
        var taskInTrash = await CreateTaskInTrashInDatabase();

        // Act
        var response = await Client.GetAsync(requestUri: $"/api/tasks/trash");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content
            .ReadFromJsonAsync<Result<IReadOnlyList<UserTaskVm>>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeTrue();
        content.Error.Should().BeNull();
        content.Value.Should().NotBeNull().And.HaveCount(1);

        var responseTask = content.Value.Single();
        responseTask.Should().NotBeNull();
        responseTask.Id.Should().Be(taskInTrash.Id);
        responseTask.Title.Should().Be(taskInTrash.Title);
        responseTask.Description.Should().BeNull();
        responseTask.FolderId.Should().Be(taskInTrash.FolderId);
        responseTask.CompletedDateTime.Should().BeNull();
        responseTask.DueDateTime.Should().Be(taskInTrash.DueDateTime);
        responseTask.MovedToTrashDateTime.Should().Be(taskInTrash.MovedToTrashDateTime);
        responseTask.TagIds.Should().BeNull();
        responseTask.CreatedDateTime.Should().Be(taskInTrash.CreatedDateTime);
        responseTask.ModifiedDateTime.Should().Be(taskInTrash.ModifiedDateTime);
    }

    [Fact]
    public async Task CreateTaskWithValidData()
    {
        // Arrange
        var folder = await CreateFolderInDatabase();

        var dueDateTime = new DateTime(
            year: 2025, month: 7, day: 1, hour: 1, minute: 1, second: 1,
            DateTimeKind.Utc);
        var creationDto = new UserTaskForCreationDto(
            Title: "Task title", folder.Id, dueDateTime, CreatedDateTime: null);

        var utcNow = new DateTime(
            year: 2025, month: 7, day: 1, hour: 1, minute: 1, second: 2,
            DateTimeKind.Utc);
        MockDateTimeProvider.Setup(p => p.UtcNow).Returns(utcNow);

        // Act
        var response = await Client.PostAsJsonAsync(requestUri: "/api/tasks", creationDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Result<UserTaskVm>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeTrue();
        content.Error.Should().BeNull();

        var responseTask = content.Value;
        responseTask.Should().NotBeNull();
        responseTask.Title.Should().Be(creationDto.Title);
        responseTask.Description.Should().BeNull();
        responseTask.FolderId.Should().Be(creationDto.FolderId);
        responseTask.CompletedDateTime.Should().BeNull();
        responseTask.DueDateTime.Should().Be(creationDto.DueDateTime);
        responseTask.MovedToTrashDateTime.Should().BeNull();
        responseTask.TagIds.Should().BeNull();
        responseTask.CreatedDateTime.Should().Be(utcNow);
        responseTask.ModifiedDateTime.Should().BeNull();

        var dbTasks = await GetTasksFromDatabase();
        dbTasks.Should().HaveCount(1);

        var dbTask = dbTasks.Single();
        dbTask.Id.Should().Be(responseTask.Id);
        dbTask.Title.Should().Be(creationDto.Title);
        dbTask.Description.Should().BeNull();
        dbTask.FolderId.Should().Be(creationDto.FolderId);
        dbTask.CompletedDateTime.Should().BeNull();
        dbTask.DueDateTime.Should().Be(creationDto.DueDateTime);
        dbTask.MovedToTrashDateTime.Should().BeNull();
        dbTask.Tags.Should().BeNullOrEmpty();
        dbTask.CreatedDateTime.Should().Be(utcNow);
        dbTask.ModifiedDateTime.Should().BeNull();
    }

    [Fact]
    public async Task CreateTaskWithInvalidData()
    {
        // Arrange
        const string INVALID_TITLE = "   \t   \n   ";
        var anyDateTime = new DateTime();
        var creationDto = new UserTaskForCreationDto(
            INVALID_TITLE, FolderId: null, DueDateTime: null, CreatedDateTime: anyDateTime);

        // Act
        var response = await Client.PostAsJsonAsync(requestUri: "/api/tasks", creationDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Result<UserTaskVm>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeFalse();
        content.Error.Should().NotBeNullOrWhiteSpace();
        content.Value.Should().BeNull();

        var dbTasks = await GetTasksFromDatabase();
        dbTasks.Should().BeEmpty();
    }

    [Fact]
    public async Task UpdateTaskWithValidDataWhenTaskExists()
    {
        // Arrange
        var oldDueDateTime = new DateTime(
            year: 2025, month: 7, day: 1, hour: 1, minute: 1, second: 1,
            DateTimeKind.Utc);
        var createdDateTime = new DateTime(
            year: 2025, month: 7, day: 1, hour: 1, minute: 1, second: 2,
            DateTimeKind.Utc);
        var task = await CreateTaskInDatabase(
            title: "Old task title", folderId: null, oldDueDateTime, createdDateTime);

        var folder = await CreateFolderInDatabase();
        var tagId1 = await CreateTagInDatabase();
        var tagId2 = await CreateTagInDatabase();
        var newDueDateTime = new DateTime(
            year: 2025, month: 7, day: 1, hour: 1, minute: 1, second: 3,
            DateTimeKind.Utc);

        var updateDto = new UserTaskForUpdateDto(
            Title: "New task title",
            Description: "Task description",
            folder.Id,
            TagIds: [tagId1.Id, tagId2.Id],
            newDueDateTime,
            ModifiedDateTime: null);

        var utcNow = new DateTime(
            year: 2025, month: 7, day: 1, hour: 1, minute: 1, second: 4,
            DateTimeKind.Utc);
        MockDateTimeProvider.Setup(p => p.UtcNow).Returns(utcNow);

        // Act
        var response = await Client.PutAsJsonAsync(
            requestUri: $"/api/tasks/{task.Id}", updateDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Result<UserTaskVm>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeTrue();
        content.Error.Should().BeNull();

        var responseTask = content.Value;
        responseTask.Should().NotBeNull();
        responseTask.Id.Should().Be(task.Id);
        responseTask.Title.Should().Be(updateDto.Title);
        responseTask.Description.Should().Be(updateDto.Description);
        responseTask.FolderId.Should().Be(updateDto.FolderId);
        responseTask.CompletedDateTime.Should().BeNull();
        responseTask.DueDateTime.Should().Be(updateDto.DueDateTime);
        responseTask.MovedToTrashDateTime.Should().BeNull();
        responseTask.TagIds.Should().BeEquivalentTo([tagId1.Id, tagId2.Id]);
        responseTask.CreatedDateTime.Should().Be(task.CreatedDateTime);
        responseTask.ModifiedDateTime.Should().Be(utcNow);

        var dbTasks = await GetTasksFromDatabase();
        dbTasks.Should().HaveCount(1);

        var dbTask = dbTasks.Single();
        dbTask.Id.Should().Be(responseTask.Id);
        dbTask.Title.Should().Be(updateDto.Title);
        dbTask.Description.Should().Be(updateDto.Description);
        dbTask.FolderId.Should().Be(updateDto.FolderId);
        dbTask.CompletedDateTime.Should().BeNull();
        dbTask.DueDateTime.Should().Be(updateDto.DueDateTime);
        dbTask.MovedToTrashDateTime.Should().BeNull();
        dbTask.Tags.Should().HaveCount(2);
        dbTask.Tags.Select(t => t.Id).ToList().Should().BeEquivalentTo([tagId1.Id, tagId2.Id]);
        dbTask.CreatedDateTime.Should().Be(task.CreatedDateTime);
        dbTask.ModifiedDateTime.Should().Be(utcNow);
    }

    [Fact]
    public async Task UpdateTaskWhenTaskNotExists()
    {
        // Arrange
        const int NON_EXISTENT_TASK_ID = 1;

        var anyDateTime = new DateTime();
        var updateDto = new UserTaskForUpdateDto(
            Title: "New task title",
            Description: "New task description",
            FolderId: null,
            TagIds: null,
            DueDateTime: null,
            ModifiedDateTime: anyDateTime);

        // Act
        var response = await Client.PutAsJsonAsync(
            requestUri: $"/api/tasks/{NON_EXISTENT_TASK_ID}", updateDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Result<UserTaskVm>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeFalse();
        content.Error.Should().NotBeNullOrWhiteSpace();
        content.Value.Should().BeNull();
    }

    [Fact]
    public async Task UpdateTaskWithInvalidData()
    {
        // Arrange
        const string OLD_TASK_TITLE = "Old task title";
        var oldDueDateTime = new DateTime(
            year: 2025, month: 7, day: 1, hour: 1, minute: 1, second: 1,
            DateTimeKind.Utc);
        var createdDateTime = new DateTime(
            year: 2025, month: 7, day: 1, hour: 1, minute: 1, second: 2,
            DateTimeKind.Utc);
        var task = await CreateTaskInDatabase(
            OLD_TASK_TITLE, folderId: null, oldDueDateTime, createdDateTime);

        const string INVALID_TITLE = "   \t   \n   ";
        var anyDateTime = new DateTime();
        var updateDto = new UserTaskForUpdateDto(
            INVALID_TITLE,
            Description: null,
            FolderId: null,
            TagIds: null,
            DueDateTime: null,
            ModifiedDateTime: anyDateTime);

        // Act
        var response = await Client.PutAsJsonAsync(
            requestUri: $"/api/tasks/{task.Id}", updateDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Result<UserTaskVm>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeFalse();
        content.Error.Should().NotBeNullOrWhiteSpace();
        content.Value.Should().BeNull();

        var dbTasks = await GetTasksFromDatabase();
        dbTasks.Should().HaveCount(1);

        var dbTask = dbTasks.Single();
        dbTask.Id.Should().Be(task.Id);
        dbTask.Title.Should().Be(OLD_TASK_TITLE);
        dbTask.Description.Should().BeNull();
        dbTask.FolderId.Should().BeNull();
        dbTask.CompletedDateTime.Should().BeNull();
        dbTask.DueDateTime.Should().Be(oldDueDateTime);
        dbTask.MovedToTrashDateTime.Should().BeNull();
        dbTask.Tags.Should().BeNullOrEmpty();
        dbTask.CreatedDateTime.Should().Be(createdDateTime);
        dbTask.ModifiedDateTime.Should().BeNull();
    }

    [Fact]
    public async Task CompleteTaskWhenTaskExists()
    {
        // Arrange
        var folder = await CreateFolderInDatabase();
        var task = await CreateIncompletedTaskInDatabase(folder.Id);

        var completeDto = new UserTaskForCompleteDto(CompletedDateTime: null);

        var utcNow = new DateTime(
            year: 2025, month: 7, day: 1, hour: 1, minute: 1, second: 10,
            DateTimeKind.Utc);
        MockDateTimeProvider.Setup(p => p.UtcNow).Returns(utcNow);

        // Act
        var response = await Client.PutAsJsonAsync(
            requestUri: $"/api/tasks/{task.Id}/completed", completeDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Result<UserTaskVm>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeTrue();
        content.Error.Should().BeNull();

        var responseTask = content.Value;
        responseTask.Should().NotBeNull();
        responseTask.Id.Should().Be(task.Id);
        responseTask.Title.Should().Be(task.Title);
        responseTask.Description.Should().BeNull();
        responseTask.FolderId.Should().Be(task.FolderId);
        responseTask.CompletedDateTime.Should().Be(utcNow);
        responseTask.DueDateTime.Should().Be(task.DueDateTime);
        responseTask.MovedToTrashDateTime.Should().BeNull();
        responseTask.TagIds.Should().BeNull();
        responseTask.CreatedDateTime.Should().Be(task.CreatedDateTime);
        responseTask.ModifiedDateTime.Should().Be(utcNow);

        var dbTasks = await GetTasksFromDatabase();
        dbTasks.Should().HaveCount(1);

        var dbTask = dbTasks.Single();
        dbTask.Id.Should().Be(task.Id);
        dbTask.Title.Should().Be(task.Title);
        dbTask.Description.Should().BeNull();
        dbTask.FolderId.Should().Be(task.FolderId);
        dbTask.CompletedDateTime.Should().Be(utcNow);
        dbTask.DueDateTime.Should().Be(task.DueDateTime);
        dbTask.MovedToTrashDateTime.Should().BeNull();
        dbTask.Tags.Should().BeNullOrEmpty();
        dbTask.CreatedDateTime.Should().Be(task.CreatedDateTime);
        dbTask.ModifiedDateTime.Should().Be(utcNow);
    }

    [Fact]
    public async Task CompleteTaskWhenTaskNotExists()
    {
        // Arrange
        const int NON_EXISTENT_TASK_ID = 1;
        var anyDateTime = new DateTime();
        var completeDto = new UserTaskForCompleteDto(CompletedDateTime: anyDateTime);

        // Act
        var response = await Client.PutAsJsonAsync(
            requestUri: $"/api/tasks/{NON_EXISTENT_TASK_ID}/completed", completeDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Result<UserTaskVm>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeFalse();
        content.Error.Should().NotBeNullOrWhiteSpace();
        content.Value.Should().BeNull();
    }

    [Fact]
    public async Task IncompleteTaskWhenTaskExists()
    {
        // Arrange
        var folder = await CreateFolderInDatabase();
        var task = await CreateCompletedTaskInDatabase(folder.Id);

        var incompleteDto = new UserTaskForIncompleteDto(ModifiedDateTime: null);

        var utcNow = new DateTime(
            year: 2025, month: 7, day: 1, hour: 1, minute: 1, second: 10,
            DateTimeKind.Utc);
        MockDateTimeProvider.Setup(p => p.UtcNow).Returns(utcNow);

        // Act
        var response = await Client.PutAsJsonAsync(
            requestUri: $"/api/tasks/{task.Id}/incompleted", incompleteDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Result<UserTaskVm>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeTrue();
        content.Error.Should().BeNull();

        var responseTask = content.Value;
        responseTask.Should().NotBeNull();
        responseTask.Id.Should().Be(task.Id);
        responseTask.Title.Should().Be(task.Title);
        responseTask.Description.Should().BeNull();
        responseTask.FolderId.Should().Be(task.FolderId);
        responseTask.CompletedDateTime.Should().BeNull();
        responseTask.DueDateTime.Should().Be(task.DueDateTime);
        responseTask.MovedToTrashDateTime.Should().BeNull();
        responseTask.TagIds.Should().BeNull();
        responseTask.CreatedDateTime.Should().Be(task.CreatedDateTime);
        responseTask.ModifiedDateTime.Should().Be(utcNow);

        var dbTasks = await GetTasksFromDatabase();
        dbTasks.Should().HaveCount(1);

        var dbTask = dbTasks.Single();
        dbTask.Id.Should().Be(task.Id);
        dbTask.Title.Should().Be(task.Title);
        dbTask.Description.Should().BeNull();
        dbTask.FolderId.Should().Be(task.FolderId);
        dbTask.CompletedDateTime.Should().BeNull();
        dbTask.DueDateTime.Should().Be(task.DueDateTime);
        dbTask.MovedToTrashDateTime.Should().BeNull();
        dbTask.Tags.Should().BeNullOrEmpty();
        dbTask.CreatedDateTime.Should().Be(task.CreatedDateTime);
        dbTask.ModifiedDateTime.Should().Be(utcNow);
    }

    [Fact]
    public async Task IncompleteTaskWhenTaskNotExists()
    {
        // Arrange
        const int NON_EXISTENT_TASK_ID = 1;
        var anyDateTime = new DateTime();
        var completeDto = new UserTaskForIncompleteDto(ModifiedDateTime: anyDateTime);

        // Act
        var response = await Client.PutAsJsonAsync(
            requestUri: $"/api/tasks/{NON_EXISTENT_TASK_ID}/incompleted", completeDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Result<UserTaskVm>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeFalse();
        content.Error.Should().NotBeNullOrWhiteSpace();
        content.Value.Should().BeNull();
    }

    [Fact]
    public async Task MoveTaskToTrashWhenTaskExists()
    {
        // Arrange
        var folder = await CreateFolderInDatabase();

        var dueDateTime = new DateTime(
            year: 2025, month: 7, day: 1, hour: 1, minute: 1, second: 1,
            DateTimeKind.Utc);
        var createdDateTime = new DateTime(
            year: 2025, month: 7, day: 1, hour: 1, minute: 1, second: 2,
            DateTimeKind.Utc);
        var task = await CreateTaskInDatabase(
            title: "Task title", folder.Id, dueDateTime, createdDateTime);

        var moveToTrashDto = new UserTaskForMoveToTrashDto(MovedToTrashDateTime: null);

        var utcNow = new DateTime(
            year: 2025, month: 7, day: 1, hour: 1, minute: 1, second: 3,
            DateTimeKind.Utc);
        MockDateTimeProvider.Setup(p => p.UtcNow).Returns(utcNow);

        // Act
        var response = await Client.PutAsJsonAsync(
            requestUri: $"/api/tasks/{task.Id}/movedToTrash", moveToTrashDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Result<UserTaskVm>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeTrue();
        content.Error.Should().BeNull();

        var responseTask = content.Value;
        responseTask.Should().NotBeNull();
        responseTask.Id.Should().Be(task.Id);
        responseTask.Title.Should().Be(task.Title);
        responseTask.Description.Should().BeNull();
        responseTask.FolderId.Should().BeNull();
        responseTask.CompletedDateTime.Should().BeNull();
        responseTask.DueDateTime.Should().Be(task.DueDateTime);
        responseTask.MovedToTrashDateTime.Should().Be(utcNow);
        responseTask.TagIds.Should().BeNull();
        responseTask.CreatedDateTime.Should().Be(task.CreatedDateTime);
        responseTask.ModifiedDateTime.Should().Be(utcNow);

        var dbTasks = await GetTasksFromDatabase();
        dbTasks.Should().HaveCount(1);

        var dbTask = dbTasks.Single();
        dbTask.Id.Should().Be(task.Id);
        dbTask.Title.Should().Be(task.Title);
        dbTask.Description.Should().BeNull();
        dbTask.FolderId.Should().BeNull();
        dbTask.CompletedDateTime.Should().BeNull();
        dbTask.DueDateTime.Should().Be(task.DueDateTime);
        dbTask.MovedToTrashDateTime.Should().Be(utcNow);
        dbTask.Tags.Should().BeNullOrEmpty();
        dbTask.CreatedDateTime.Should().Be(task.CreatedDateTime);
        dbTask.ModifiedDateTime.Should().Be(utcNow);
    }

    [Fact]
    public async Task MoveTaskToTrashWhenTaskNotExists()
    {
        // Arrange
        const int NON_EXISTENT_TASK_ID = 1;
        var anyDateTime = new DateTime();
        var moveToTrashDto = new UserTaskForMoveToTrashDto(MovedToTrashDateTime: anyDateTime);

        // Act
        var response = await Client.PutAsJsonAsync(
            requestUri: $"/api/tasks/{NON_EXISTENT_TASK_ID}/movedToTrash", moveToTrashDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Result<UserTaskVm>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeFalse();
        content.Error.Should().NotBeNullOrWhiteSpace();
        content.Value.Should().BeNull();
    }

    [Fact]
    public async Task MoveTaskFromTrashWhenTaskAndFolderExist()
    {
        // Arrange
        var task = await CreateTaskInTrashInDatabase();

        var folder = await CreateFolderInDatabase();
        var moveFromTrashDto = new UserTaskForMoveFromTrashDto(folder.Id, ModifiedDateTime: null);

        var utcNow = new DateTime(
            year: 2025, month: 7, day: 1, hour: 1, minute: 1, second: 10,
            DateTimeKind.Utc);
        MockDateTimeProvider.Setup(p => p.UtcNow).Returns(utcNow);

        // Act
        var response = await Client.PutAsJsonAsync(
            requestUri: $"/api/tasks/{task.Id}/movedFromTrash", moveFromTrashDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Result<UserTaskVm>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeTrue();
        content.Error.Should().BeNull();

        var responseTask = content.Value;
        responseTask.Should().NotBeNull();
        responseTask.Id.Should().Be(task.Id);
        responseTask.Title.Should().Be(task.Title);
        responseTask.Description.Should().BeNull();
        responseTask.FolderId.Should().Be(folder.Id);
        responseTask.CompletedDateTime.Should().BeNull();
        responseTask.DueDateTime.Should().Be(task.DueDateTime);
        responseTask.MovedToTrashDateTime.Should().BeNull();
        responseTask.TagIds.Should().BeNull();
        responseTask.CreatedDateTime.Should().Be(task.CreatedDateTime);
        responseTask.ModifiedDateTime.Should().Be(utcNow);

        var dbTasks = await GetTasksFromDatabase();
        dbTasks.Should().HaveCount(1);

        var dbTask = dbTasks.Single();
        dbTask.Id.Should().Be(task.Id);
        dbTask.Title.Should().Be(task.Title);
        dbTask.Description.Should().BeNull();
        dbTask.FolderId.Should().Be(folder.Id);
        dbTask.CompletedDateTime.Should().BeNull();
        dbTask.DueDateTime.Should().Be(task.DueDateTime);
        dbTask.MovedToTrashDateTime.Should().BeNull();
        dbTask.Tags.Should().BeNullOrEmpty();
        dbTask.CreatedDateTime.Should().Be(task.CreatedDateTime);
        dbTask.ModifiedDateTime.Should().Be(utcNow);
    }

    [Fact]
    public async Task MoveTaskFromTrashWhenTaskNotExits()
    {
        // Arrange
        const int NON_EXISTENT_TASK_ID = 1;
        var anyDateTime = new DateTime();
        var moveToTrashDto = new UserTaskForMoveToTrashDto(MovedToTrashDateTime: anyDateTime);

        // Act
        var response = await Client.PutAsJsonAsync(
            requestUri: $"/api/tasks/{NON_EXISTENT_TASK_ID}/movedFromTrash", moveToTrashDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Result<UserTaskVm>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeFalse();
        content.Error.Should().NotBeNullOrWhiteSpace();
        content.Value.Should().BeNull();
    }

    [Fact]
    public async Task MoveTaskFromTrashWhenFolderNotExists()
    {
        // Arrange
        var task = await CreateTaskInTrashInDatabase();

        const int NON_EXISTENT_FOLDER_ID = 1;
        var anyDateTime = new DateTime();
        var moveFromTrashDto = new UserTaskForMoveFromTrashDto(
            NON_EXISTENT_FOLDER_ID, ModifiedDateTime: anyDateTime);

        // Act
        var response = await Client.PutAsJsonAsync(
            requestUri: $"/api/tasks/{task.Id}/movedFromTrash", moveFromTrashDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Result<UserTaskVm>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeFalse();
        content.Error.Should().NotBeNullOrWhiteSpace();
        content.Value.Should().BeNull();

        var dbTasks = await GetTasksFromDatabase();
        dbTasks.Should().HaveCount(1);

        var dbTask = dbTasks.Single();
        dbTask.Id.Should().Be(task.Id);
        dbTask.Title.Should().Be(task.Title);
        dbTask.Description.Should().BeNull();
        dbTask.FolderId.Should().BeNull();
        dbTask.CompletedDateTime.Should().BeNull();
        dbTask.DueDateTime.Should().Be(task.DueDateTime);
        dbTask.MovedToTrashDateTime.Should().Be(task.MovedToTrashDateTime);
        dbTask.Tags.Should().BeNullOrEmpty();
        dbTask.CreatedDateTime.Should().Be(task.CreatedDateTime);
        dbTask.ModifiedDateTime.Should().Be(task.ModifiedDateTime);
    }

    [Fact]
    public async Task DeleteTaskWhenTaskExists()
    {
        // Arrange
        var taskToDelete = await CreateTaskInTrashInDatabase();
        var otherTask = await CreateTaskInTrashInDatabase();

        // Act
        var response = await Client
            .DeleteAsync(requestUri: $"/api/tasks/{taskToDelete.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Result>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeTrue();
        content.Error.Should().BeNull();

        var dbTasks = await GetTasksFromDatabase();
        dbTasks.Should().HaveCount(1);
        dbTasks.Single().Id.Should().Be(otherTask.Id);
    }

    [Fact]
    public async Task DeleteTaskWhenTaskNotExists()
    {
        // Arrange
        const int NON_EXISTENT_TASK_ID = 1;

        // Act
        var response = await Client
            .DeleteAsync(requestUri: $"/api/tasks/{NON_EXISTENT_TASK_ID}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Result>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeFalse();
        content.Error.Should().NotBeNullOrWhiteSpace();
    }

    #region helpers
    private async Task<UserTask> CreateTaskInDatabase(
        string title = "Task title 42",
        int? folderId = null,
        DateTime? dueDateTime = null,
        DateTime? createdDateTime = null)
    {
        createdDateTime ??= new DateTime(
            year: 2025, month: 1, day: 1, hour: 1, minute: 1, second: 1,
            DateTimeKind.Utc);

        var creationDto = new UserTaskForCreationDto(title, folderId, dueDateTime, createdDateTime);
        var taskResult = UserTask.CreateTask(creationDto, createdDateTime.Value);

        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        dbContext.Tasks.Add(taskResult.Value);
        await dbContext.SaveChangesAsync();

        return taskResult.Value;
    }

    private async Task<UserTask> CreateIncompletedTaskInDatabase(int folderId)
    {
        var dueDateTime = new DateTime(
            year: 2025, month: 1, day: 1, hour: 1, minute: 1, second: 1,
            DateTimeKind.Utc);
        var createdDateTime = new DateTime(
            year: 2025, month: 1, day: 1, hour: 1, minute: 1, second: 2,
            DateTimeKind.Utc);
        var creationDto = new UserTaskForCreationDto(
            Title: "Incompleted task title", folderId, dueDateTime, createdDateTime);
        var taskResult = UserTask.CreateTask(creationDto, createdDateTime);

        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        dbContext.Tasks.Add(taskResult.Value);
        await dbContext.SaveChangesAsync();

        return taskResult.Value;
    }

    private async Task<UserTask> CreateCompletedTaskInDatabase(int folderId)
    {
        var dueDateTime = new DateTime(
            year: 2025, month: 1, day: 1, hour: 1, minute: 2, second: 1,
            DateTimeKind.Utc);
        var createdDateTime = new DateTime(
            year: 2025, month: 1, day: 1, hour: 1, minute: 2, second: 2,
            DateTimeKind.Utc);
        var creationDto = new UserTaskForCreationDto(
            Title: "Completed task title", folderId, dueDateTime, createdDateTime);
        var taskResult = UserTask.CreateTask(creationDto, createdDateTime);
        var task = taskResult.Value;

        var completedDateTime = new DateTime(
            year: 2025, month: 1, day: 1, hour: 1, minute: 2, second: 3,
            DateTimeKind.Utc);
        task.Complete(completedDateTime);

        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        dbContext.Tasks.Add(task);
        await dbContext.SaveChangesAsync();

        return task;
    }

    private async Task<UserTask> CreateTaskInTrashInDatabase()
    {
        var dueDateTime = new DateTime(
            year: 2025, month: 1, day: 1, hour: 1, minute: 3, second: 1,
            DateTimeKind.Utc);
        var createdDateTime = new DateTime(
            year: 2025, month: 1, day: 1, hour: 1, minute: 3, second: 2,
            DateTimeKind.Utc);
        var creationDto = new UserTaskForCreationDto(
            Title: "Task in trash title", FolderId: null, dueDateTime, createdDateTime);
        var taskResult = UserTask.CreateTask(creationDto, createdDateTime);
        var task = taskResult.Value;

        var movedToTrashDateTime = new DateTime(
            year: 2025, month: 1, day: 1, hour: 1, minute: 3, second: 3,
            DateTimeKind.Utc);
        task.MoveToTrash(movedToTrashDateTime);

        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        dbContext.Tasks.Add(task);
        await dbContext.SaveChangesAsync();

        return task;
    }

    private async Task<Folder> CreateFolderInDatabase()
    {
        var anyDateTime = new DateTime();
        var creationDto = new FolderForCreationDto(Title: "Folder title 42", anyDateTime);
        var folderResult = Folder.CreateFolder(creationDto, anyDateTime);

        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        dbContext.Folders.Add(folderResult.Value);
        await dbContext.SaveChangesAsync();

        return folderResult.Value;
    }

    private async Task<Tag> CreateTagInDatabase()
    {
        var anyDateTime = new DateTime();
        var creationDto = new TagForCreationDto(
            Title: "Tag title 42", Color: "424242", anyDateTime);
        var tagResult = Tag.CreateTag(creationDto, anyDateTime);

        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        dbContext.Tags.Add(tagResult.Value);
        await dbContext.SaveChangesAsync();

        return tagResult.Value;
    }

    private async Task<IReadOnlyList<UserTask>> GetTasksFromDatabase()
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        return await dbContext.Tasks.Include(t => t.Tags).ToListAsync();
    }
    #endregion
}