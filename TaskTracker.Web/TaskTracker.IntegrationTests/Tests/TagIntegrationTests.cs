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

        await CreateTagInDatabase();

        // Act
        var response = await Client.GetAsync(
            requestUri: $"/api/tags/{tag.Id}",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Result<TagVm>>(
            TestContext.Current.CancellationToken);

        content.ShouldNotBeNull();
        content.IsOk.ShouldBeTrue();
        content.Error.ShouldBeNull();

        var responseTag = content.Value;
        responseTag.ShouldNotBeNull();
        responseTag.Id.ShouldBe(tag.Id);
        responseTag.Title.ShouldBe(tag.Title);
        responseTag.Color.ShouldBe(tag.Color);
        responseTag.CreatedDateTime.ShouldBe(tag.CreatedDateTime);
        responseTag.ModifiedDateTime.ShouldBeNull();
    }

    [Fact]
    public async Task GetTagByIdWhenTagNotExists()
    {
        // Arrange
        const int NON_EXISTENT_TAG_ID = 1;

        // Act
        var response = await Client.GetAsync(
            requestUri: $"/api/tags/{NON_EXISTENT_TAG_ID}",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Result<TagVm>>(
            TestContext.Current.CancellationToken);

        content.ShouldNotBeNull();
        content.IsOk.ShouldBeFalse();
        content.Error.ShouldNotBeNullOrWhiteSpace();
        content.Value.ShouldBeNull();
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
        var response = await Client.GetAsync(
            requestUri: "/api/tags",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var content = await response.Content
            .ReadFromJsonAsync<Result<IReadOnlyList<TagVm>>>(
                TestContext.Current.CancellationToken);

        content.ShouldNotBeNull();
        content.IsOk.ShouldBeTrue();
        content.Error.ShouldBeNull();
        content.Value.ShouldNotBeNull();
        content.Value.Count.ShouldBe(2);

        var responseTag1 = content.Value.SingleOrDefault(t => t.Id == tag1.Id);
        responseTag1.ShouldNotBeNull();
        responseTag1.Title.ShouldBe(tag1.Title);
        responseTag1.Color.ShouldBe(tag1.Color);
        responseTag1.CreatedDateTime.ShouldBe(tag1.CreatedDateTime);
        responseTag1.ModifiedDateTime.ShouldBeNull();

        var responseTag2 = content.Value.SingleOrDefault(t => t.Id == tag2.Id);
        responseTag2.ShouldNotBeNull();
        responseTag2.Title.ShouldBe(tag2.Title);
        responseTag2.Color.ShouldBe(tag2.Color);
        responseTag2.CreatedDateTime.ShouldBe(tag2.CreatedDateTime);
        responseTag2.ModifiedDateTime.ShouldBeNull();
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
        var response = await Client.PostAsJsonAsync(
            requestUri: "/api/tags",
            creationDto,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Result<TagVm>>(
            TestContext.Current.CancellationToken);

        content.ShouldNotBeNull();
        content.IsOk.ShouldBeTrue();
        content.Error.ShouldBeNull();

        var responseTag = content.Value;
        responseTag.ShouldNotBeNull();
        responseTag.Title.ShouldBe(creationDto.Title);
        responseTag.Color.ShouldBe(creationDto.Color);
        responseTag.CreatedDateTime.ShouldBe(utcNow);

        var dbTags = await GetTagsFromDatabase();
        dbTags.Count.ShouldBe(1);

        var dbTag = dbTags.Single();
        dbTag.Id.ShouldBe(responseTag.Id);
        dbTag.Title.ShouldBe(creationDto.Title);
        dbTag.Color.ShouldBe(creationDto.Color);
        dbTag.CreatedDateTime.ShouldBe(utcNow);
        dbTag.ModifiedDateTime.ShouldBeNull();
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
        var response = await Client.PostAsJsonAsync(
            requestUri: "/api/tags",
            creationDto,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Result<TagVm>>(
            TestContext.Current.CancellationToken);

        content.ShouldNotBeNull();
        content.IsOk.ShouldBeFalse();
        content.Error.ShouldNotBeNullOrWhiteSpace();
        content.Value.ShouldBeNull();

        var dbTags = await GetTagsFromDatabase();
        dbTags.ShouldBeEmpty();
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
            requestUri: $"/api/tags/{tag.Id}",
            updateDto,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Result<TagVm>>(
            TestContext.Current.CancellationToken);

        content.ShouldNotBeNull();
        content.IsOk.ShouldBeTrue();
        content.Error.ShouldBeNull();

        var responseTag = content.Value;
        responseTag.ShouldNotBeNull();
        responseTag.Id.ShouldBe(tag.Id);
        responseTag.Title.ShouldBe(updateDto.Title);
        responseTag.Color.ShouldBe(updateDto.Color);
        responseTag.CreatedDateTime.ShouldBe(tag.CreatedDateTime);
        responseTag.ModifiedDateTime.ShouldBe(utcNow);

        var dbTags = await GetTagsFromDatabase();
        dbTags.Count.ShouldBe(1);

        var dbTag = dbTags.Single();
        dbTag.Id.ShouldBe(tag.Id);
        dbTag.Title.ShouldBe(updateDto.Title);
        dbTag.Color.ShouldBe(updateDto.Color);
        dbTag.CreatedDateTime.ShouldBe(tag.CreatedDateTime);
        dbTag.ModifiedDateTime.ShouldBe(utcNow);
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
            requestUri: $"/api/tags/{NON_EXISTENT_TAG_ID}",
            updateDto,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Result<TagVm>>(
            TestContext.Current.CancellationToken);

        content.ShouldNotBeNull();
        content.IsOk.ShouldBeFalse();
        content.Error.ShouldNotBeNullOrWhiteSpace();
        content.Value.ShouldBeNull();
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
            requestUri: $"/api/tags/{tag.Id}",
            updateDto,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Result<TagVm>>(
            TestContext.Current.CancellationToken);

        content.ShouldNotBeNull();
        content.IsOk.ShouldBeFalse();
        content.Error.ShouldNotBeNullOrWhiteSpace();
        content.Value.ShouldBeNull();

        var dbTags = await GetTagsFromDatabase();
        dbTags.Count.ShouldBe(1);

        var dbTag = dbTags.Single();
        dbTag.Id.ShouldBe(tag.Id);
        dbTag.Title.ShouldBe(OLD_TITLE);
        dbTag.Color.ShouldBe(OLD_COLOR);
        dbTag.CreatedDateTime.ShouldBe(createdDateTime);
        dbTag.ModifiedDateTime.ShouldBeNull();
    }

    [Fact]
    public async Task DeleteTagWhenTagExists()
    {
        // Arrange
        var tagToDelete = await CreateTagInDatabase();
        var otherTag = await CreateTagInDatabase();

        // Act
        var response = await Client.DeleteAsync(
            requestUri: $"/api/tags/{tagToDelete.Id}",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Result<bool>>(
            TestContext.Current.CancellationToken);

        content.ShouldNotBeNull();
        content.IsOk.ShouldBeTrue();
        content.Value.ShouldBeTrue();
        content.Error.ShouldBeNull();

        var dbTags = await GetTagsFromDatabase();
        dbTags.Count.ShouldBe(1);
        dbTags.Single().Id.ShouldBe(otherTag.Id);
    }

    [Fact]
    public async Task DeleteTagWhenTagNotExists()
    {
        // Arrange
        const int NON_EXISTENT_TAG_ID = 1;

        // Act
        var response = await Client.DeleteAsync(
            requestUri: $"/api/tags/{NON_EXISTENT_TAG_ID}",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Result<bool>>(
            TestContext.Current.CancellationToken);

        content.ShouldNotBeNull();
        content.IsOk.ShouldBeFalse();
        content.Value.ShouldBeFalse();
        content.Error.ShouldNotBeNullOrWhiteSpace();
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
