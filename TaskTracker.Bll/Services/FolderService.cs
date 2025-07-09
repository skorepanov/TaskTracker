namespace TaskTracker.Bll.Services;

public class FolderService(
    IFolderRepository folderRepository,
    IDateTimeProvider dateTimeProvider)
{
    public async Task<Result<FolderVm>> GetFolderById(int folderId)
    {
        var folder = await folderRepository.GetFolder(folderId);

        if (folder is null)
        {
            return Result.Failure<FolderVm>(
                error: $"Папка не обнаружена (id = {folderId})");
        }

        var folderVm = new FolderVm(folder);
        return Result<FolderVm>.Success(folderVm);
    }

    public async Task<Result<IReadOnlyList<FolderVm>>> GetFolders()
    {
        var folders = await folderRepository.GetFolders();
        var folderVms = folders.Select(f => new FolderVm(f)).ToList();
        return Result<IReadOnlyList<FolderVm>>.Success(folderVms);
    }

    public async Task<Result<FolderVm>> CreateFolder(FolderForCreationDto folderDto)
    {
        var newFolder = Folder.CreateFolder(folderDto, dateTimeProvider.UtcNow);
        await folderRepository.CreateFolder(newFolder);

        var newFolderVm = new FolderVm(newFolder);
        return Result<FolderVm>.Success(newFolderVm);
    }

    public async Task<Result<FolderVm>> UpdateFolder(
        int folderId, FolderForUpdateDto folderDto)
    {
        var folder = await folderRepository.GetFolder(folderId);

        if (folder is null)
        {
            return Result.Failure<FolderVm>(
                error: $"Папка не обнаружена (id = {folderId})");
        }

        folder.UpdateFolder(folderDto, dateTimeProvider.UtcNow);
        await folderRepository.UpdateFolder(folder);

        var folderVm = new FolderVm(folder);
        return Result<FolderVm>.Success(folderVm);
    }

    public async Task<Result> DeleteFolder(int folderId)
    {
        var folder = await folderRepository.GetFolder(folderId);

        if (folder is null)
        {
            return Result.Failure<FolderVm>(
                error: $"Папка не обнаружена (id = {folderId})");
        }

        await folderRepository.DeleteFolder(folder);

        return Result.Success();
    }
}
