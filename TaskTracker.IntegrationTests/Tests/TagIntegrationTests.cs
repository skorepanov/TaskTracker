namespace TaskTracker.IntegrationTests.Tests;

public class TagIntegrationTests(ApiWebApplicationFactory factory)
    : IntegrationTestBase(factory)
{
    [Fact]
    public async Task GetTagByIdWhenTagExists()
    {
        // Arrange
        var createdDateTime = new DateTime(
            year: 2025, month: 7, day: 1, hour: 1, minute: 1, second: 1,
            DateTimeKind.Utc);
        var tag = await CreateTagInDatabase(
            title: "Tag title", color: "123456", createdDateTime);

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
    public async Task GetTagByIdWhenTagNotExists()
    {
        // Arrange
        const int NON_EXISTENT_TAG_ID = 1;

        // Act
        var response = await Client.GetAsync(
            requestUri: $"/api/tags/{NON_EXISTENT_TAG_ID}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Result<TagVm>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeFalse();
        content.Error.Should().NotBeNullOrWhiteSpace();
        content.Value.Should().BeNull();
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
    public async Task CreateTagWithValidData()
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
    public async Task CreateTagWithInvalidData()
    {
        // Arrange
        const string INVALID_TITLE = "   \t   \n   ";
        const string INVALID_COLOR = "   \t   \n   ";
        var anyDateTime = new DateTime();
        var creationDto = new TagForCreationDto(
            INVALID_TITLE, INVALID_COLOR, CreatedDateTime: anyDateTime);

        // Act
        var response = await Client.PostAsJsonAsync(requestUri: "/api/tags", creationDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Result<TagVm>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeFalse();
        content.Error.Should().NotBeNullOrWhiteSpace();
        content.Value.Should().BeNull();

        var dbTags = await GetTagsFromDatabase();
        dbTags.Should().BeEmpty();
    }

    [Fact]
    public async Task UpdateTagWithValidDataWhenTagExists()
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
    public async Task UpdateTagWhenTagNotExists()
    {
        // Arrange
        const int NON_EXISTENT_TAG_ID = 1;

        var anyDateTime = new DateTime();
        var updateDto = new TagForUpdateDto(
            Title: "New folder title", Color: "424242", ModifiedDateTime: anyDateTime);

        // Act
        var response = await Client.PutAsJsonAsync(
            requestUri: $"/api/tags/{NON_EXISTENT_TAG_ID}", updateDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Result<TagVm>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeFalse();
        content.Error.Should().NotBeNullOrWhiteSpace();
        content.Value.Should().BeNull();
    }

    [Fact]
    public async Task UpdateTagWithInvalidData()
    {
        // Arrange
        const string OLD_TITLE = "Old tag title";
        const string OLD_COLOR = "111111";
        var createdDateTime = new DateTime(
            year: 2025, month: 7, day: 1, hour: 1, minute: 1, second: 1,
            DateTimeKind.Utc);
        var tag = await CreateTagInDatabase(OLD_TITLE, OLD_COLOR, createdDateTime);

        const string INVALID_TITLE = "   \t   \n   ";
        const string INVALID_COLOR = "   \t   \n   ";
        var anyDateTime = new DateTime();
        var updateDto = new TagForUpdateDto(
            INVALID_TITLE, INVALID_COLOR, ModifiedDateTime: anyDateTime);

        // Act
        var response = await Client.PutAsJsonAsync(
            requestUri: $"/api/tags/{tag.Id}", updateDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Result<TagVm>>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeFalse();
        content.Error.Should().NotBeNullOrWhiteSpace();
        content.Value.Should().BeNull();

        var dbTags = await GetTagsFromDatabase();
        dbTags.Should().HaveCount(1);

        var dbTag = dbTags.Single();
        dbTag.Id.Should().Be(tag.Id);
        dbTag.Title.Should().Be(OLD_TITLE);
        dbTag.Color.Should().Be(OLD_COLOR);
        dbTag.CreatedDateTime.Should().Be(createdDateTime);
        dbTag.ModifiedDateTime.Should().BeNull();
    }

    [Fact]
    public async Task DeleteTagWhenTagExists()
    {
        // Arrange
        var tagToDelete = await CreateTagInDatabase();
        var otherTag = await CreateTagInDatabase();

        // Act
        var response = await Client
            .DeleteAsync(requestUri: $"/api/tags/{tagToDelete.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Result>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeTrue();
        content.Error.Should().BeNull();

        var dbTags = await GetTagsFromDatabase();
        dbTags.Should().HaveCount(1);
        dbTags.Single().Id.Should().Be(otherTag.Id);
    }

    [Fact]
    public async Task DeleteTagWhenTagNotExists()
    {
        // Arrange
        const int NON_EXISTENT_TAG_ID = 1;

        // Act
        var response = await Client
            .DeleteAsync(requestUri: $"/api/tags/{NON_EXISTENT_TAG_ID}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Result>();

        content.Should().NotBeNull();
        content.IsOk.Should().BeFalse();
        content.Error.Should().NotBeNullOrWhiteSpace();
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
