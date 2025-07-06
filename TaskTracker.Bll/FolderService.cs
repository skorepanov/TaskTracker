namespace TaskTracker.Bll;

public class FolderService(
    IFolderRepository folderRepository,
    IDateTimeProvider dateTimeProvider)
{
    public async Task<Folder> GetFolderById(int folderId)
    {
        var folder = await folderRepository.GetFolder(folderId);

        if (folder is null)
        {
            throw new DomainEntityNotFoundException(
                domainEntityType: typeof(Folder),
                message: $"Папка не обнаружена (id = {folderId})");
        }

        return folder;
    }

    public async Task<IReadOnlyList<Folder>> GetFolders()
    {
        var folders = await folderRepository.GetFolders();
        return folders;
    }

    public async Task<Folder> CreateFolder(FolderForCreationDto folderDto)
    {
        var newFolder = Folder.CreateFolder(folderDto, dateTimeProvider.UtcNow);
        await folderRepository.CreateFolder(newFolder);

        return newFolder;
    }

    public async Task<Folder> UpdateFolder(int folderId, FolderForUpdateDto folderDto)
    {
        var folder = await folderRepository.GetFolder(folderId);

        if (folder is null)
        {
            throw new DomainEntityNotFoundException(
                domainEntityType: typeof(Folder),
                message: $"Папка не обнаружена (id = {folderId})");
        }

        folder.UpdateFolder(folderDto, dateTimeProvider.UtcNow);
        await folderRepository.UpdateFolder(folder);

        return folder;
    }

    public async Task DeleteFolder(int folderId)
    {
        var folder = await folderRepository.GetFolder(folderId);

        if (folder is null)
        {
            throw new DomainEntityNotFoundException(
                domainEntityType: typeof(Folder),
                message: $"Папка не обнаружена (id = {folderId})");
        }

        await folderRepository.DeleteFolder(folder);
    }
}
