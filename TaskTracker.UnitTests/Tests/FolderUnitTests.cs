namespace TaskTracker.UnitTests.Tests;

public class FolderUnitTests
{
    #region Create folder
    [Fact]
    public void CreateFolderWithFieldNormalization()
    {
        // Arrange
        const string TITLE = "Folder title";
        var createdDateTime = new DateTime(year: 2025, month: 4, day: 24);

        var folderDto = new FolderForCreationDto(
            Title: $"   {TITLE}    ",
            createdDateTime);

        var anyDateTime = new DateTime();

        // Act
        var sut = Folder.CreateFolder(folderDto, now: anyDateTime);

        // Assert
        sut.IsOk.Should().BeTrue();
        sut.Error.Should().BeNull();

        sut.Value.Should().NotBeNull();
        sut.Value.Title.Should().Be(TITLE);
        sut.Value.CreatedDateTime.Should().Be(createdDateTime);
        sut.Value.ModifiedDateTime.Should().BeNull();
    }

    [Fact]
    public void CreateFolderWithoutTitle()
    {
        // Arrange
        var folderDto = new FolderForCreationDto(
            Title: "   \t   \n   ",
            CreatedDateTime: null);

        var anyDateTime = new DateTime();

        // Act
        var sut = Folder.CreateFolder(folderDto, now: anyDateTime);

        // Assert
        sut.IsOk.Should().BeFalse();
        sut.Value.Should().BeNull();
        sut.Error.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void CreateFolderWithoutCreatedDateTime()
    {
        // Arrange
        var folderDto = new FolderForCreationDto(
            Title: "Folder Title 42",
            CreatedDateTime: null);

        var now = new DateTime(year: 2025, month: 4, day: 24);

        // Act
        var sut = Folder.CreateFolder(folderDto, now);

        // Assert
        sut.IsOk.Should().BeTrue();
        sut.Error.Should().BeNull();

        sut.Value.Should().NotBeNull();
        sut.Value.CreatedDateTime.Should().Be(now);
    }

    [Fact]
    public void CreateFolderWithCreatedDateTimeNotEqualsToNow()
    {
        // Arrange
        var createdDateTime = new DateTime(year: 2025, month: 1, day: 1);
        var now = new DateTime(year: 2025, month: 1, day: 2);

        var folderDto = new FolderForCreationDto(
            Title: "Folder Title 42",
            createdDateTime);

        // Act
        var sut = Folder.CreateFolder(folderDto, now);

        // Assert
        sut.IsOk.Should().BeTrue();
        sut.Error.Should().BeNull();

        sut.Value.Should().NotBeNull();
        sut.Value.CreatedDateTime.Should().Be(createdDateTime);
    }
    #endregion

    #region Update folder
    [Fact]
    public void UpdateFolderWithFieldNormalization()
    {
        // Arrange
        var sut = CreateSut(title: "Old folder title");

        const string NEW_TITLE = "New folder title";
        var modifiedDateTime = new DateTime(year: 2025, month: 4, day: 28);

        var folderDto = new FolderForUpdateDto(
            Title: $"   {NEW_TITLE}    ",
            modifiedDateTime);

        // Act
        var result = sut.UpdateFolder(folderDto, modifiedDateTime);

        // Assert
        result.IsOk.Should().BeTrue();
        result.Error.Should().BeNull();

        sut.Title.Should().Be(NEW_TITLE);
        sut.ModifiedDateTime.Should().Be(modifiedDateTime);
    }

    [Fact]
    public void UpdateFolderWithoutTitle()
    {
        // Arrange
        const string OLD_TITLE = "Old folder title";
        var sut = CreateSut(OLD_TITLE);

        var folderDto = new FolderForUpdateDto(
            Title: "   \t   \n   ",
            ModifiedDateTime: null);

        var anyDateTime = new DateTime();

        // Act
        var result = sut.UpdateFolder(folderDto, now: anyDateTime);

        // Assert
        result.IsOk.Should().BeFalse();
        result.Error.Should().NotBeNullOrWhiteSpace();

        sut.Title.Should().Be(OLD_TITLE);
        sut.ModifiedDateTime.Should().BeNull();
    }

    [Fact]
    public void UpdateFolderWithoutModifiedDateTime()
    {
        // Arrange
        var sut = CreateSut();

        var now = new DateTime(year: 2025, month: 4, day: 28);

        var folderDto = new FolderForUpdateDto(
            Title: "Folder title 42",
            ModifiedDateTime: null);

        // Act
        var result = sut.UpdateFolder(folderDto, now);

        // Assert
        result.IsOk.Should().BeTrue();
        result.Error.Should().BeNull();

        sut.ModifiedDateTime.Should().Be(now);
    }

    [Fact]
    public void UpdateFolderWithModifiedDateTimeNotEqualsToNow()
    {
        // Arrange
        var modifiedDateTime = new DateTime(year: 2025, month: 1, day: 1);
        var now = new DateTime(year: 2025, month: 1, day: 2);

        var sut = CreateSut();

        var folderDto = new FolderForUpdateDto(
            Title: "Folder title 42",
            modifiedDateTime);

        // Act
        var result = sut.UpdateFolder(folderDto, now);

        // Assert
        result.IsOk.Should().BeTrue();
        result.Error.Should().BeNull();

        sut.ModifiedDateTime.Should().Be(modifiedDateTime);
    }
    #endregion

    #region helpers
    private Folder CreateSut(
        string title = "Folder title 42",
        DateTime? createdDateTime = null,
        DateTime? now = null)
    {
        createdDateTime ??= new DateTime(year: 2025, month: 1, day: 1);
        now ??= new DateTime(year: 2025, month: 1, day: 2);

        var folderDto = new FolderForCreationDto(title, createdDateTime.Value);

        var folderResult = Folder.CreateFolder(folderDto, now.Value);
        return folderResult.Value;
    }
    #endregion
}
