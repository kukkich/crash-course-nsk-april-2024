namespace Market.Misc;

public readonly struct Result<TValue, TError>
{
    public bool IsFailure { get; }
    public bool IsSucceed => !IsFailure;
    public TValue? Value => _value;
    public TError? Error => _error;

    private readonly TValue? _value;
    private readonly TError? _error;
    
    public Result(TValue value)
    {
        _value = value;
        _error = default;

        IsFailure = false;
    }

    public Result(TError error)
    {
        _error = error;
        _value = default;

        IsFailure = true;
    }

    public static implicit operator Result<TValue, TError>(TValue value) => new(value);

    public static implicit operator Result<TValue, TError>(TError error) => new(error);

    public T Match<T>(Func<TValue, T> onSuccess, Func<TError, T> onFailure) 
        => IsFailure ? onFailure(_error!) : onSuccess(_value!);
}