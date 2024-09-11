using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace HBLibrary.Common.Results;
[DebuggerDisplay("State = {ResultState}, Value = {Value}, Error = {Error}")]
public readonly struct Result<TValue, TError> : IEquatable<Result<TValue, TError>>, IEquatable<TValue> {
    public ResultState ResultState { get; }
    public bool IsSuccess => ResultState == ResultState.Success;
    public bool IsFaulted => ResultState == ResultState.Faulted;
    public TValue? Value { get; }
    public TError? Error { get; }

    private Result(ResultState resultState, TValue? value, TError? error) {
        ResultState = resultState;
        Value = value;
        Error = error;
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TValue, TError> Ok(TValue value) => new Result<TValue, TError>(ResultState.Success, value, default);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TValue, TError> Fail(TError value) => new Result<TValue, TError>(ResultState.Faulted, default, value);

    public static implicit operator Result<TValue, TError>(TValue value) => new Result<TValue, TError>(ResultState.Success, value, default);
    public static implicit operator Result<TValue, TError>(TError error) => new Result<TValue, TError>(ResultState.Faulted, default, error);

    [Pure]
    public void Deconstruct(out ResultState resultState, out TValue? value, out TError? error) {
        resultState = ResultState;
        value = Value;
        error = Error;
    }

    [Pure]
    public R Match<R>(Func<TValue, R> success, Func<TError, R> failure) {
        return ResultState == ResultState.Success
               ? success(Value!)
               : failure(Error!);
    }

    [Pure]
    public Task<R> MatchAsync<R>(Func<TValue, Task<R>> success, Func<TError, Task<R>> failure) {
        return ResultState == ResultState.Success
               ? success(Value!)
               : failure(Error!);
    }

    [Pure]
    public Result<U, TError> Map<U>(Func<TValue, U> mapFunc) {
        return ResultState == ResultState.Success
            ? Result<U, TError>.Ok(mapFunc(Value!))
            : Result<U, TError>.Fail(Error!);
    }

    [Pure]
    public async Task<Result<U, TError>> MapAsync<U>(Func<TValue, Task<U>> mapFunc) {
        if (ResultState == ResultState.Success) {
            U result = await mapFunc(Value!);
            return Result<U, TError>.Ok(result);
        }

        return Result<U, TError>.Fail(Error!);
    }

    [Pure]
    public Result<TValue, U> MapError<U>(Func<TError, U> mapErrorFunc) {
        return ResultState == ResultState.Faulted
            ? Result<TValue, U>.Fail(mapErrorFunc(Error!))
            : Result<TValue, U>.Ok(Value!);
    }

    [Pure]
    public async Task<Result<TValue, U>> MapErrorAsync<U>(Func<TError, Task<U>> mapErrorFunc) {
        if (ResultState == ResultState.Faulted) {
            U result = await mapErrorFunc(Error!);
            return Result<TValue, U>.Fail(result);
        }

        return Result<TValue, U>.Ok(Value!);
    }

    [Pure]
    public Result<U, TError> Bind<U>(Func<TValue, Result<U, TError>> bindFunc) {
        return ResultState == ResultState.Success
            ? bindFunc(Value!)
            : Result<U, TError>.Fail(Error!);
    }

    [Pure]
    public Task<Result<U, TError>> BindAsync<U>(Func<TValue, Task<Result<U, TError>>> bindFunc) {
        return ResultState == ResultState.Success
            ? bindFunc(Value!)
            : Task.FromResult(Result<U, TError>.Fail(Error!));
    }

    [Pure]
    public Result<TValue, TError> Tap(Action<TValue> successAction) {
        if (ResultState == ResultState.Success) {
            successAction(Value!);
        }

        return this;
    }

    [Pure]
    public async Task<Result<TValue, TError>> TapAsync(Func<TValue, Task> successAction) {
        if (ResultState == ResultState.Success) {
            await successAction(Value!);
        }

        return this;
    }

    [Pure]
    public Result<TValue, TError> TapError(Action<TError> errorAction) {
        if (ResultState == ResultState.Faulted) {
            errorAction(Error!);
        }

        return this;
    }
    
    [Pure]
    public async Task<Result<TValue, TError>> TapErrorAsync(Func<TError, Task> errorAction) {
        if (ResultState == ResultState.Faulted) {
            await errorAction(Error!);
        }

        return this;
    }

    [Pure]
    public Result<TValue, TError> Recover(Func<TError, TValue> recoveryFunc) {
        if (ResultState == ResultState.Faulted) {
            return Result<TValue, TError>.Ok(recoveryFunc(Error!));
        }

        return this;
    }

    [Pure]
    public async Task<Result<TValue, TError>> RecoverAsync(Func<TError, Task<TValue>> recoveryFunc) {
        if (ResultState == ResultState.Faulted) {
            TValue recoveredValue = await recoveryFunc(Error!);
            return Result<TValue, TError>.Ok(recoveredValue);
        }

        return this;
    }

    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) {
        return obj is Result<TValue, TError> other && Equals(other) ||
            obj is TValue otherValue && Equals(otherValue);
    }

    [Pure]
    public bool Equals(Result<TValue, TError> other) {
        return ResultState == other.ResultState &&
             EqualityComparer<TValue?>.Default.Equals(Value, other.Value) &&
             EqualityComparer<TError?>.Default.Equals(Error, other.Error);
    }

    [Pure]
    public bool Equals(TValue? other) {
        return ResultState == ResultState.Success &&
            EqualityComparer<TValue?>.Default.Equals(Value, other);
    }

    [Pure]
    public static bool operator ==(Result<TValue, TError> left, Result<TValue, TError> right) {
        return left.Equals(right);
    }

    [Pure]
    public static bool operator !=(Result<TValue, TError> left, Result<TValue, TError> right) {
        return !(left == right);
    }

    [Pure]
    public static bool operator ==(TValue left, Result<TValue, TError> right) {
        return (left?.Equals(right)).GetValueOrDefault();
    }

    [Pure]
    public static bool operator !=(TValue left, Result<TValue, TError> right) {
        return !(left == right);
    }

    [Pure]
    public override int GetHashCode() {
        return HBHashCode.Combine(ResultState, Value, Error);
    }
}

