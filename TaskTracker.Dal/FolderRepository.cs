using Microsoft.EntityFrameworkCore;
using TaskTracker.Bll;
using TaskTracker.Bll.Models;

namespace TaskTracker.Dal;

public class FolderRepository(ApplicationContext _db) : IFolderRepository
{
    public async Task<Folder?> GetFolder(int folderId)
    {
        return await _db.Folders.SingleOrDefaultAsync(f => f.Id == folderId);
    }

    public async Task<IReadOnlyList<Folder>> GetFolders()
    {
        return await _db.Folders.ToListAsync();
    }

    public async Task CreateFolder(Folder folder)
    {
        _db.Folders.Add(folder);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateFolder(Folder folder)
    {
        _db.Folders.Update(folder);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteFolder(Folder folder)
    {
        _db.Folders.Remove(folder);
        await _db.SaveChangesAsync();
    }
}