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
        var normalizedTitle = folderDto.Title.Trim();
        var createdDateTime = folderDto.CreatedDateTime ?? now;

        return new Folder(normalizedTitle, createdDateTime);
    }

    public void UpdateFolder(FolderForUpdateDto folderDto, DateTime now)
    {
        Title = folderDto.Title.Trim();
        ModifiedDateTime = folderDto.ModifiedDateTime ?? now;
    }
}
