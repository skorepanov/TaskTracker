namespace TaskTracker.Dal.Repositories;

public class FolderRepository(ApplicationContext db) : IFolderRepository
{
    public async Task<Folder?> GetFolder(int folderId)
    {
        return await db.Folders.SingleOrDefaultAsync(f => f.Id == folderId);
    }

    public async Task<IReadOnlyList<Folder>> GetFolders()
    {
        return await db.Folders.ToListAsync();
    }

    public async Task CreateFolder(Folder folder)
    {
        db.Folders.Add(folder);
        await db.SaveChangesAsync();
    }

    public async Task UpdateFolder(Folder folder)
    {
        db.Folders.Update(folder);
        await db.SaveChangesAsync();
    }

    public async Task DeleteFolder(Folder folder)
    {
        db.Folders.Remove(folder);
        await db.SaveChangesAsync();
    }
}