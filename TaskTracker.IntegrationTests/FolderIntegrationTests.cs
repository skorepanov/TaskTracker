using System.Net.Http.Json;
using FluentAssertions;
using TaskTracker.Web.Models;
using Xunit;

namespace TaskTracker.IntegrationTests;

public class FolderIntegrationTests(ApiWebApplicationFactory factory)
    : IntegrationTest(factory)
{
    [Fact]
    public async Task GetFolder()
    {
        // Arrange

        // Act
        var response = await Client
            .GetFromJsonAsync<ApiResponse<FolderVm[]>>(
                "/api/folders");

        // Assert
        response.Should().NotBeNull();
        response.IsOk.Should().BeTrue();
        response.Error.Should().BeNull();
    }
}