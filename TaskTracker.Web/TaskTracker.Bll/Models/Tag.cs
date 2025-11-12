namespace TaskTracker.Bll.Models;

public class Tag
{
    public int Id { get; init; }

    public string Title { get; private set; }

    public string Color { get; private set; }

    public DateTime CreatedDateTime { get; private set; }

    public DateTime? ModifiedDateTime { get; private set; }

    public List<UserTask> UserTasks { get; init; } = [];

    private Tag(string title, string color, DateTime createdDateTime)
    {
        Title = title;
        Color = color;
        CreatedDateTime = createdDateTime;
    }

    public static Tag CreateTag(TagForCreationDto tagDto, DateTime now)
    {
        var validationErrors = new List<string>();

        ValidateTitle(tagDto.Title, validationErrors);
        ValidateColor(tagDto.Color, validationErrors);

        if (validationErrors.Count > 0)
        {
            var error = string.Join(separator: ", ",  validationErrors);
            throw new DomainException(error);
        }

        var normalizedTitle = tagDto.Title!.Trim();
        var normalizedColor = tagDto.Color!.Trim();
        var createdDateTime = tagDto.CreatedDateTime ?? now;

        var tag = new Tag(normalizedTitle, normalizedColor, createdDateTime);
        return tag;
    }

    public void UpdateTag(TagForUpdateDto tagDto, DateTime now)
    {
        var validationErrors = new List<string>();

        ValidateTitle(tagDto.Title, validationErrors);
        ValidateColor(tagDto.Color, validationErrors);

        if (validationErrors.Count > 0)
        {
            var error = string.Join(separator: ", ",  validationErrors);
            throw new DomainException(error);
        }

        Title = tagDto.Title!.Trim();
        Color = tagDto.Color!.Trim();
        ModifiedDateTime = tagDto.ModifiedDateTime ?? now;
    }

    private static void ValidateTitle(string? title, List<string> errors)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            errors.Add(ErrorMessages.TagTitleIsEmpty);
        }
    }

    private static void ValidateColor(string? color, List<string> errors)
    {
        if (string.IsNullOrWhiteSpace(color))
        {
            errors.Add(ErrorMessages.TagColorIsEmpty);
        }
    }
}
