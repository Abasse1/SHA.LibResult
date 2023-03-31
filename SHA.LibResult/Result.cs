namespace SHA.LibResult;

public enum ResultState
{
    Success,
    Failed
}

public record Result<T>
{
    public ResultState State { get; private set; }

    public T? Value { get; private set; }

    public Exception? Exception { get; private set; }

    private Result()
    {
    }

    public static Result<T> Create(Exception exception)
    {
        return new Result<T>
        {
            Value = default,
            State = ResultState.Failed,
            Exception = exception
        };
    }

    public static Result<T> Create(T value)
    {
        return new Result<T>
        {
            Value = value,
            State = ResultState.Success,
            Exception = null
        };
    }

    public bool IsSuccess => State == ResultState.Success;

    public bool IsFail => State == ResultState.Failed;

    public static implicit operator Result<T>(T value)
    {
        return Result<T>.Create(value);
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
        return IsSuccess ? Result<TypeToMap>.Create(f(Value)) : Result<TypeToMap>.Create(Exception!);
    }
}