namespace TaskTracker.Bll.Models;

public class Tag
{
    public int Id { get; }

    public string Title { get; private set; }

    public string Color { get; private set; }

    public DateTime CreatedDateTime { get; private set; }

    public DateTime? ModifiedDateTime { get; private set; }

    private Tag(string title, string color, DateTime createdDateTime)
    {
        Title = title;
        Color = color;
        CreatedDateTime = createdDateTime;
    }

    public static Tag CreateTag(TagForCreationDto tagDto, DateTime now)
    {
        var normalizedTitle = tagDto.Title.Trim();
        var normalizedColor = tagDto.Color.Trim();
        var createdDateTime = tagDto.CreatedDateTime ?? now;

        return new Tag(normalizedTitle, normalizedColor, createdDateTime);
    }

    public void UpdateTag(TagForUpdateDto tagDto, DateTime now)
    {
        Title = tagDto.Title.Trim();
        Color = tagDto.Color.Trim();
        ModifiedDateTime = tagDto.ModifiedDateTime ?? now;
    }
}
