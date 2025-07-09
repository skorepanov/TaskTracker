namespace TaskTracker.IntegrationTests.Tests;

public class TagIntegrationTests(ApiWebApplicationFactory factory)
    : IntegrationTestBase(factory)
{
    [Fact]
    public async Task GetTagById()
    {
        // Arrange
        var createdDateTime = new DateTime(
            year: 2025, month: 7, day: 1, hour: 1, minute: 1, second: 1,
            DateTimeKind.Utc);
        var tag = await CreateTagInDatabase(title: "Tag title", color: "123456", createdDateTime);

        var otherTag = await CreateTagInDatabase();

        // Act
        var response = await Client.GetAsync(requestUri: $"/api/tags/{tag.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Result<TagVm>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeTrue();
        content.Error.Should().BeNull();

        var responseTag = content.Value;
        responseTag.Should().NotBeNull();
        responseTag.Id.Should().Be(tag.Id);
        responseTag.Title.Should().Be(tag.Title);
        responseTag.Color.Should().Be(tag.Color);
        responseTag.CreatedDateTime.Should().Be(tag.CreatedDateTime);
        responseTag.ModifiedDateTime.Should().BeNull();
    }

    [Fact]
    public async Task GetTags()
    {
        // Arrange
        var createdDateTime1 = new DateTime(
            year: 2025, month: 7, day: 1, hour: 1, minute: 1, second: 1,
            DateTimeKind.Utc);
        var tag1 = await CreateTagInDatabase(
            title: "Tag title 1", color: "111111", createdDateTime1);

        var createdDateTime2 = new DateTime(
            year: 2025, month: 7, day: 1, hour: 1, minute: 1, second: 2,
            DateTimeKind.Utc);
        var tag2 = await CreateTagInDatabase(
            title: "Tag title 2", color: "222222", createdDateTime2);

        // Act
        var response = await Client.GetAsync(requestUri: "/api/tags");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content
            .ReadFromJsonAsync<Result<IReadOnlyList<TagVm>>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeTrue();
        content.Error.Should().BeNull();
        content.Value.Should().NotBeNull().And.HaveCount(2);

        var responseTag1 = content.Value.SingleOrDefault(t => t.Id == tag1.Id);
        responseTag1.Should().NotBeNull();
        responseTag1.Title.Should().Be(tag1.Title);
        responseTag1.Color.Should().Be(tag1.Color);
        responseTag1.CreatedDateTime.Should().Be(tag1.CreatedDateTime);
        responseTag1.ModifiedDateTime.Should().BeNull();

        var responseTag2 = content.Value.SingleOrDefault(t => t.Id == tag2.Id);
        responseTag2.Should().NotBeNull();
        responseTag2.Title.Should().Be(tag2.Title);
        responseTag2.Color.Should().Be(tag2.Color);
        responseTag2.CreatedDateTime.Should().Be(tag2.CreatedDateTime);
        responseTag2.ModifiedDateTime.Should().BeNull();
    }

    [Fact]
    public async Task CreateTag()
    {
        // Arrange
        var creationDto = new TagForCreationDto(
            Title: "Tag title", Color: "123456", CreatedDateTime: null);

        var utcNow = new DateTime(
            year: 2025, month: 7, day: 1, hour: 1, minute: 1, second: 1,
            DateTimeKind.Utc);
        MockDateTimeProvider.Setup(p => p.UtcNow).Returns(utcNow);

        // Act
        var response = await Client.PostAsJsonAsync(requestUri: "/api/tags", creationDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Result<TagVm>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeTrue();
        content.Error.Should().BeNull();

        var responseTag = content.Value;
        responseTag.Should().NotBeNull();
        responseTag.Title.Should().Be(creationDto.Title);
        responseTag.Color.Should().Be(creationDto.Color);
        responseTag.CreatedDateTime.Should().Be(utcNow);

        var dbTags = await GetTagsFromDatabase();
        dbTags.Should().HaveCount(1);

        var dbTag = dbTags.Single();
        dbTag.Id.Should().Be(responseTag.Id);
        dbTag.Title.Should().Be(creationDto.Title);
        dbTag.Color.Should().Be(creationDto.Color);
        dbTag.CreatedDateTime.Should().Be(utcNow);
        dbTag.ModifiedDateTime.Should().BeNull();
    }

    [Fact]
    public async Task UpdateTag()
    {
        // Arrange
        var createdDateTime = new DateTime(
            year: 2025, month: 7, day: 1, hour: 1, minute: 1, second: 1,
            DateTimeKind.Utc);
        var tag = await CreateTagInDatabase(
            title: "Old tag title", color: "111111", createdDateTime);

        var updateDto = new TagForUpdateDto(
            Title: "New tag title", Color: "222222", ModifiedDateTime: null);

        var utcNow = new DateTime(
            year: 2025, month: 7, day: 1, hour: 1, minute: 1, second: 2,
            DateTimeKind.Utc);
        MockDateTimeProvider.Setup(p => p.UtcNow).Returns(utcNow);

        // Act
        var response = await Client.PutAsJsonAsync(
            requestUri: $"/api/tags/{tag.Id}", updateDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Result<TagVm>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeTrue();
        content.Error.Should().BeNull();

        var responseTag = content.Value;
        responseTag.Should().NotBeNull();
        responseTag.Id.Should().Be(tag.Id);
        responseTag.Title.Should().Be(updateDto.Title);
        responseTag.Color.Should().Be(updateDto.Color);
        responseTag.CreatedDateTime.Should().Be(tag.CreatedDateTime);
        responseTag.ModifiedDateTime.Should().Be(utcNow);

        var dbTags = await GetTagsFromDatabase();
        dbTags.Should().HaveCount(1);

        var dbTag = dbTags.Single();
        dbTag.Id.Should().Be(tag.Id);
        dbTag.Title.Should().Be(updateDto.Title);
        dbTag.Color.Should().Be(updateDto.Color);
        dbTag.CreatedDateTime.Should().Be(tag.CreatedDateTime);
        dbTag.ModifiedDateTime.Should().Be(utcNow);
    }

    [Fact]
    public async Task DeleteTag()
    {
        // Arrange
        var tagToDelete = await CreateTagInDatabase();
        var otherTag = await CreateTagInDatabase();

        // Act
        var response = await Client
            .DeleteAsync(requestUri: $"/api/tags/{tagToDelete.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Result<TagVm>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeTrue();
        content.Error.Should().BeNull();
        content.Value.Should().BeNull();

        var dbTags = await GetTagsFromDatabase();
        dbTags.Should().HaveCount(1);
        dbTags.Single().Id.Should().Be(otherTag.Id);
    }

    #region helpers
    private async Task<Tag> CreateTagInDatabase(
        string title = "Tag title 42",
        string color = "424242",
        DateTime? createdDateTime = null)
    {
        createdDateTime ??= new DateTime(
            year: 2025, month: 1, day: 1, hour: 1, minute: 1, second: 1,
            DateTimeKind.Utc);

        var creationDto = new TagForCreationDto(title, color, createdDateTime);
        var tag = Tag.CreateTag(creationDto, createdDateTime.Value);

        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        dbContext.Tags.Add(tag);
        await dbContext.SaveChangesAsync();

        return tag;
    }

    private async Task<IReadOnlyList<Tag>> GetTagsFromDatabase()
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        return await dbContext.Tags.ToListAsync();
    }
    #endregion
}
