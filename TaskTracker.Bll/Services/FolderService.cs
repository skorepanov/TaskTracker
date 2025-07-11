namespace TaskTracker.Bll.Services;

public class FolderService(
    IFolderRepository folderRepository,
    IDateTimeProvider dateTimeProvider)
{
    public async Task<Result<FolderVm>> GetFolderById(int folderId)
    {
        var folder = await folderRepository.GetFolder(folderId);
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
        var newFolderResult = Folder.CreateFolder(folderDto, dateTimeProvider.UtcNow);

        if (!newFolderResult.IsOk)
        {
            return Result.Failure<FolderVm>(newFolderResult.Error ?? string.Empty);
        }

        await folderRepository.CreateFolder(newFolderResult.Value);

        var newFolderVm = new FolderVm(newFolderResult.Value);
        return Result<FolderVm>.Success(newFolderVm);
    }

    public async Task<Result<FolderVm>> UpdateFolder(
        int folderId, FolderForUpdateDto folderDto)
    {
        var folder = await folderRepository.GetFolder(folderId);
        var updateResult = folder.UpdateFolder(folderDto, dateTimeProvider.UtcNow);

        if (!updateResult.IsOk)
        {
            return Result.Failure<FolderVm>(updateResult.Error ?? string.Empty);
        }

        await folderRepository.UpdateFolder(folder);

        var folderVm = new FolderVm(folder);
        return Result<FolderVm>.Success(folderVm);
    }

    public async Task<Result> DeleteFolder(int folderId)
    {
        var folder = await folderRepository.GetFolder(folderId);
        await folderRepository.DeleteFolder(folder);
        return Result.Success();
    }
}
