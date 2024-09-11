using System.Diagnostics.Contracts;

namespace HBLibrary.Common.Results;
public readonly struct Result<TValue, TError> {
    [Pure]
    public bool IsValid { get; }
    public TValue? Value { get; }
    public TError? Error { get; }

    public Result(bool isValid, TValue? value, TError? error) {
        IsValid = isValid;
        Value = value;
        Error = error;
    }

    public static Result<TValue, TError> Success(TValue value) => new Result<TValue, TError>(true, value, default);
    public static Result<TValue, TError> Failure(TError value) => new Result<TValue, TError>(true, default, value);

    public static implicit operator Result<TValue, TError>(TValue value) => new Result<TValue, TError>(true, value, default);
    public static implicit operator Result<TValue, TError>(TError error) => new Result<TValue, TError>(true, default, error);

    public void Deconstruct(out bool isValid, out TValue? value, out TError? error) {
        isValid = IsValid;
        value = Value;
        error = Error;
    }

    public R Match<R>(Func<TValue, R> success, Func<TError, R> failure) => IsValid ? success(Value!) : failure(Error!);
}

public readonly struct Result<TValue> {
    public bool IsValid { get; }
    public TValue? Value { get; }
    public Exception? Error { get; }

    public Result(bool isValid, TValue? value, Exception? error) {
        IsValid = isValid;
        Value = value;
        Error = error;
    }

    public static Result<TValue> Success(TValue value) => new Result<TValue>(true, value, default);
    public static Result<TValue> Failure(Exception value) => new Result<TValue>(true, default, value);


    public static implicit operator Result<TValue>(TValue value) => new Result<TValue>(true, value, default);
    public static implicit operator Result<TValue>(Exception error) => new Result<TValue>(true, default, error);

    public void Deconstruct(out bool isValid, out TValue? value, out Exception? error) {
        isValid = IsValid;
        value = Value;
        error = Error;
    }

    public R Match<R>(Func<TValue, R> success, Func<Exception, R> failure) => IsValid ? success(Value!) : failure(Error!);

}