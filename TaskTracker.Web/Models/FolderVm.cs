using TaskTracker.Bll;

namespace TaskTracker.Web;

public class FolderVm
{
    public int Id { get; }
    public string Title { get; }
    public int IncompleteTaskCount { get; }

    public FolderVm(Folder folder)
    {
        this.Id = folder.Id;
        this.Title = folder.Title;
        this.IncompleteTaskCount = folder.IncompleteTaskCount;
    }

    public static IReadOnlyCollection<FolderVm> CreateCollectionFrom(IEnumerable<Folder> folders)
    {
        return folders.Select(f => new FolderVm(f)).ToList();
    }
}
