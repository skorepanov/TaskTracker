namespace TaskTracker.Web.Models;

/// <summary>
/// Результат выполнения запроса
/// </summary>
public class ApiResponse<T>
{
    /// <summary>
    /// Запрос выполнена успешно?
    /// </summary>
    public bool IsOk { get; init; }

    /// <summary>
    /// Результат запроса
    /// </summary>
    public T? Result { get; init; }

    /// <summary>
    /// Сообщение об ошибке запроса
    /// </summary>
    public string? Error { get; init; }

    /// <summary>
    /// Создать успешный результат
    /// </summary>
    public static ApiResponse<T> Success(T? result)
    {
        return new ApiResponse<T>
        {
            IsOk = true,
            Result = result,
        };
    }

    /// <summary>
    /// Создать результат с ошибкой
    /// </summary>
    public static ApiResponse<T> Failure(string error)
    {
        return new ApiResponse<T>
        {
            IsOk = false,
            Error = error,
        };
    }
}