using System.Net.Http.Json;
using FluentAssertions;
using TaskTracker.Web.Models;
using Xunit;

namespace TaskTracker.IntegrationTests;

[Collection("IntegrationTests")]
public class FolderIntegrationTests(ApiWebApplicationFactory factory)
    : IntegrationTestBase(factory)
{
    [Fact]
    public async Task GetFolders()
    {
        // Arrange

        // Act
        var response = await Client.GetFromJsonAsync<ApiResponse<FolderVm[]>>(
            requestUri: "/api/folders");

        // Assert
        response.Should().NotBeNull();
        response.IsOk.Should().BeTrue();
        response.Error.Should().BeNull();
    }
}