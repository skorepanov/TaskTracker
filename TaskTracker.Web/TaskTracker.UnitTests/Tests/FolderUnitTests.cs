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
        sut.Title.ShouldBe(TITLE);
        sut.CreatedDateTime.ShouldBe(createdDateTime);
        sut.ModifiedDateTime.ShouldBeNull();
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
        var action = () => Folder.CreateFolder(folderDto, now: anyDateTime);

        // Assert
        Should.Throw<DomainException>(action)
            .Message.ShouldNotBeNullOrWhiteSpace();
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
        sut.CreatedDateTime.ShouldBe(now);
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
        sut.CreatedDateTime.ShouldBe(createdDateTime);
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
        sut.UpdateFolder(folderDto, modifiedDateTime);

        // Assert
        sut.Title.ShouldBe(NEW_TITLE);
        sut.ModifiedDateTime.ShouldBe(modifiedDateTime);
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
        var action = () => sut.UpdateFolder(folderDto, now: anyDateTime);

        // Assert
        Should.Throw<DomainException>(action)
            .Message.ShouldNotBeNullOrWhiteSpace();

        sut.Title.ShouldBe(OLD_TITLE);
        sut.ModifiedDateTime.ShouldBeNull();
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
        sut.UpdateFolder(folderDto, now);

        // Assert
        sut.ModifiedDateTime.ShouldBe(now);
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
        sut.UpdateFolder(folderDto, now);

        // Assert
        sut.ModifiedDateTime.ShouldBe(modifiedDateTime);
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

        var folder = Folder.CreateFolder(folderDto, now.Value);
        return folder;
    }
    #endregion
}
