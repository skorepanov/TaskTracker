using System.Threading.Tasks;

namespace TaskTracker.Bll;

public interface IFolderRepository
{
    Task<Folder?> GetFolder(int folderId);
    Task<IReadOnlyList<Folder>> GetFolders();

    Task CreateFolder(Folder folder);

    Task UpdateFolder(Folder folder);
}