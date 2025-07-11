namespace TaskTracker.UnitTests.Tests;

public class TagUnitTests
{
    #region Create tag
    [Fact]
    public void CreateTagWithFieldNormalization()
    {
        // Arrange
        const string TITLE = "Tag title";
        const string COLOR = "424242";
        var createdDateTime = new DateTime(year: 2025, month: 5, day: 28);

        var tagDto = new TagForCreationDto(
            Title: $"   {TITLE}    ",
            Color: $"   {COLOR}    ",
            createdDateTime);

        var anyDateTime = new DateTime();

        // Act
        var sut = Tag.CreateTag(tagDto, now: anyDateTime);

        // Assert
        sut.IsOk.Should().BeTrue();
        sut.Error.Should().BeNull();

        sut.Value.Should().NotBeNull();
        sut.Value.Title.Should().Be(TITLE);
        sut.Value.Color.Should().Be(COLOR);
        sut.Value.CreatedDateTime.Should().Be(createdDateTime);
        sut.Value.ModifiedDateTime.Should().BeNull();
    }

    [Fact]
    public void CreateTagWithoutTitle()
    {
        // Arrange
        var tagDto = new TagForCreationDto(
            Title: "   \t   \n   ",
            Color: "424242",
            CreatedDateTime: null);

        var anyDateTime = new DateTime();

        // Act
        var sut = Tag.CreateTag(tagDto, now: anyDateTime);

        // Assert
        sut.IsOk.Should().BeFalse();
        sut.Value.Should().BeNull();
        sut.Error.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void CreateTagWithEmptyColor()
    {
        // Arrange
        var tagDto = new TagForCreationDto(
            Title: "Tag title",
            Color: "   \t   \n   ",
            CreatedDateTime: null);

        var anyDateTime = new DateTime();

        // Act
        var sut = Tag.CreateTag(tagDto, now: anyDateTime);

        // Assert
        sut.IsOk.Should().BeFalse();
        sut.Value.Should().BeNull();
        sut.Error.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void CreateTagWithoutCreatedDateTime()
    {
        // Arrange
        var tagDto = new TagForCreationDto(
            Title: "Tag title 42",
            Color: "424242",
            CreatedDateTime: null);

        var now = new DateTime(year: 2025, month: 5, day: 28);

        // Act
        var sut = Tag.CreateTag(tagDto, now);

        // Assert
        sut.IsOk.Should().BeTrue();
        sut.Error.Should().BeNull();

        sut.Value.Should().NotBeNull();
        sut.Value.CreatedDateTime.Should().Be(now);
    }

    [Fact]
    public void CreateTagWithCreatedDateTimeNotEqualsToNow()
    {
        // Arrange
        var createdDateTime = new DateTime(year: 2025, month: 1, day: 1);
        var now = new DateTime(year: 2025, month: 1, day: 2);

        var tagDto = new TagForCreationDto(
            Title: "Tag title 42",
            Color: "424242",
            CreatedDateTime: createdDateTime);

        // Act
        var sut = Tag.CreateTag(tagDto, now);

        // Assert
        sut.IsOk.Should().BeTrue();
        sut.Error.Should().BeNull();

        sut.Value.Should().NotBeNull();
        sut.Value.CreatedDateTime.Should().Be(createdDateTime);
    }
    #endregion

    #region Update tag
    [Fact]
    public void UpdateTagWithFieldNormalization()
    {
        // Arrange
        var sut = CreateSut(title: "Old tag title", color: "000000");

        const string NEW_TITLE = "New tag title";
        const string NEW_COLOR = "111111";
        var modifiedDateTime = new DateTime(year: 2025, month: 5, day: 29);

        var tagDto = new TagForUpdateDto(
            Title: $"   {NEW_TITLE}    ",
            Color: $"   {NEW_COLOR}    ",
            modifiedDateTime);

        // Act
        var result = sut.UpdateTag(tagDto, modifiedDateTime);

        // Assert
        result.IsOk.Should().BeTrue();
        result.Error.Should().BeNull();

        sut.Title.Should().Be(NEW_TITLE);
        sut.Color.Should().Be(NEW_COLOR);
        sut.ModifiedDateTime.Should().Be(modifiedDateTime);
    }

    [Fact]
    public void UpdateTagWithoutTitle()
    {
        // Arrange
        const string OLD_TITLE = "Old tag title";
        const string OLD_COLOR = "000000";
        var sut = CreateSut(OLD_TITLE, OLD_COLOR);

        var tagDto = new TagForUpdateDto(
            Title: "   \t   \n   ",
            Color: "111111",
            ModifiedDateTime: null);

        var anyDateTime = new DateTime();

        // Act
        var result = sut.UpdateTag(tagDto, now: anyDateTime);

        // Assert
        result.IsOk.Should().BeFalse();
        result.Error.Should().NotBeNullOrWhiteSpace();

        sut.Title.Should().Be(OLD_TITLE);
        sut.Color.Should().Be(OLD_COLOR);
        sut.ModifiedDateTime.Should().BeNull();
    }

    [Fact]
    public void UpdateTagWithEmptyColor()
    {
        // Arrange
        const string OLD_TITLE = "Old tag title";
        const string OLD_COLOR = "000000";
        var sut = CreateSut(OLD_TITLE, OLD_COLOR);

        var tagDto = new TagForUpdateDto(
            Title: "New tag title",
            Color: "   \t   \n   ",
            ModifiedDateTime: null);

        var anyDateTime = new DateTime();

        // Act
        var result = sut.UpdateTag(tagDto, now: anyDateTime);

        // Assert
        result.IsOk.Should().BeFalse();
        result.Error.Should().NotBeNullOrWhiteSpace();

        sut.Title.Should().Be(OLD_TITLE);
        sut.Color.Should().Be(OLD_COLOR);
        sut.ModifiedDateTime.Should().BeNull();
    }

    [Fact]
    public void UpdateTagWithoutModifiedDateTime()
    {
        // Arrange
        var sut = CreateSut();

        var now = new DateTime(year: 2025, month: 5, day: 29);

        var tagDto = new TagForUpdateDto(
            Title: "Tag title 42",
            Color: "424242",
            ModifiedDateTime: null);

        // Act
        var result = sut.UpdateTag(tagDto, now);

        // Assert
        result.IsOk.Should().BeTrue();
        result.Error.Should().BeNull();

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
            Color: "424242",
            modifiedDateTime);

        // Act
        var result = sut.UpdateTag(tagDto, now);

        // Assert
        result.IsOk.Should().BeTrue();
        result.Error.Should().BeNull();

        sut.ModifiedDateTime.Should().Be(modifiedDateTime);
    }
    #endregion

    #region helpers
    private Tag CreateSut(
        string title = "Tag title 42",
        string color = "424242",
        DateTime? createdDateTime = null,
        DateTime? now = null)
    {
        createdDateTime ??= new DateTime(year: 2025, month: 1, day: 1);
        now ??= new DateTime(year: 2025, month: 1, day: 2);

        var tagDto = new TagForCreationDto(title, color, createdDateTime);

        var tagResult = Tag.CreateTag(tagDto, now.Value);
        return tagResult.Value;
    }
    #endregion
}