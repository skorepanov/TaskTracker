namespace TaskTracker.Application.Services;

public class ExecutionService
{
    public async Task<Result<T>> TryExecute<T>(Func<Task<T>> callback)
    {
        try
        {
            var result = await callback();
            return Result<T>.Success(result);
        }
        catch (Exception e)
        {
            return Result<T>.Failure(e.Message);
        }
    }
}
