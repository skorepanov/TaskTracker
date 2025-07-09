namespace TaskTracker.Bll.Models;

/// <summary>
/// Результат выполнения запроса
/// </summary>
public class Result
{
    /// <summary>
    /// Запрос выполнен успешно?
    /// </summary>
    public bool IsOk { get; init; }

    /// <summary>
    /// Сообщение об ошибке запроса
    /// </summary>
    public string? Error { get; init; }

    /// <summary>
    /// Создать успешный результат
    /// </summary>
    public static Result Success()
    {
        return new Result
        {
            IsOk = true
        };
    }

    /// <summary>
    /// Создать результат с ошибкой
    /// </summary>
    public static Result Failure(string error)
    {
        return new Result
        {
            IsOk = false,
            Error = error,
        };
    }

    /// <summary>
    /// Создать обобщённый результат с ошибкой
    /// </summary>
    public static Result<T> Failure<T>(string error)
    {
        return new Result<T>
        {
            IsOk = false,
            Error = error,
        };
    }
}

/// <summary>
/// Обобщённый результат выполнения запроса
/// </summary>
public class Result<T> : Result
{
    /// <summary>
    /// Результат запроса
    /// </summary>
    public T? Value { get; init; }

    /// <summary>
    /// Создать обобщённый успешный результат
    /// </summary>
    public static Result<T> Success(T? value)
    {
        return new Result<T>
        {
            IsOk = true,
            Value = value,
        };
    }
}