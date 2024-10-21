using HBLibrary.Core;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace HBLibrary.DataStructures;
[DebuggerDisplay("State = {resultState}, Value = {value}, Error = {error}")]
public readonly struct Result<TValue, TError> : IEquatable<Result<TValue, TError>>, IEquatable<TValue> {
    private readonly TValue? value;
    private readonly TError? error;
    private readonly ResultState resultState;

    public bool IsSuccess => resultState == ResultState.Success;
    public bool IsFaulted => resultState == ResultState.Faulted;

    public Result(TValue value) {
        resultState = ResultState.Success;
        this.value = value;
    }

    public Result(TError error) {
        resultState = ResultState.Faulted;
        this.error = error;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TValue, TError> Ok(TValue value) => new Result<TValue, TError>(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TValue, TError> Fail(TError error) => new Result<TValue, TError>(error);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Result<TValue, TError>(TValue value) => new Result<TValue, TError>(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Result<TValue, TError>(TError error) => new Result<TValue, TError>(error);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Option<TValue>(Result<TValue, TError> result) => new Option<TValue>(result.value, result.IsSuccess);

    public void Deconstruct(out ResultState resultState, out TValue? value, out TError? error) {
        resultState = this.resultState;
        value = this.value;
        error = this.error;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Option<TValue> ToOption() {
        return IsSuccess ? Option<TValue>.Some(value!) : Option<TValue>.None();
    }

    public R Match<R>(Func<TValue, R> success, Func<TError, R> failure) {
        return resultState == ResultState.Success
               ? success(value!)
               : failure(error!);
    }

    public Task<R> MatchAsync<R>(Func<TValue, Task<R>> success, Func<TError, Task<R>> failure) {
        return resultState == ResultState.Success
               ? success(value!)
               : failure(error!);
    }

    public Result<U, TError> Map<U>(Func<TValue, U> mapFunc) {
        return resultState == ResultState.Success
            ? Result<U, TError>.Ok(mapFunc(value!))
            : Result<U, TError>.Fail(error!);
    }

    public async Task<Result<U, TError>> MapAsync<U>(Func<TValue, Task<U>> mapFunc) {
        if (resultState == ResultState.Success) {
            U result = await mapFunc(value!);
            return Result<U, TError>.Ok(result);
        }

        return Result<U, TError>.Fail(error!);
    }

    public Result<TValue, U> MapError<U>(Func<TError, U> mapErrorFunc) {
        return resultState == ResultState.Faulted
            ? Result<TValue, U>.Fail(mapErrorFunc(error!))
            : Result<TValue, U>.Ok(value!);
    }

    public async Task<Result<TValue, U>> MapErrorAsync<U>(Func<TError, Task<U>> mapErrorFunc) {
        if (resultState == ResultState.Faulted) {
            U result = await mapErrorFunc(error!);
            return Result<TValue, U>.Fail(result);
        }

        return Result<TValue, U>.Ok(value!);
    }

    public Result<U, TError> Bind<U>(Func<TValue, Result<U, TError>> bindFunc) {
        return resultState == ResultState.Success
            ? bindFunc(value!)
            : Result<U, TError>.Fail(error!);
    }

    public Task<Result<U, TError>> BindAsync<U>(Func<TValue, Task<Result<U, TError>>> bindFunc) {
        return resultState == ResultState.Success
            ? bindFunc(value!)
            : Task.FromResult(Result<U, TError>.Fail(error!));
    }

    public Result<TValue, TError> Tap(Action<TValue> successAction) {
        if (resultState == ResultState.Success) {
            successAction(value!);
        }

        return this;
    }

    public async Task<Result<TValue, TError>> TapAsync(Func<TValue, Task> successAction) {
        if (resultState == ResultState.Success) {
            await successAction(value!);
        }

        return this;
    }

    public Result<TValue, TError> TapError(Action<TError> errorAction) {
        if (resultState == ResultState.Faulted) {
            errorAction(error!);
        }

        return this;
    }

    public async Task<Result<TValue, TError>> TapErrorAsync(Func<TError, Task> errorAction) {
        if (resultState == ResultState.Faulted) {
            await errorAction(error!);
        }

        return this;
    }

    public Result<TValue, TError> Recover(Func<TError, TValue> recoveryFunc) {
        if (resultState == ResultState.Faulted) {
            return Result<TValue, TError>.Ok(recoveryFunc(error!));
        }

        return this;
    }

    public async Task<Result<TValue, TError>> RecoverAsync(Func<TError, Task<TValue>> recoveryFunc) {
        if (resultState == ResultState.Faulted) {
            TValue recoveredValue = await recoveryFunc(error!);
            return Result<TValue, TError>.Ok(recoveredValue);
        }

        return this;
    }

    public override bool Equals([NotNullWhen(true)] object? obj) {
        return obj is Result<TValue, TError> other && Equals(other) ||
            obj is TValue otherValue && Equals(otherValue);
    }

    public bool Equals(Result<TValue, TError> other) {
        return resultState == other.resultState &&
             EqualityComparer<TValue?>.Default.Equals(value, other.value) &&
             EqualityComparer<TError?>.Default.Equals(error, other.error);
    }

    public bool Equals(TValue? other) {
        return resultState == ResultState.Success &&
            EqualityComparer<TValue?>.Default.Equals(value, other);
    }

    public static bool operator ==(Result<TValue, TError> left, Result<TValue, TError> right) {
        return left.Equals(right);
    }

    public static bool operator !=(Result<TValue, TError> left, Result<TValue, TError> right) {
        return !(left == right);
    }

    public static bool operator ==(TValue left, Result<TValue, TError> right) {
        return (left?.Equals(right)).GetValueOrDefault();
    }

    public static bool operator !=(TValue left, Result<TValue, TError> right) {
        return !(left == right);
    }

    public override int GetHashCode() {
        return HBHashCode.Combine(resultState, value, error);
    }
}

[DebuggerDisplay("State = {resultState}, Value = {Value}, Error = {Error}")]
public readonly struct Result<TValue> : IEquatable<Result<TValue>>, IEquatable<TValue> {
    private readonly ResultState resultState;
    public TValue? Value { get; }
    public Exception? Error { get; }
    public bool IsSuccess => resultState == ResultState.Success;
    public bool IsFaulted => resultState == ResultState.Faulted;

    public Result(TValue value) {
        resultState = ResultState.Success;
        Value = value;
    }

    public Result(Exception error) {
        resultState = ResultState.Faulted;
        Error = error;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TValue> Ok(TValue value) => new Result<TValue>(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TValue> Fail(Exception error) => new Result<TValue>(error);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Result<TValue>(TValue value) => new Result<TValue>(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Result<TValue>(Exception error) => new Result<TValue>(error);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Option<TValue>(Result<TValue> result) => new Option<TValue>(result.Value, result.IsSuccess);

    public void Deconstruct(out ResultState resultState, out TValue? value, out Exception? error) {
        resultState = this.resultState;
        value = Value;
        error = Error;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Option<TValue> ToOption() {
        return IsSuccess ? Option<TValue>.Some(Value!) : Option<TValue>.None();
    }

    public R Match<R>(Func<TValue, R> success, Func<Exception, R> failure) {
        return IsSuccess
        ? success(Value!)
        : failure(Error!);
    }

    public Task<R> MatchAsync<R>(Func<TValue, Task<R>> success, Func<Exception, Task<R>> failure) {
        return IsSuccess
        ? success(Value!)
        : failure(Error!);
    }

    public Result<U> Map<U>(Func<TValue, U> mapFunc) {
        return IsSuccess
            ? Result<U>.Ok(mapFunc(Value!))
            : Result<U>.Fail(Error!);
    }

    public async Task<Result<U>> MapAsync<U>(Func<TValue, Task<U>> mapFunc) {
        if (IsSuccess) {
            U result = await mapFunc(Value!);
            return Result<U>.Ok(result);
        }

        return Result<U>.Fail(Error!);
    }

    public Result<U> Bind<U>(Func<TValue, Result<U>> bindFunc) {
        return IsSuccess
            ? bindFunc(Value!)
            : Result<U>.Fail(Error!);
    }

    public Task<Result<U>> BindAsync<U>(Func<TValue, Task<Result<U>>> bindFunc) {
        return resultState == ResultState.Success
            ? bindFunc(Value!)
            : Task.FromResult(Result<U>.Fail(Error!));
    }

    public Result<TValue> Tap(Action<TValue> successAction) {
        if (IsSuccess) {
            successAction(Value!);
        }
        return this;
    }

    public async Task<Result<TValue>> TapAsync(Func<TValue, Task> successAction) {
        if (resultState == ResultState.Success) {
            await successAction(Value!);
        }
        return this;
    }

    public Result<TValue> TapError(Action<Exception> errorAction) {
        if (IsFaulted) {
            errorAction(Error!);
        }
        return this;
    }

    public async Task<Result<TValue>> TapErrorAsync(Func<Exception, Task> errorAction) {
        if (resultState == ResultState.Faulted) {
            await errorAction(Error!);
        }
        return this;
    }

    public Result<TValue> Recover(Func<Exception, TValue> recoveryFunc) {
        if (IsFaulted) {
            return Result<TValue>.Ok(recoveryFunc(Error!));
        }
        return this;
    }

    public async Task<Result<TValue>> RecoverAsync(Func<Exception, Task<TValue>> recoveryFunc) {
        if (resultState == ResultState.Faulted) {
            TValue recoveredValue = await recoveryFunc(Error!);
            return Result<TValue>.Ok(recoveredValue);
        }

        return this;
    }


    public TValue GetValueOrThrow() {
        if (IsFaulted) {
            throw Error!;
        }

        return Value!;
    }
    public void ThrowIfFaulted() {
        if (IsFaulted) {
            throw Error!;
        }
    }

    public override bool Equals([NotNullWhen(true)] object? obj) {
        return obj is Result<TValue> other && Equals(other) ||
            obj is TValue otherValue && Equals(otherValue);
    }

    public bool Equals(Result<TValue> other) {
        return resultState == other.resultState &&
             EqualityComparer<TValue?>.Default.Equals(Value, other.Value) &&
             Error == other.Error;
    }

    public bool Equals(TValue? other) {
        return resultState == ResultState.Success &&
            EqualityComparer<TValue?>.Default.Equals(Value, other);
    }

    public static bool operator ==(Result<TValue> left, Result<TValue> right) {
        return left.Equals(right);
    }

    public static bool operator !=(Result<TValue> left, Result<TValue> right) {
        return !(left == right);
    }

    public static bool operator ==(TValue left, Result<TValue> right) {
        return (left?.Equals(right)).GetValueOrDefault();
    }

    public static bool operator !=(TValue left, Result<TValue> right) {
        return !(left == right);
    }

    public override int GetHashCode() {
        return HBHashCode.Combine(resultState, Value, Error);
    }
}

[DebuggerDisplay("State = {resultState}")]
public readonly struct Result : IEquatable<Result> {
    private readonly ResultState resultState;
    public bool IsSuccess => resultState == ResultState.Success;
    public bool IsFaulted => resultState == ResultState.Faulted;
    public string? Message { get; }
    public Exception? Exception { get; }

    public Result(ResultState resultState, string? message, Exception? exception) {
        this.resultState = resultState;
        Message = message;
        Exception = exception;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result Ok() => new Result(ResultState.Success, null, null);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result Ok(string message) => new Result(ResultState.Success, message, null);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result Fail(string message) => new Result(ResultState.Faulted, message, new Exception(message));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result Fail(Exception exception) => new Result(ResultState.Faulted, exception.Message, exception);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result Fail(string message, Exception exception) => new Result(ResultState.Faulted, message, exception);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Result(Exception error) => new Result(ResultState.Faulted, default, error);

    public void ThrowIfFaulted() {
        if (IsFaulted) {
            throw Exception!;
        }
    }

    public R Match<R>(Func<R> success, Func<Exception, R> failure) {
        return IsSuccess
        ? success()
        : failure(Exception!);
    }

    public Task<R> MatchAsync<R>(Func<Task<R>> success, Func<Exception, Task<R>> failure) {
        return IsSuccess
        ? success()
        : failure(Exception!);
    }

    public Result TapError(Action<Exception> errorAction) {
        if (IsFaulted) {
            errorAction(Exception!);
        }
        return this;
    }

    public async Task<Result> TapErrorAsync(Func<Exception, Task> errorAction) {
        if (resultState == ResultState.Faulted) {
            await errorAction(Exception!);
        }
        return this;
    }


    public bool Equals(Result other) {
        return resultState == other.resultState &&
            Message == other.Message &&
            Exception == other.Exception;
    }

    public override bool Equals(object? obj) {
        return obj is Result res && Equals(res);
    }

    public override int GetHashCode() {
        return HBHashCode.Combine(resultState, Message, Exception);
    }

    public static bool operator ==(Result left, Result right) {
        return left.Equals(right);
    }

    public static bool operator !=(Result left, Result right) {
        return !(left == right);
    }

    public void Deconstruct(out ResultState resultState, out string? message, out Exception? exception) {
        resultState = this.resultState;
        message = Message;
        exception = Exception;
    }

    public void Deconstruct(out string? message, out Exception? exception) {
        message = Message;
        exception = Exception;
    }
}

public enum ResultState : byte {
    Success,
    Faulted
}