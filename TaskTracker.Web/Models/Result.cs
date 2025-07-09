namespace TaskTracker.Web.Models;

/// <summary>
/// Результат выполнения запроса
/// </summary>
public class Result<T>
{
    /// <summary>
    /// Запрос выполнена успешно?
    /// </summary>
    public bool IsOk { get; init; }

    /// <summary>
    /// Результат запроса
    /// </summary>
    public T? Value { get; init; }

    /// <summary>
    /// Сообщение об ошибке запроса
    /// </summary>
    public string? Error { get; init; }

    /// <summary>
    /// Создать успешный результат
    /// </summary>
    public static Result<T> Success(T? value)
    {
        return new Result<T>
        {
            IsOk = true,
            Value = value,
        };
    }

    /// <summary>
    /// Создать результат с ошибкой
    /// </summary>
    public static Result<T> Failure(string error)
    {
        return new Result<T>
        {
            IsOk = false,
            Error = error,
        };
    }
}