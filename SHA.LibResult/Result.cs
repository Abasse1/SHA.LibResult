namespace SHA.LibResult;

public enum ResultState
{
    Success,
    Failed
}

public record Result<T>
{
    public ResultState State { get; }

    public T? Value { get; }

    public Exception? Exception { get; }

    private Result()
    {
    }

    public Result(T value)
    {
        Value = value;
        State = ResultState.Success;
        Exception = null;
    }

    public Result(Exception exception)
    {
        Value = default;
        State = ResultState.Failed;
        Exception = exception;
    }

    public bool IsSuccess => State == ResultState.Success;

    public bool IsFail => State == ResultState.Failed;

    public static implicit operator Result<T>(T value)
    {
        return new(value);
    }

    public static implicit operator T(Result<T> result)
    {
        return result.Value!;
    }

    public TResult Match<TResult>(Func<T, TResult> success, Func<Exception, TResult> fail)
    {
        return IsSuccess ? success(Value!) : fail(Exception!);
    }

    public TResult Match<TResult>(Func<TResult> success, Func<Exception, TResult> fail)
    {
        return IsSuccess ? success() : fail(Exception!);
    }

    public async Task<TResult> MatchAsync<TResult>(Func<T, Task<TResult>> success, Func<Exception, TResult> fail)
    {
        return IsSuccess ? await success(Value!) : fail(Exception!);
    }
    public async Task<TResult> MatchAsync<TResult>(Func<Task<TResult>> success, Func<Exception, TResult> fail)
    {
        return IsSuccess ? await success() : fail(Exception!);
    }
    public Result<TypeToMap> Map<TypeToMap>(Func<T?, TypeToMap> f)
    {
        return IsSuccess ? new Result<TypeToMap>(f(Value)) : new Result<TypeToMap>(Exception!);
    }
}