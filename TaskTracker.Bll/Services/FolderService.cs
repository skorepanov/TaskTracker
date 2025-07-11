namespace TaskTracker.Bll.Services;

public class FolderService(
    IFolderRepository folderRepository,
    IDateTimeProvider dateTimeProvider)
{
    public async Task<FolderVm> GetFolderById(int folderId)
    {
        var folder = await folderRepository.GetFolder(folderId);
        var folderVm = new FolderVm(folder);
        return folderVm;
    }

    public async Task<IReadOnlyList<FolderVm>> GetFolders()
    {
        var folders = await folderRepository.GetFolders();
        var folderVms = folders.Select(f => new FolderVm(f)).ToList();
        return folderVms;
    }

    public async Task<FolderVm> CreateFolder(FolderForCreationDto folderDto)
    {
        var newFolder = Folder.CreateFolder(folderDto, dateTimeProvider.UtcNow);

        await folderRepository.CreateFolder(newFolder);

        var newFolderVm = new FolderVm(newFolder);
        return newFolderVm;
    }

    public async Task<FolderVm> UpdateFolder(
        int folderId, FolderForUpdateDto folderDto)
    {
        var folder = await folderRepository.GetFolder(folderId);
        folder.UpdateFolder(folderDto, dateTimeProvider.UtcNow);

        await folderRepository.UpdateFolder(folder);

        var folderVm = new FolderVm(folder);
        return folderVm;
    }

    public async Task<bool> DeleteFolder(int folderId)
    {
        var folder = await folderRepository.GetFolder(folderId);
        await folderRepository.DeleteFolder(folder);
        return true;
    }
}
