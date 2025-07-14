namespace TaskTracker.Bll.Models;

public class Folder
{
    public int Id { get; }

    public string Title { get; private set; }

    public DateTime CreatedDateTime { get; private set; }

    public DateTime? ModifiedDateTime { get; private set; }

    private Folder(string title, DateTime createdDateTime)
    {
        Title = title;
        CreatedDateTime = createdDateTime;
    }

    public static Folder CreateFolder(FolderForCreationDto folderDto, DateTime now)
    {
        var validationErrors = new List<string>();

        ValidateTitle(folderDto.Title, validationErrors);

        if (validationErrors.Count > 0)
        {
            var error = string.Join(separator: ", ",  validationErrors);
            throw new DomainException(error);
        }

        var normalizedTitle = folderDto.Title.Trim();
        var createdDateTime = folderDto.CreatedDateTime ?? now;

        var folder = new Folder(normalizedTitle, createdDateTime);
        return folder;
    }

    public void UpdateFolder(FolderForUpdateDto folderDto, DateTime now)
    {
        var validationErrors = new List<string>();

        ValidateTitle(folderDto.Title, validationErrors);

        if (validationErrors.Count > 0)
        {
            var error = string.Join(separator: ", ",  validationErrors);
            throw new DomainException(error);
        }

        Title = folderDto.Title.Trim();
        ModifiedDateTime = folderDto.ModifiedDateTime ?? now;
    }

    private static void ValidateTitle(string? title, List<string> errors)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            errors.Add(ErrorMessages.FolderTitleIsEmpty);
        }
    }
}
