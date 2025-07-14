namespace TaskTracker.Bll.Models;

public class Result<T>
{
    public bool IsOk { get; init; }

    public string? Error { get; init; }

    public T? Value { get; init; }

    public static Result<T> Success(T value)
    {
        return new Result<T>
        {
            IsOk = true,
            Value = value,
        };
    }

    public static Result<T> Failure(string error)
    {
        return new Result<T>
        {
            IsOk = false,
            Error = error,
        };
    }
}