namespace TaskTracker.Bll;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
