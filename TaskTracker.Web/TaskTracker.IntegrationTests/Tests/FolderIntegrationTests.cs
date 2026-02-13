namespace TaskTracker.IntegrationTests.Tests;

public class FolderIntegrationTests(ApiWebApplicationFactory factory)
    : IntegrationTestBase(factory)
{
    [Fact]
    public async Task GetFolderByIdWhenFolderExists()
    {
        // Arrange
        var createdDateTime = new DateTime(
            year: 2025, month: 7, day: 1, hour: 1, minute: 1, second: 1,
            DateTimeKind.Utc);
        var folder = await CreateFolderInDatabase(title: "Folder title", createdDateTime);

        await CreateFolderInDatabase();

        // Act
        var response = await Client.GetAsync(
            requestUri: $"/api/folders/{folder.Id}",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var responseFolder = await response.Content.ReadFromJsonAsync<FolderVm>(
            TestContext.Current.CancellationToken);

        responseFolder.ShouldNotBeNull();
        responseFolder.Id.ShouldBe(folder.Id);
        responseFolder.Title.ShouldBe(folder.Title);
        responseFolder.CreatedDateTime.ShouldBe(folder.CreatedDateTime);
        responseFolder.ModifiedDateTime.ShouldBeNull();
    }

    [Fact]
    public async Task GetFolderByIdWhenFolderNotExists()
    {
        // Arrange
        const int NON_EXISTENT_FOLDER_ID = 1;

        // Act
        var response = await Client.GetAsync(
            requestUri: $"/api/folders/{NON_EXISTENT_FOLDER_ID}",
            TestContext.Current.CancellationToken);

        // Assert
        await AssertResponseWithDomainProblemDetails(response);
    }

    [Fact]
    public async Task GetFolders()
    {
        // Arrange
        var createdDateTime1 = new DateTime(
            year: 2025, month: 7, day: 1, hour: 1, minute: 1, second: 1,
            DateTimeKind.Utc);
        var folder1 = await CreateFolderInDatabase(title: "Folder title 1", createdDateTime1);

        var createdDateTime2 = new DateTime(
            year: 2025, month: 7, day: 1, hour: 1, minute: 1, second: 2,
            DateTimeKind.Utc);
        var folder2 = await CreateFolderInDatabase(title: "Folder title 2", createdDateTime2);

        // Act
        var response = await Client.GetAsync(
            requestUri: "/api/folders",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var responseFolders = await response.Content
            .ReadFromJsonAsync<IReadOnlyList<FolderVm>>(
                TestContext.Current.CancellationToken);

        responseFolders.ShouldNotBeNull();
        responseFolders.Count.ShouldBe(2);

        var responseFolder1 = responseFolders.SingleOrDefault(f => f.Id == folder1.Id);
        responseFolder1.ShouldNotBeNull();
        responseFolder1.Title.ShouldBe(folder1.Title);
        responseFolder1.CreatedDateTime.ShouldBe(folder1.CreatedDateTime);
        responseFolder1.ModifiedDateTime.ShouldBeNull();

        var responseFolder2 = responseFolders.SingleOrDefault(f => f.Id == folder2.Id);
        responseFolder2.ShouldNotBeNull();
        responseFolder2.Title.ShouldBe(folder2.Title);
        responseFolder2.CreatedDateTime.ShouldBe(folder2.CreatedDateTime);
        responseFolder2.ModifiedDateTime.ShouldBeNull();
    }

    [Fact]
    public async Task CreateFolderWithValidData()
    {
        // Arrange
        var creationDto = new FolderForCreationDto(Title: "Folder title", CreatedDateTime: null);

        var utcNow = new DateTime(
            year: 2025, month: 7, day: 1, hour: 1, minute: 1, second: 1,
            DateTimeKind.Utc);
        MockDateTimeProvider.Setup(p => p.UtcNow).Returns(utcNow);

        // Act
        var response = await Client.PostAsJsonAsync(
            requestUri: "api/folders",
            creationDto,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var responseFolder = await response.Content.ReadFromJsonAsync<FolderVm>(
            TestContext.Current.CancellationToken);

        responseFolder.ShouldNotBeNull();
        responseFolder.Title.ShouldBe(creationDto.Title);
        responseFolder.CreatedDateTime.ShouldBe(utcNow);
        responseFolder.ModifiedDateTime.ShouldBeNull();

        var dbFolders = await GetFoldersFromDatabase();
        dbFolders.Count.ShouldBe(1);

        var dbFolder = dbFolders.Single();
        dbFolder.Id.ShouldBe(responseFolder.Id);
        dbFolder.Title.ShouldBe(creationDto.Title);
        dbFolder.CreatedDateTime.ShouldBe(utcNow);
        dbFolder.ModifiedDateTime.ShouldBeNull();
    }

    [Fact]
    public async Task CreateFolderWithInvalidData()
    {
        // Arrange
        const string INVALID_TITLE = "   \t   \n   ";
        var anyDateTime = new DateTime();
        var creationDto = new FolderForCreationDto(
            INVALID_TITLE, CreatedDateTime: anyDateTime);

        // Act
        var response = await Client.PostAsJsonAsync(
            requestUri: "api/folders",
            creationDto,
            TestContext.Current.CancellationToken);

        // Assert
        await AssertResponseWithDomainProblemDetails(response);

        var dbFolders = await GetFoldersFromDatabase();
        dbFolders.ShouldBeEmpty();
    }

    [Fact]
    public async Task UpdateFolderWithValidDataWhenFolderExists()
    {
        // Arrange
        var createdDateTime = new DateTime(
            year: 2025, month: 7, day: 1, hour: 1, minute: 1, second: 1,
            DateTimeKind.Utc);
        var folder = await CreateFolderInDatabase(title: "Old folder title", createdDateTime);

        var updateDto = new FolderForUpdateDto(Title: "New folder title", ModifiedDateTime: null);

        var utcNow = new DateTime(
            year: 2025, month: 7, day: 1, hour: 1, minute: 1, second: 2,
            DateTimeKind.Utc);
        MockDateTimeProvider.Setup(p => p.UtcNow).Returns(utcNow);

        // Act
        var response = await Client.PutAsJsonAsync(
            requestUri: $"/api/folders/{folder.Id}",
            updateDto,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var responseFolder = await response.Content.ReadFromJsonAsync<FolderVm>(
            TestContext.Current.CancellationToken);

        responseFolder.ShouldNotBeNull();
        responseFolder.Id.ShouldBe(folder.Id);
        responseFolder.Title.ShouldBe(updateDto.Title);
        responseFolder.CreatedDateTime.ShouldBe(folder.CreatedDateTime);
        responseFolder.ModifiedDateTime.ShouldBe(utcNow);

        var dbFolders = await GetFoldersFromDatabase();
        dbFolders.Count.ShouldBe(1);

        var dbFolder = dbFolders.Single();
        dbFolder.Id.ShouldBe(folder.Id);
        dbFolder.Title.ShouldBe(updateDto.Title);
        dbFolder.CreatedDateTime.ShouldBe(folder.CreatedDateTime);
        dbFolder.ModifiedDateTime.ShouldBe(utcNow);
    }

    [Fact]
    public async Task UpdateFolderWhenFolderNotExists()
    {
        // Arrange
        const int NON_EXISTENT_FOLDER_ID = 1;

        var anyDateTime = new DateTime();
        var updateDto = new FolderForUpdateDto(
            Title: "New folder title", ModifiedDateTime: anyDateTime);

        // Act
        var response = await Client.PutAsJsonAsync(
            requestUri: $"/api/folders/{NON_EXISTENT_FOLDER_ID}",
            updateDto,
            TestContext.Current.CancellationToken);

        // Assert
        await AssertResponseWithDomainProblemDetails(response);
    }

    [Fact]
    public async Task UpdateFolderWithInvalidData()
    {
        // Arrange
        const string OLD_TITLE = "Old folder title";
        var createdDateTime = new DateTime(
            year: 2025, month: 7, day: 1, hour: 1, minute: 1, second: 1,
            DateTimeKind.Utc);
        var folder = await CreateFolderInDatabase(OLD_TITLE, createdDateTime);

        const string INVALID_TITLE = "   \t   \n   ";
        var anyDateTime = new DateTime();
        var updateDto = new FolderForUpdateDto(
            INVALID_TITLE, ModifiedDateTime: anyDateTime);

        // Act
        var response = await Client.PutAsJsonAsync(
            requestUri: $"/api/folders/{folder.Id}",
            updateDto,
            TestContext.Current.CancellationToken);

        // Assert
        await AssertResponseWithDomainProblemDetails(response);

        var dbFolders = await GetFoldersFromDatabase();
        dbFolders.Count.ShouldBe(1);

        var dbFolder = dbFolders.Single();
        dbFolder.Id.ShouldBe(folder.Id);
        dbFolder.Title.ShouldBe(OLD_TITLE);
        dbFolder.CreatedDateTime.ShouldBe(createdDateTime);
        dbFolder.ModifiedDateTime.ShouldBeNull();
    }

    [Fact]
    public async Task DeleteFolderWhenFolderExists()
    {
        // Arrange
        var folderToDelete = await CreateFolderInDatabase();
        var otherFolder = await CreateFolderInDatabase();

        // Act
        var response = await Client.DeleteAsync(
            requestUri: $"/api/folders/{folderToDelete.Id}",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var isDeleted = await response.Content.ReadFromJsonAsync<bool>(
            TestContext.Current.CancellationToken);

        isDeleted.ShouldBeTrue();

        var dbFolders = await GetFoldersFromDatabase();
        dbFolders.Count.ShouldBe(1);
        dbFolders.Single().Id.ShouldBe(otherFolder.Id);
    }

    [Fact]
    public async Task DeleteFolderWhenFolderNotExists()
    {
        // Arrange
        const int NON_EXISTENT_FOLDER_ID = 1;

        // Act
        var response = await Client.DeleteAsync(
            requestUri: $"/api/folders/{NON_EXISTENT_FOLDER_ID}",
            TestContext.Current.CancellationToken);

        // Assert
        await AssertResponseWithDomainProblemDetails(response);
    }

    #region helpers
    private async Task<Folder> CreateFolderInDatabase(
        string title = "Folder title 42",
        DateTime? createdDateTime = null)
    {
        createdDateTime ??= new DateTime(
            year: 2025, month: 1, day: 1, hour: 1, minute: 1, second: 1,
            DateTimeKind.Utc);

        var creationDto = new FolderForCreationDto(title, createdDateTime.Value);
        var folder = Folder.CreateFolder(creationDto, createdDateTime.Value);

        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        dbContext.Folders.Add(folder);
        await dbContext.SaveChangesAsync();

        return folder;
    }

    private async Task<IReadOnlyList<Folder>> GetFoldersFromDatabase()
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        return await dbContext.Folders.ToListAsync();
    }
    #endregion
}
