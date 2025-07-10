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

    public static Result<Folder> CreateFolder(FolderForCreationDto folderDto, DateTime now)
    {
        if (string.IsNullOrWhiteSpace(folderDto.Title))
        {
            return Result.Failure<Folder>(
                error: "Не заполнено название папки");
        }

        var normalizedTitle = folderDto.Title.Trim();
        var createdDateTime = folderDto.CreatedDateTime ?? now;

        var folder = new Folder(normalizedTitle, createdDateTime);
        return Result<Folder>.Success(folder);
    }

    public Result UpdateFolder(FolderForUpdateDto folderDto, DateTime now)
    {
        if (string.IsNullOrWhiteSpace(folderDto.Title))
        {
            return Result.Failure(error: "Не заполнено название папки");
        }

        Title = folderDto.Title.Trim();
        ModifiedDateTime = folderDto.ModifiedDateTime ?? now;

        return Result.Success();
    }
}
