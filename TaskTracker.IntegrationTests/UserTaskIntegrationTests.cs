namespace TaskTracker.IntegrationTests;

public class UserTaskIntegrationTests(ApiWebApplicationFactory factory)
    : IntegrationTestBase(factory)
{
    [Fact]
    public async Task GetTaskById()
    {
        // Arrange
        var folder = CreateFolderInDatabase();

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

        var content = await response.Content.ReadFromJsonAsync<ApiResponse<UserTaskVm>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeTrue();
        content.Error.Should().BeNull();

        var responseTask = content.Result;
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
        responseTask.ModifiedDateTime.Should().Be(null);
    }

    [Fact]
    public async Task GetIncompletedTasks()
    {
        // Arrange
        var folder = await CreateFolderInDatabase();

        var incompletedTask = await CreateIncompletedTaskInDatabase(folder.Id);
        var completedTask = await CreateCompletedTaskInDatabase(folder.Id);
        var taskInTrash = await CreateTaskInTrashInDatabase(folder.Id);

        // Act
        var response = await Client.GetAsync(requestUri: $"/api/tasks/incomplete");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content
            .ReadFromJsonAsync<ApiResponse<IReadOnlyList<UserTaskVm>>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeTrue();
        content.Error.Should().BeNull();
        content.Result.Should().NotBeNull().And.HaveCount(1);

        var responseTask = content.Result.Single();
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
        responseTask.ModifiedDateTime.Should().Be(null);
    }

    [Fact]
    public async Task GetCompletedTasks()
    {
        // Arrange
        var folder = await CreateFolderInDatabase();

        var incompletedTask = await CreateIncompletedTaskInDatabase(folder.Id);
        var completedTask = await CreateCompletedTaskInDatabase(folder.Id);
        var taskInTrash = await CreateTaskInTrashInDatabase(folder.Id);

        // Act
        var response = await Client.GetAsync(requestUri: $"/api/tasks/complete");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content
            .ReadFromJsonAsync<ApiResponse<IReadOnlyList<UserTaskVm>>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeTrue();
        content.Error.Should().BeNull();
        content.Result.Should().NotBeNull().And.HaveCount(1);

        var responseTask = content.Result.Single();
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
        var taskInTrash = await CreateTaskInTrashInDatabase(folder.Id);

        // Act
        var response = await Client.GetAsync(requestUri: $"/api/tasks/trash");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content
            .ReadFromJsonAsync<ApiResponse<IReadOnlyList<UserTaskVm>>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeTrue();
        content.Error.Should().BeNull();
        content.Result.Should().NotBeNull().And.HaveCount(1);

        var responseTask = content.Result.Single();
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
    public async Task DeleteTask()
    {
        // Arrange
        var folder = await CreateFolderInDatabase();

        var taskToDelete = await CreateTaskInTrashInDatabase(folder.Id);
        var otherTask = await CreateTaskInTrashInDatabase(folder.Id);

        // Act
        var response = await Client
            .DeleteAsync(requestUri: $"/api/tasks/{taskToDelete.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<ApiResponse<UserTaskVm>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeTrue();
        content.Error.Should().BeNull();
        content.Result.Should().BeNull();

        var dbTasks = await GetTasksFromDatabase();
        dbTasks.Should().HaveCount(1);
        dbTasks.Single().Id.Should().Be(otherTask.Id);
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
        var task = UserTask.CreateTask(creationDto, createdDateTime.Value);

        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        dbContext.Tasks.Add(task);
        await dbContext.SaveChangesAsync();

        return task;
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
        var task = UserTask.CreateTask(creationDto, createdDateTime);

        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        dbContext.Tasks.Add(task);
        await dbContext.SaveChangesAsync();

        return task;
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
        var task = UserTask.CreateTask(creationDto, createdDateTime);

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

    private async Task<UserTask> CreateTaskInTrashInDatabase(int folderId)
    {
        var dueDateTime = new DateTime(
            year: 2025, month: 1, day: 1, hour: 1, minute: 3, second: 1,
            DateTimeKind.Utc);
        var createdDateTime = new DateTime(
            year: 2025, month: 1, day: 1, hour: 1, minute: 3, second: 2,
            DateTimeKind.Utc);
        var creationDto = new UserTaskForCreationDto(
            Title: "Task in trash title", folderId, dueDateTime, createdDateTime);
        var task = UserTask.CreateTask(creationDto, createdDateTime);

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
        var folder = Folder.CreateFolder(creationDto, anyDateTime);

        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        dbContext.Folders.Add(folder);
        await dbContext.SaveChangesAsync();

        return folder;
    }

    private async Task<IReadOnlyList<UserTask>> GetTasksFromDatabase()
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        return await dbContext.Tasks.ToListAsync();
    }
    #endregion
}