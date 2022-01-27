namespace TaskTracker.Web;

public class FolderVm
{
    public int Id { get; }
    public string Title { get; }
    public int IncompletedTaskCount { get; }

    public FolderVm(Folder folder)
    {
        this.Id = folder.Id;
        this.Title = folder.Title;
        this.IncompletedTaskCount = folder.IncompleteTaskCount;
    }
}
