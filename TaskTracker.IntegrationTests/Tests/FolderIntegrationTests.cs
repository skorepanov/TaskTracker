namespace TaskTracker.IntegrationTests.Tests;

public class FolderIntegrationTests(ApiWebApplicationFactory factory)
    : IntegrationTestBase(factory)
{
    [Fact]
    public async Task GetFolderById()
    {
        // Arrange
        var createdDateTime = new DateTime(
            year: 2025, month: 7, day: 1, hour: 1, minute: 1, second: 1,
            DateTimeKind.Utc);
        var folder = await CreateFolderInDatabase(title: "Folder title", createdDateTime);

        var otherFolder = await CreateFolderInDatabase();

        // Act
        var response = await Client.GetAsync(requestUri: $"/api/folders/{folder.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<ApiResponse<FolderVm>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeTrue();
        content.Error.Should().BeNull();

        var responseFolder = content.Result;
        responseFolder.Should().NotBeNull();
        responseFolder.Id.Should().Be(folder.Id);
        responseFolder.Title.Should().Be(folder.Title);
        responseFolder.CreatedDateTime.Should().Be(folder.CreatedDateTime);
        responseFolder.ModifiedDateTime.Should().BeNull();
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
        var response = await Client.GetAsync(requestUri: "/api/folders");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content
            .ReadFromJsonAsync<ApiResponse<IReadOnlyList<FolderVm>>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeTrue();
        content.Error.Should().BeNull();
        content.Result.Should().NotBeNull().And.HaveCount(2);

        var responseFolder1 = content.Result.SingleOrDefault(f => f.Id == folder1.Id);
        responseFolder1.Should().NotBeNull();
        responseFolder1.Title.Should().Be(folder1.Title);
        responseFolder1.CreatedDateTime.Should().Be(folder1.CreatedDateTime);
        responseFolder1.ModifiedDateTime.Should().BeNull();

        var responseFolder2 = content.Result.SingleOrDefault(f => f.Id == folder2.Id);
        responseFolder2.Should().NotBeNull();
        responseFolder2.Title.Should().Be(folder2.Title);
        responseFolder2.CreatedDateTime.Should().Be(folder2.CreatedDateTime);
        responseFolder2.ModifiedDateTime.Should().BeNull();
    }

    [Fact]
    public async Task CreateFolder()
    {
        // Arrange
        var creationDto = new FolderForCreationDto(Title: "Folder title", CreatedDateTime: null);

        var utcNow = new DateTime(
            year: 2025, month: 7, day: 1, hour: 1, minute: 1, second: 1,
            DateTimeKind.Utc);
        MockDateTimeProvider.Setup(p => p.UtcNow).Returns(utcNow);

        // Act
        var response = await Client.PostAsJsonAsync(requestUri: "api/folders", creationDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var content = await response.Content.ReadFromJsonAsync<ApiResponse<FolderVm>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeTrue();
        content.Error.Should().BeNull();

        var responseFolder = content.Result;
        responseFolder.Should().NotBeNull();
        responseFolder.Title.Should().Be(creationDto.Title);
        responseFolder.CreatedDateTime.Should().Be(utcNow);
        responseFolder.ModifiedDateTime.Should().BeNull();

        var dbFolders = await GetFoldersFromDatabase();
        dbFolders.Should().HaveCount(1);

        var dbFolder = dbFolders.Single();
        dbFolder.Id.Should().Be(responseFolder.Id);
        dbFolder.Title.Should().Be(creationDto.Title);
        dbFolder.CreatedDateTime.Should().Be(utcNow);
        dbFolder.ModifiedDateTime.Should().BeNull();
    }

    [Fact]
    public async Task UpdateFolder()
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
            requestUri: $"/api/folders/{folder.Id}", updateDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<ApiResponse<FolderVm>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeTrue();
        content.Error.Should().BeNull();

        var responseFolder = content.Result;
        responseFolder.Should().NotBeNull();
        responseFolder.Id.Should().Be(folder.Id);
        responseFolder.Title.Should().Be(updateDto.Title);
        responseFolder.CreatedDateTime.Should().Be(folder.CreatedDateTime);
        responseFolder.ModifiedDateTime.Should().Be(utcNow);

        var dbFolders = await GetFoldersFromDatabase();
        dbFolders.Should().HaveCount(1);

        var dbFolder = dbFolders.Single();
        dbFolder.Id.Should().Be(folder.Id);
        dbFolder.Title.Should().Be(updateDto.Title);
        dbFolder.CreatedDateTime.Should().Be(folder.CreatedDateTime);
        dbFolder.ModifiedDateTime.Should().Be(utcNow);
    }

    [Fact]
    public async Task DeleteFolder()
    {
        // Arrange
        var folderToDelete = await CreateFolderInDatabase();
        var otherFolder = await CreateFolderInDatabase();

        // Act
        var response = await Client
            .DeleteAsync(requestUri: $"/api/folders/{folderToDelete.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<ApiResponse<FolderVm>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeTrue();
        content.Error.Should().BeNull();
        content.Result.Should().BeNull();

        var dbFolders = await GetFoldersFromDatabase();
        dbFolders.Should().HaveCount(1);
        dbFolders.Single().Id.Should().Be(otherFolder.Id);
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
