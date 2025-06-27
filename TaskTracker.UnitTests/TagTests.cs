namespace TaskTracker.UnitTests;

public class TagTests
{
    #region Create tag
    [Fact]
    public void CreateTagWithFieldNormalization()
    {
        // Arrange
        const string TITLE = "Tag title";
        const string COLOR = "#424242";
        var createdDateTime = new DateTime(year: 2025, month: 5, day: 28);

        var tagDto = new TagForCreationDto(
            Title: $"   {TITLE}    ",
            Color: $"   {COLOR}    ",
            createdDateTime);

        // Act
        var sut = Tag.CreateTag(tagDto, now: It.IsAny<DateTime>());

        // Assert
        sut.Title.Should().Be(TITLE);
        sut.Color.Should().Be(COLOR);
        sut.CreatedDateTime.Should().Be(createdDateTime);
        sut.ModifiedDateTime.Should().BeNull();
    }

    [Fact]
    public void CreateTagWithoutCreatedDateTime()
    {
        // Arrange
        var tagDto = new TagForCreationDto(
            Title: "Tag title 42",
            Color: "#424242",
            CreatedDateTime: null);

        var now = new DateTime(year: 2025, month: 5, day: 28);

        // Act
        var sut = Tag.CreateTag(tagDto, now);

        // Assert
        sut.CreatedDateTime.Should().Be(now);
    }

    [Fact]
    public void CreateTagWithCreatedDateTimeNotEqualsToNow()
    {
        // Arrange
        var createdDateTime = new DateTime(year: 2025, month: 1, day: 1);
        var now = new DateTime(year: 2025, month: 1, day: 2);

        var tagDto = new TagForCreationDto(
            Title: "Tag title 42",
            Color: "#424242",
            CreatedDateTime: createdDateTime);

        // Act
        var sut = Tag.CreateTag(tagDto, now);

        // Assert
        sut.CreatedDateTime.Should().Be(createdDateTime);
    }
    #endregion

    #region Update tag
    [Fact]
    public void UpdateTagWithFieldNormalization()
    {
        // Arrange
        var sut = CreateSut(title: "Old tag title", color: "#000000");

        const string NEW_TITLE = "New tag title";
        const string NEW_COLOR = "#111111";
        var modifiedDateTime = new DateTime(year: 2025, month: 5, day: 29);

        var tagDto = new TagForUpdateDto(
            Title: $"   {NEW_TITLE}    ",
            Color: $"   {NEW_COLOR}    ",
            modifiedDateTime);

        // Act
        sut.UpdateTag(tagDto, modifiedDateTime);

        // Assert
        sut.Title.Should().Be(NEW_TITLE);
        sut.Color.Should().Be(NEW_COLOR);
        sut.ModifiedDateTime.Should().Be(modifiedDateTime);
    }

    [Fact]
    public void UpdateTagWithoutModifiedDateTime()
    {
        // Arrange
        var sut = CreateSut();

        var now = new DateTime(year: 2025, month: 5, day: 29);

        var tagDto = new TagForUpdateDto(
            Title: "Tag title 42",
            Color: "#424242",
            ModifiedDateTime: null);

        // Act
        sut.UpdateTag(tagDto, now);

        // Assert
        sut.ModifiedDateTime.Should().Be(now);
    }

    [Fact]
    public void UpdateTagWithModifiedDateTimeNotEqualsToNow()
    {
        // Arrange
        var modifiedDateTime = new DateTime(year: 2025, month: 1, day: 1);
        var now = new DateTime(year: 2025, month: 1, day: 2);

        var sut = CreateSut();

        var tagDto = new TagForUpdateDto(
            Title: "Tag title 42",
            Color: "#424242",
            modifiedDateTime);

        // Act
        sut.UpdateTag(tagDto, now);

        // Assert
        sut.ModifiedDateTime.Should().Be(modifiedDateTime);
    }
    #endregion

    #region helpers
    private Tag CreateSut(
        string title = "Tag title 42",
        string color = "#424242",
        DateTime? createdDateTime = null,
        DateTime? now = null)
    {
        createdDateTime ??= new DateTime(year: 2025, month: 1, day: 1);
        now ??= new DateTime(year: 2025, month: 1, day: 2);

        var tagDto = new TagForCreationDto(title, color, createdDateTime);

        return Tag.CreateTag(tagDto, now.Value);
    }
    #endregion
}