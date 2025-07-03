using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskTracker.Bll.Models;
using TaskTracker.Dal;
using TaskTracker.Web.Models;
using Xunit;

namespace TaskTracker.IntegrationTests;

public class FolderIntegrationTests(ApiWebApplicationFactory factory)
    : IntegrationTestBase(factory)
{
    [Fact]
    public async Task GetFolderById()
    {
        var createdDateTime = new DateTime(year: 2025, month: 7, day: 1,
            hour: 1, minute: 1, second: 1, DateTimeKind.Utc);
        var folder = await CreateFolder(title: "Folder title", createdDateTime);

        var otherFolder = await CreateFolder();

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
        var createdDateTime1 = new DateTime(year: 2025, month: 7, day: 1,
            hour: 1, minute: 1, second: 1, DateTimeKind.Utc);
        var folder1 = await CreateFolder(title: "Folder title 1", createdDateTime1);

        var createdDateTime2 = new DateTime(year: 2025, month: 7, day: 1,
            hour: 1, minute: 1, second: 2, DateTimeKind.Utc);
        var folder2 = await CreateFolder(title: "Folder title 2", createdDateTime2);

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
    public async Task DeleteFolder()
    {
        var folderToDelete = await CreateFolder();
        var otherFolder = await CreateFolder();

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

        var dbFolders = await GetFoldersFromDb();
        dbFolders.Should().NotBeNull().And.HaveCount(1);
        dbFolders.Single().Id.Should().Be(otherFolder.Id);
    }

    #region helpers
    private async Task<Folder> CreateFolder(
        string title = "Folder title 42",
        DateTime? createdDateTime = null,
        DateTime? now = null)
    {
        createdDateTime ??= new DateTime(
            year: 2025, month: 1, day: 1, hour: 1, minute: 1, second: 1, DateTimeKind.Utc);

        now ??= new DateTime(
            year: 2025, month: 1, day: 1, hour: 1, minute: 1, second: 2, DateTimeKind.Utc);

        var folderDto = new FolderForCreationDto(title, createdDateTime.Value);
        var folder = Folder.CreateFolder(folderDto, now.Value);

        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        dbContext.Folders.Add(folder);
        await dbContext.SaveChangesAsync();

        return folder;
    }

    private async Task<IReadOnlyList<Folder>> GetFoldersFromDb()
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        return await dbContext.Folders.ToListAsync();
    }
    #endregion
}
