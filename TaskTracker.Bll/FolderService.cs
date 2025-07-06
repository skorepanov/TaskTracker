namespace TaskTracker.Bll;

public class FolderService(
    IFolderRepository _folderRepository,
    IDateTimeProvider _dateTimeProvider)
{
    public async Task<Folder> GetFolderById(int folderId)
    {
        var folder = await _folderRepository.GetFolder(folderId);

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
        var folders = await _folderRepository.GetFolders();
        return folders;
    }

    public async Task<Folder> CreateFolder(FolderForCreationDto folderDto)
    {
        var newFolder = Folder.CreateFolder(folderDto, _dateTimeProvider.UtcNow);
        await _folderRepository.CreateFolder(newFolder);

        return newFolder;
    }

    public async Task<Folder> UpdateFolder(int folderId, FolderForUpdateDto folderDto)
    {
        var folder = await _folderRepository.GetFolder(folderId);

        if (folder is null)
        {
            throw new DomainEntityNotFoundException(
                domainEntityType: typeof(Folder),
                message: $"Папка не обнаружена (id = {folderId})");
        }

        folder.UpdateFolder(folderDto, _dateTimeProvider.UtcNow);
        await _folderRepository.UpdateFolder(folder);

        return folder;
    }

    public async Task DeleteFolder(int folderId)
    {
        var folder = await _folderRepository.GetFolder(folderId);

        if (folder is null)
        {
            throw new DomainEntityNotFoundException(
                domainEntityType: typeof(Folder),
                message: $"Папка не обнаружена (id = {folderId})");
        }

        await _folderRepository.DeleteFolder(folder);
    }
}
