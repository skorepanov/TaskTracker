namespace TaskTracker.Bll.Models;

public class Tag
{
    public int Id { get; }

    public string Title { get; private set; }

    public string Color { get; private set; }

    public DateTime CreatedDateTime { get; private set; }

    public DateTime? ModifiedDateTime { get; private set; }
}
