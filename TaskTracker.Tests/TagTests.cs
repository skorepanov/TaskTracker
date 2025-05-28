namespace TaskTracker.Tests;

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
}