[DebuggerDisplay("State = {ResultState}, Value = {Value}, Error = {Error}")]
public readonly struct Result<TValue> : IEquatable<Result<TValue>>, IEquatable<TValue> {
    public ResultState ResultState { get; }
    public bool IsSuccess => ResultState == ResultState.Success;
    public bool IsFaulted => ResultState == ResultState.Faulted;
    public TValue? Value { get; }
    public Exception? Error { get; }

    private Result(ResultState resultState, TValue? value, Exception? error) {
        ResultState = resultState;
        Value = value;
        Error = error;
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TValue> Ok(TValue value) => new Result<TValue>(ResultState.Success, value, default);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TValue> Fail(Exception value) => new Result<TValue>(ResultState.Faulted, default, value);


    public static implicit operator Result<TValue>(TValue value) => new Result<TValue>(ResultState.Success, value, default);
    public static implicit operator Result<TValue>(Exception error) => new Result<TValue>(ResultState.Faulted, default, error);

    [Pure]
    public void Deconstruct(out ResultState resultState, out TValue? value, out Exception? error) {
        resultState = ResultState;
        value = Value;
        error = Error;
    }

    [Pure]
    public R Match<R>(Func<TValue, R> success, Func<Exception, R> failure) {
        return IsSuccess
        ? success(Value!)
        : failure(Error!);
    }

    [Pure]
    public Task<R> MatchAsync<R>(Func<TValue, Task<R>> success, Func<Exception, Task<R>> failure) {
        return IsSuccess
        ? success(Value!)
        : failure(Error!);
    }

    [Pure]
    public Result<U> Map<U>(Func<TValue, U> mapFunc) {
        return IsSuccess
            ? Result<U>.Ok(mapFunc(Value!))
            : Result<U>.Fail(Error!);
    }

    [Pure]
    public async Task<Result<U>> MapAsync<U>(Func<TValue, Task<U>> mapFunc) {
        if (IsSuccess) {
            U result = await mapFunc(Value!);
            return Result<U>.Ok(result);
        }

        return Result<U>.Fail(Error!);
    }

    [Pure]
    public Result<U> Bind<U>(Func<TValue, Result<U>> bindFunc) {
        return IsSuccess
            ? bindFunc(Value!)
            : Result<U>.Fail(Error!);
    }

    [Pure]
    public Task<Result<U>> BindAsync<U>(Func<TValue, Task<Result<U>>> bindFunc) {
        return ResultState == ResultState.Success
            ? bindFunc(Value!)
            : Task.FromResult(Result<U>.Fail(Error!));
    }

    [Pure]
    public Result<TValue> Tap(Action<TValue> successAction) {
        if (IsSuccess) {
            successAction(Value!);
        }
        return this;
    }

    [Pure]
    public async Task<Result<TValue>> TapAsync(Func<TValue, Task> successAction) {
        if (ResultState == ResultState.Success) {
            await successAction(Value!);
        }
        return this;
    }

    [Pure]
    public Result<TValue> TapError(Action<Exception> errorAction) {
        if (IsFaulted) {
            errorAction(Error!);
        }
        return this;
    }

    [Pure]
    public async Task<Result<TValue>> TapErrorAsync(Func<Exception, Task> errorAction) {
        if (ResultState == ResultState.Faulted) {
            await errorAction(Error!);
        }
        return this;
    }

    [Pure]
    public Result<TValue> Recover(Func<Exception, TValue> recoveryFunc) {
        if (IsFaulted) {
            return Result<TValue>.Ok(recoveryFunc(Error!));
        }
        return this;
    }

    [Pure]
    public async Task<Result<TValue>> RecoverAsync(Func<Exception, Task<TValue>> recoveryFunc) {
        if (ResultState == ResultState.Faulted) {
            TValue recoveredValue = await recoveryFunc(Error!);
            return Result<TValue>.Ok(recoveredValue);
        }

        return this;
    }

    [Pure]
    public void ThrowIfFaulted() {
        if(IsFaulted) {
            throw Error!;
        }
    }

    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) {
        return obj is Result<TValue> other && Equals(other) ||
            obj is TValue otherValue && Equals(otherValue);
    }

    [Pure]
    public bool Equals(Result<TValue> other) {
        return ResultState == other.ResultState &&
             EqualityComparer<TValue?>.Default.Equals(Value, other.Value) &&
             Error == other.Error;
    }

    [Pure]
    public bool Equals(TValue? other) {
        return ResultState == ResultState.Success &&
            EqualityComparer<TValue?>.Default.Equals(Value, other);
    }

    [Pure]
    public static bool operator ==(Result<TValue> left, Result<TValue> right) {
        return left.Equals(right);
    }

    [Pure]
    public static bool operator !=(Result<TValue> left, Result<TValue> right) {
        return !(left == right);
    }

    [Pure]
    public static bool operator ==(TValue left, Result<TValue> right) {
        return (left?.Equals(right)).GetValueOrDefault();
    }

    [Pure]
    public static bool operator !=(TValue left, Result<TValue> right) {
        return !(left == right);
    }

    [Pure]
    public override int GetHashCode() {
        return HBHashCode.Combine(ResultState, Value, Error);
    }
}

public enum ResultState : byte {
    Success,
    Faulted
}