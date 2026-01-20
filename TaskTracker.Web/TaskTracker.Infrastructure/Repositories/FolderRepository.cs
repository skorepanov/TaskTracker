namespace TaskTracker.Infrastructure.Repositories;

public class FolderRepository(ApplicationContext db) : IFolderRepository
{
    public async Task<bool> IsFolderExists(int folderId)
    {
        return await db.Folders.AnyAsync(f => f.Id == folderId);
    }

    public async Task<Folder> GetFolder(int folderId)
    {
        var folder = await db.Folders.SingleOrDefaultAsync(f => f.Id == folderId);

        if (folder is null)
        {
            var error = ErrorMessages.FolderNotFound(folderId);
            throw new DomainException(error);
        }

        return folder;
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
