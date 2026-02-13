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

       await CreateTaskInDatabase();

        var utcNow = new DateTime(
            year: 2025, month: 7, day: 2, hour: 1, minute: 1, second: 1,
            DateTimeKind.Utc);
        MockDateTimeProvider.Setup(p => p.UtcNow).Returns(utcNow);

        // Act
        var response = await Client.GetAsync(
            requestUri: $"/api/tasks/{task.Id}",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var responseTask = await response.Content.ReadFromJsonAsync<UserTaskVm>(
            TestContext.Current.CancellationToken);

        responseTask.ShouldNotBeNull();
        responseTask.Id.ShouldBe(task.Id);
        responseTask.Title.ShouldBe(task.Title);
        responseTask.Description.ShouldBeNull();
        responseTask.FolderId.ShouldBe(task.FolderId);
        responseTask.CompletedDateTime.ShouldBeNull();
        responseTask.DueDateTime.ShouldBe(task.DueDateTime);
        responseTask.MovedToTrashDateTime.ShouldBeNull();
        responseTask.TagIds.ShouldBeEmpty();
        responseTask.CreatedDateTime.ShouldBe(task.CreatedDateTime);
        responseTask.ModifiedDateTime.ShouldBeNull();
    }

    [Fact]
    public async Task GetTaskByIdWhenTaskNotExists()
    {
        // Arrange
        const int NON_EXISTENT_TASK_ID = 1;

        // Act
        var response = await Client.GetAsync(
            requestUri: $"/api/tasks/{NON_EXISTENT_TASK_ID}",
            TestContext.Current.CancellationToken);

        // Assert
        await AssertResponseWithDomainProblemDetails(response);
    }

    [Fact]
    public async Task GetIncompletedTasks()
    {
        // Arrange
        var folder = await CreateFolderInDatabase();
        var incompletedTask = await CreateIncompletedTaskInDatabase(folder.Id);

        await CreateCompletedTaskInDatabase(folder.Id);
        await CreateTaskInTrashInDatabase();

        // Act
        var response = await Client.GetAsync(
            requestUri: "/api/tasks/incomplete",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var responseTasks = await response.Content
            .ReadFromJsonAsync<IReadOnlyList<UserTaskVm>>(
                TestContext.Current.CancellationToken);

        responseTasks.ShouldNotBeNull();
        responseTasks.Count.ShouldBe(1);

        var responseTask = responseTasks.Single();
        responseTask.ShouldNotBeNull();
        responseTask.Id.ShouldBe(incompletedTask.Id);
        responseTask.Title.ShouldBe(incompletedTask.Title);
        responseTask.Description.ShouldBeNull();
        responseTask.FolderId.ShouldBe(incompletedTask.FolderId);
        responseTask.CompletedDateTime.ShouldBeNull();
        responseTask.DueDateTime.ShouldBe(incompletedTask.DueDateTime);
        responseTask.MovedToTrashDateTime.ShouldBeNull();
        responseTask.TagIds.ShouldBeEmpty();
        responseTask.CreatedDateTime.ShouldBe(incompletedTask.CreatedDateTime);
        responseTask.ModifiedDateTime.ShouldBeNull();
    }

    [Fact]
    public async Task GetCompletedTasks()
    {
        // Arrange
        var folder = await CreateFolderInDatabase();
        var completedTask = await CreateCompletedTaskInDatabase(folder.Id);

        await CreateIncompletedTaskInDatabase(folder.Id);
        await CreateTaskInTrashInDatabase();

        // Act
        var response = await Client.GetAsync(
            requestUri: "/api/tasks/complete",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var responseTasks = await response.Content
            .ReadFromJsonAsync<IReadOnlyList<UserTaskVm>>(
                TestContext.Current.CancellationToken);

        responseTasks.ShouldNotBeNull();
        responseTasks.Count.ShouldBe(1);

        var responseTask = responseTasks.Single();
        responseTask.ShouldNotBeNull();
        responseTask.Id.ShouldBe(completedTask.Id);
        responseTask.Title.ShouldBe(completedTask.Title);
        responseTask.Description.ShouldBeNull();
        responseTask.FolderId.ShouldBe(completedTask.FolderId);
        responseTask.CompletedDateTime.ShouldBe(completedTask.CompletedDateTime);
        responseTask.DueDateTime.ShouldBe(completedTask.DueDateTime);
        responseTask.MovedToTrashDateTime.ShouldBeNull();
        responseTask.TagIds.ShouldBeEmpty();
        responseTask.CreatedDateTime.ShouldBe(completedTask.CreatedDateTime);
        responseTask.ModifiedDateTime.ShouldBe(completedTask.ModifiedDateTime);
    }

    [Fact]
    public async Task GetTasksInTrash()
    {
        // Arrange
        var folder = await CreateFolderInDatabase();
        var taskInTrash = await CreateTaskInTrashInDatabase();

        await CreateIncompletedTaskInDatabase(folder.Id);
        await CreateCompletedTaskInDatabase(folder.Id);

        // Act
        var response = await Client.GetAsync(
            requestUri: "/api/tasks/trash",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var responseTasks = await response.Content
            .ReadFromJsonAsync<IReadOnlyList<UserTaskVm>>(
                TestContext.Current.CancellationToken);

        responseTasks.ShouldNotBeNull();
        responseTasks.Count.ShouldBe(1);

        var responseTask = responseTasks.Single();
        responseTask.ShouldNotBeNull();
        responseTask.Id.ShouldBe(taskInTrash.Id);
        responseTask.Title.ShouldBe(taskInTrash.Title);
        responseTask.Description.ShouldBeNull();
        responseTask.FolderId.ShouldBe(taskInTrash.FolderId);
        responseTask.CompletedDateTime.ShouldBeNull();
        responseTask.DueDateTime.ShouldBe(taskInTrash.DueDateTime);
        responseTask.MovedToTrashDateTime.ShouldBe(taskInTrash.MovedToTrashDateTime);
        responseTask.TagIds.ShouldBeEmpty();
        responseTask.CreatedDateTime.ShouldBe(taskInTrash.CreatedDateTime);
        responseTask.ModifiedDateTime.ShouldBe(taskInTrash.ModifiedDateTime);
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
        var response = await Client.PostAsJsonAsync(
            requestUri: "/api/tasks",
            creationDto,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var responseTask = await response.Content.ReadFromJsonAsync<UserTaskVm>(
            TestContext.Current.CancellationToken);

        responseTask.ShouldNotBeNull();
        responseTask.Title.ShouldBe(creationDto.Title);
        responseTask.Description.ShouldBeNull();
        responseTask.FolderId.ShouldBe(creationDto.FolderId);
        responseTask.CompletedDateTime.ShouldBeNull();
        responseTask.DueDateTime.ShouldBe(creationDto.DueDateTime);
        responseTask.MovedToTrashDateTime.ShouldBeNull();
        responseTask.TagIds.ShouldBeEmpty();
        responseTask.CreatedDateTime.ShouldBe(utcNow);
        responseTask.ModifiedDateTime.ShouldBeNull();

        var dbTasks = await GetTasksFromDatabase();
        dbTasks.Count.ShouldBe(1);

        var dbTask = dbTasks.Single();
        dbTask.Id.ShouldBe(responseTask.Id);
        dbTask.Title.ShouldBe(creationDto.Title);
        dbTask.Description.ShouldBeNull();
        dbTask.FolderId.ShouldBe(creationDto.FolderId);
        dbTask.CompletedDateTime.ShouldBeNull();
        dbTask.DueDateTime.ShouldBe(creationDto.DueDateTime);
        dbTask.MovedToTrashDateTime.ShouldBeNull();
        dbTask.Tags.ShouldBeEmpty();
        dbTask.CreatedDateTime.ShouldBe(utcNow);
        dbTask.ModifiedDateTime.ShouldBeNull();
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
        var response = await Client.PostAsJsonAsync(
            requestUri: "/api/tasks",
            creationDto,
            TestContext.Current.CancellationToken);

        // Assert
        await AssertResponseWithDomainProblemDetails(response);

        var dbTasks = await GetTasksFromDatabase();
        dbTasks.ShouldBeEmpty();
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
            requestUri: $"/api/tasks/{task.Id}",
            updateDto,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var responseTask = await response.Content.ReadFromJsonAsync<UserTaskVm>(
            TestContext.Current.CancellationToken);

        responseTask.ShouldNotBeNull();
        responseTask.Id.ShouldBe(task.Id);
        responseTask.Title.ShouldBe(updateDto.Title);
        responseTask.Description.ShouldBe(updateDto.Description);
        responseTask.FolderId.ShouldBe(updateDto.FolderId);
        responseTask.CompletedDateTime.ShouldBeNull();
        responseTask.DueDateTime.ShouldBe(updateDto.DueDateTime);
        responseTask.MovedToTrashDateTime.ShouldBeNull();
        responseTask.TagIds.ShouldBe([tagId1.Id, tagId2.Id]);
        responseTask.CreatedDateTime.ShouldBe(task.CreatedDateTime);
        responseTask.ModifiedDateTime.ShouldBe(utcNow);

        var dbTasks = await GetTasksFromDatabase();
        dbTasks.Count.ShouldBe(1);

        var dbTask = dbTasks.Single();
        dbTask.Id.ShouldBe(responseTask.Id);
        dbTask.Title.ShouldBe(updateDto.Title);
        dbTask.Description.ShouldBe(updateDto.Description);
        dbTask.FolderId.ShouldBe(updateDto.FolderId);
        dbTask.CompletedDateTime.ShouldBeNull();
        dbTask.DueDateTime.ShouldBe(updateDto.DueDateTime);
        dbTask.MovedToTrashDateTime.ShouldBeNull();
        dbTask.Tags.Count.ShouldBe(2);
        dbTask.Tags.Select(t => t.Id).ToList().ShouldBe([tagId1.Id, tagId2.Id]);
        dbTask.CreatedDateTime.ShouldBe(task.CreatedDateTime);
        dbTask.ModifiedDateTime.ShouldBe(utcNow);
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
            requestUri: $"/api/tasks/{NON_EXISTENT_TASK_ID}",
            updateDto,
            TestContext.Current.CancellationToken);

        // Assert
        await AssertResponseWithDomainProblemDetails(response);
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
            requestUri: $"/api/tasks/{task.Id}",
            updateDto,
            TestContext.Current.CancellationToken);

        // Assert
        await AssertResponseWithDomainProblemDetails(response);

        var dbTasks = await GetTasksFromDatabase();
        dbTasks.Count.ShouldBe(1);

        var dbTask = dbTasks.Single();
        dbTask.Id.ShouldBe(task.Id);
        dbTask.Title.ShouldBe(OLD_TASK_TITLE);
        dbTask.Description.ShouldBeNull();
        dbTask.FolderId.ShouldBeNull();
        dbTask.CompletedDateTime.ShouldBeNull();
        dbTask.DueDateTime.ShouldBe(oldDueDateTime);
        dbTask.MovedToTrashDateTime.ShouldBeNull();
        dbTask.Tags.ShouldBeEmpty();
        dbTask.CreatedDateTime.ShouldBe(createdDateTime);
        dbTask.ModifiedDateTime.ShouldBeNull();
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
            requestUri: $"/api/tasks/{task.Id}/completed",
            completeDto,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var responseTask = await response.Content.ReadFromJsonAsync<UserTaskVm>(
            TestContext.Current.CancellationToken);

        responseTask.ShouldNotBeNull();
        responseTask.Id.ShouldBe(task.Id);
        responseTask.Title.ShouldBe(task.Title);
        responseTask.Description.ShouldBeNull();
        responseTask.FolderId.ShouldBe(task.FolderId);
        responseTask.CompletedDateTime.ShouldBe(utcNow);
        responseTask.DueDateTime.ShouldBe(task.DueDateTime);
        responseTask.MovedToTrashDateTime.ShouldBeNull();
        responseTask.TagIds.ShouldBeEmpty();
        responseTask.CreatedDateTime.ShouldBe(task.CreatedDateTime);
        responseTask.ModifiedDateTime.ShouldBe(utcNow);

        var dbTasks = await GetTasksFromDatabase();
        dbTasks.Count.ShouldBe(1);

        var dbTask = dbTasks.Single();
        dbTask.Id.ShouldBe(task.Id);
        dbTask.Title.ShouldBe(task.Title);
        dbTask.Description.ShouldBeNull();
        dbTask.FolderId.ShouldBe(task.FolderId);
        dbTask.CompletedDateTime.ShouldBe(utcNow);
        dbTask.DueDateTime.ShouldBe(task.DueDateTime);
        dbTask.MovedToTrashDateTime.ShouldBeNull();
        dbTask.Tags.ShouldBeEmpty();
        dbTask.CreatedDateTime.ShouldBe(task.CreatedDateTime);
        dbTask.ModifiedDateTime.ShouldBe(utcNow);
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
            requestUri: $"/api/tasks/{NON_EXISTENT_TASK_ID}/completed",
            completeDto,
            TestContext.Current.CancellationToken);

        // Assert
        await AssertResponseWithDomainProblemDetails(response);
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
            requestUri: $"/api/tasks/{task.Id}/incompleted",
            incompleteDto,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var responseTask = await response.Content.ReadFromJsonAsync<UserTaskVm>(
            TestContext.Current.CancellationToken);

        responseTask.ShouldNotBeNull();
        responseTask.Id.ShouldBe(task.Id);
        responseTask.Title.ShouldBe(task.Title);
        responseTask.Description.ShouldBeNull();
        responseTask.FolderId.ShouldBe(task.FolderId);
        responseTask.CompletedDateTime.ShouldBeNull();
        responseTask.DueDateTime.ShouldBe(task.DueDateTime);
        responseTask.MovedToTrashDateTime.ShouldBeNull();
        responseTask.TagIds.ShouldBeEmpty();
        responseTask.CreatedDateTime.ShouldBe(task.CreatedDateTime);
        responseTask.ModifiedDateTime.ShouldBe(utcNow);

        var dbTasks = await GetTasksFromDatabase();
        dbTasks.Count.ShouldBe(1);

        var dbTask = dbTasks.Single();
        dbTask.Id.ShouldBe(task.Id);
        dbTask.Title.ShouldBe(task.Title);
        dbTask.Description.ShouldBeNull();
        dbTask.FolderId.ShouldBe(task.FolderId);
        dbTask.CompletedDateTime.ShouldBeNull();
        dbTask.DueDateTime.ShouldBe(task.DueDateTime);
        dbTask.MovedToTrashDateTime.ShouldBeNull();
        dbTask.Tags.ShouldBeEmpty();
        dbTask.CreatedDateTime.ShouldBe(task.CreatedDateTime);
        dbTask.ModifiedDateTime.ShouldBe(utcNow);
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
            requestUri: $"/api/tasks/{NON_EXISTENT_TASK_ID}/incompleted",
            completeDto,
            TestContext.Current.CancellationToken);

        // Assert
        await AssertResponseWithDomainProblemDetails(response);
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
            requestUri: $"/api/tasks/{task.Id}/movedToTrash",
            moveToTrashDto,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var responseTask = await response.Content.ReadFromJsonAsync<UserTaskVm>(
            TestContext.Current.CancellationToken);

        responseTask.ShouldNotBeNull();
        responseTask.Id.ShouldBe(task.Id);
        responseTask.Title.ShouldBe(task.Title);
        responseTask.Description.ShouldBeNull();
        responseTask.FolderId.ShouldBeNull();
        responseTask.CompletedDateTime.ShouldBeNull();
        responseTask.DueDateTime.ShouldBe(task.DueDateTime);
        responseTask.MovedToTrashDateTime.ShouldBe(utcNow);
        responseTask.TagIds.ShouldBeEmpty();
        responseTask.CreatedDateTime.ShouldBe(task.CreatedDateTime);
        responseTask.ModifiedDateTime.ShouldBe(utcNow);

        var dbTasks = await GetTasksFromDatabase();
        dbTasks.Count.ShouldBe(1);

        var dbTask = dbTasks.Single();
        dbTask.Id.ShouldBe(task.Id);
        dbTask.Title.ShouldBe(task.Title);
        dbTask.Description.ShouldBeNull();
        dbTask.FolderId.ShouldBeNull();
        dbTask.CompletedDateTime.ShouldBeNull();
        dbTask.DueDateTime.ShouldBe(task.DueDateTime);
        dbTask.MovedToTrashDateTime.ShouldBe(utcNow);
        dbTask.Tags.ShouldBeEmpty();
        dbTask.CreatedDateTime.ShouldBe(task.CreatedDateTime);
        dbTask.ModifiedDateTime.ShouldBe(utcNow);
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
            requestUri: $"/api/tasks/{NON_EXISTENT_TASK_ID}/movedToTrash",
            moveToTrashDto,
            TestContext.Current.CancellationToken);

        // Assert
        await AssertResponseWithDomainProblemDetails(response);
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
            requestUri: $"/api/tasks/{task.Id}/movedFromTrash",
            moveFromTrashDto,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var responseTask = await response.Content.ReadFromJsonAsync<UserTaskVm>(
            TestContext.Current.CancellationToken);

        responseTask.ShouldNotBeNull();
        responseTask.Id.ShouldBe(task.Id);
        responseTask.Title.ShouldBe(task.Title);
        responseTask.Description.ShouldBeNull();
        responseTask.FolderId.ShouldBe(folder.Id);
        responseTask.CompletedDateTime.ShouldBeNull();
        responseTask.DueDateTime.ShouldBe(task.DueDateTime);
        responseTask.MovedToTrashDateTime.ShouldBeNull();
        responseTask.TagIds.ShouldBeEmpty();
        responseTask.CreatedDateTime.ShouldBe(task.CreatedDateTime);
        responseTask.ModifiedDateTime.ShouldBe(utcNow);

        var dbTasks = await GetTasksFromDatabase();
        dbTasks.Count.ShouldBe(1);

        var dbTask = dbTasks.Single();
        dbTask.Id.ShouldBe(task.Id);
        dbTask.Title.ShouldBe(task.Title);
        dbTask.Description.ShouldBeNull();
        dbTask.FolderId.ShouldBe(folder.Id);
        dbTask.CompletedDateTime.ShouldBeNull();
        dbTask.DueDateTime.ShouldBe(task.DueDateTime);
        dbTask.MovedToTrashDateTime.ShouldBeNull();
        dbTask.Tags.ShouldBeEmpty();
        dbTask.CreatedDateTime.ShouldBe(task.CreatedDateTime);
        dbTask.ModifiedDateTime.ShouldBe(utcNow);
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
            requestUri: $"/api/tasks/{NON_EXISTENT_TASK_ID}/movedFromTrash",
            moveToTrashDto,
            TestContext.Current.CancellationToken);

        // Assert
        await AssertResponseWithDomainProblemDetails(response);
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
            requestUri: $"/api/tasks/{task.Id}/movedFromTrash",
            moveFromTrashDto,
            TestContext.Current.CancellationToken);

        // Assert
        await AssertResponseWithDomainProblemDetails(response);

        var dbTasks = await GetTasksFromDatabase();
        dbTasks.Count.ShouldBe(1);

        var dbTask = dbTasks.Single();
        dbTask.Id.ShouldBe(task.Id);
        dbTask.Title.ShouldBe(task.Title);
        dbTask.Description.ShouldBeNull();
        dbTask.FolderId.ShouldBeNull();
        dbTask.CompletedDateTime.ShouldBeNull();
        dbTask.DueDateTime.ShouldBe(task.DueDateTime);
        dbTask.MovedToTrashDateTime.ShouldBe(task.MovedToTrashDateTime);
        dbTask.Tags.ShouldBeEmpty();
        dbTask.CreatedDateTime.ShouldBe(task.CreatedDateTime);
        dbTask.ModifiedDateTime.ShouldBe(task.ModifiedDateTime);
    }

    [Fact]
    public async Task DeleteTaskWhenTaskExists()
    {
        // Arrange
        var taskToDelete = await CreateTaskInTrashInDatabase();
        var otherTask = await CreateTaskInTrashInDatabase();

        // Act
        var response = await Client.DeleteAsync(
            requestUri: $"/api/tasks/{taskToDelete.Id}",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var isDeleted = await response.Content.ReadFromJsonAsync<bool>(
            TestContext.Current.CancellationToken);

        isDeleted.ShouldBeTrue();

        var dbTasks = await GetTasksFromDatabase();
        dbTasks.Count.ShouldBe(1);
        dbTasks.Single().Id.ShouldBe(otherTask.Id);
    }

    [Fact]
    public async Task DeleteTaskWhenTaskNotExists()
    {
        // Arrange
        const int NON_EXISTENT_TASK_ID = 1;

        // Act
        var response = await Client.DeleteAsync(
            requestUri: $"/api/tasks/{NON_EXISTENT_TASK_ID}",
            TestContext.Current.CancellationToken);

        // Assert
        await AssertResponseWithDomainProblemDetails(response);
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

    private async Task<Tag> CreateTagInDatabase()
    {
        var anyDateTime = new DateTime();
        var creationDto = new TagForCreationDto(
            Title: "Tag title 42", Color: "424242", anyDateTime);
        var tag = Tag.CreateTag(creationDto, anyDateTime);

        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        dbContext.Tags.Add(tag);
        await dbContext.SaveChangesAsync();

        return tag;
    }

    private async Task<IReadOnlyList<UserTask>> GetTasksFromDatabase()
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        return await dbContext.Tasks.Include(t => t.Tags).ToListAsync();
    }
    #endregion
}
