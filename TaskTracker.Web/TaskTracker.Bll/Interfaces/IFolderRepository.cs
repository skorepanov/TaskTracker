namespace TaskTracker.Bll.Interfaces;

public interface IFolderRepository
{
    Task<bool> IsFolderExists(int folderId);

    Task<Folder> GetFolder(int folderId);

    Task<IReadOnlyList<Folder>> GetFolders();

    Task CreateFolder(Folder folder);

    Task UpdateFolder(Folder folder);

    Task DeleteFolder(Folder folder);
}